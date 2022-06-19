using AngleSharp.Html.Parser;
using ConsoleParser.Repository;
using ConsoleParser.Repository.DBO;
using ConsoleParser.Repository.DBO.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

//см 1) Паттерны проектирования в Repository => Base
//см 8) Кэши в WApi (WeatherForecastController)


var parser = new Parser("https://gismeteo.ru");
var dbConnectionString = "Server=localhost;Port=3306;Database=weatherappscheme;Uid=root;Pwd=root;";

var angleParser = new HtmlParser();

var completeForecastData = new ConcurrentBag<WeatherForecast>(); //3) Потокобезопасные коллекции
var allProcessesResult = new ConcurrentList<bool>(); //4) Примитивы синхронизации

while (true)
{
    var mainPageContent = await parser.GetContent();

    if (!string.IsNullOrEmpty(mainPageContent))
    {
        var rawDocument = angleParser.ParseDocument(mainPageContent);
        var cityListContent = rawDocument.QuerySelectorAll("div")
            .Where(x => x.GetAttribute("class") != null
            && Regex.Replace(x.GetAttribute("class")!.ToLower(), "[^a-z]", "") == "citiespopular")
            .ToList();

        if (cityListContent != null && cityListContent.Any())
        {
            var hrefs = new List<CityNameLinkPair>(); //Список популярных пунктов и ссылки на погоду в них

            var document = cityListContent[0];

            foreach (var element in document.QuerySelectorAll("a"))
            {
                var href = element.GetAttribute("href");
                hrefs.Add(new CityNameLinkPair(element.TextContent, href!));
            }

            var cities = hrefs.Where(x => !string.IsNullOrEmpty(x.Link)).ToArray();

            Parallel.ForEach(cities, 
                x => Task.Run(async () => await ProcessCity(x))); //5) Многопоточность

            Task.WaitAll();

            while (allProcessesResult.Count < cities.Length) //4) Примитивы синхронизации
                ;

            var forecastRepository = new WeatherForecastRepository(
                                        new WeatherAppSchemeDbContext(dbConnectionString)); //9) Clean architecture

            await forecastRepository.UpdateAsync(completeForecastData, true); //3) Потокобезопасные коллекции
        }
    }
    Thread.Sleep(1000 * 60 * 60 * 24);
}

DateOnly CreateDate(int value)
{
    var result = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, value);
    if (value < DateTime.Now.Day)
        result = result.AddMonths(1);

    return result;
}

int CreateWindValue(string source)
{
    var data = source.Split("-");
    int outerValue;
    var windSpeeds = new List<int>();
    foreach (var value in data)
    {
        if (int.TryParse(value, out outerValue))
            windSpeeds.Add(outerValue);
    }

    if (windSpeeds.Count > 0)
    {
        return (int)windSpeeds.Average();
    }

    return 0;
}

async Task ProcessCity(CityNameLinkPair pair) //6) Асинхронность
{
    var cityWeatherMainPageContent = await parser.GetContent(pair.Link);
    if (!string.IsNullOrEmpty(cityWeatherMainPageContent)) //получена страница с погодой по городу (общая)
    {
        var innerDocument = angleParser.ParseDocument(cityWeatherMainPageContent);

        var hrefContent = innerDocument.QuerySelectorAll("a")
            .Where
            (x => Regex.Replace(x.TextContent, "[^а-яА-Я0-9]", "").ToLower() == "10дней")
            .ToList();

        if (hrefContent != null && hrefContent.Any())
        {
            var relativeLink = hrefContent[0].GetAttribute("href");
            if (!relativeLink!.Contains(pair.Link))
                relativeLink = $"{pair.Link}{relativeLink}";

            var cityWeather10DaysPageContent = await parser.GetContent(relativeLink);
            if (!string.IsNullOrEmpty(cityWeather10DaysPageContent)) //получена страница с погодой за 10 дней по городу
            {
                var inner10DaysWeatherDocument = angleParser.ParseDocument(cityWeather10DaysPageContent);

                var weatherContent = inner10DaysWeatherDocument.QuerySelectorAll("div")
                    .Where
                    (x => x.GetAttribute("data-widget") != null && x.GetAttribute("data-widget")!.Trim().ToLower() == "weather")
                    .ToList();

                if (weatherContent != null && weatherContent.Any()) //если найден виджет с погодой
                {
                    var dates = weatherContent[0].QuerySelectorAll("div")
                        .Where(x => x.GetAttribute("class") == "date")
                        .Select
                        (x => CreateDate
                            (int.Parse
                                (Regex.Replace(x.TextContent, "[^0-9]", ""))
                            )
                        )
                        .ToList();

                    if (dates.Count == 10) //если удается корректно получить 10 дат
                    {
                        var temperatures = weatherContent[0].QuerySelectorAll("span")
                            .Where(x => x.GetAttribute("class") != null
                                && Regex.Replace(x.GetAttribute("class")!.ToLower(), "[^a-z]", "").Contains("temperaturec"))
                            .Select(x => int.Parse(x.TextContent))
                            .ToList();

                        if (temperatures.Count == 20) //если удается корректно получить 10 дневных и 10 ночных температур
                        {
                            var windSpeeds = weatherContent[0].QuerySelectorAll("span")
                                .Where(x => x.GetAttribute("class") != null
                                && Regex.Replace(x.GetAttribute("class")!.ToLower(), "[^a-z]", "").Contains("windms")
                                )
                                .Select(x => CreateWindValue(x.TextContent))
                                .ToList();

                            if (windSpeeds.Count >= 10) //удалось получить 10 значений скорости ветра в метрах в секунду
                            {
                                if(windSpeeds.Count>10)
                                {
                                    windSpeeds.RemoveRange(0, windSpeeds.Count-10);
                                }
                                try
                                {
                                    var cityRepository = new CityRepository(
                                        new WeatherAppSchemeDbContext(dbConnectionString)); //9) Clean architecture

                                    var forecastRepository = new WeatherForecastRepository(
                                        new WeatherAppSchemeDbContext(dbConnectionString)); //9) Clean architecture

                                    var currentCity = await cityRepository.DbSet
                                        .FirstOrDefaultAsync(x => x.Name == pair.Name);

                                    if (currentCity == null)
                                    {
                                        currentCity = new City(
                                            id: 0,
                                            name: pair.Name);

                                        await cityRepository.UpdateAsync(currentCity, true);
                                    }

                                    var cityID = currentCity.Id;

                                    var datesArray = dates.Select(x => new DateTime(x.Year, x.Month, x.Day)).ToArray();
                                    var temperaturesArray = temperatures.ToArray();
                                    var windSpeedsArray = windSpeeds.ToArray();

                                    var currentForecasts = await forecastRepository.DbSet
                                        .Where(x => x.CityId == cityID && datesArray.Contains(x.Date))
                                        .Select(x => x.Date)
                                        .ToListAsync();

                                    if (currentForecasts.Count < 10)
                                    {
                                        for (int index = 0; index < 10; index++)
                                        {
                                            if (!currentForecasts.Contains(datesArray[index]))
                                                completeForecastData.Add(
                                                    new WeatherForecast(
                                                    cityID,
                                                    datesArray[index],
                                                    temperaturesArray[index],
                                                    temperaturesArray[index + 10],
                                                    windSpeedsArray[index]
                                                    ));
                                        }
                                    }
                                    allProcessesResult.Add(true);
                                    return;
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }

            }
        }
    }
    allProcessesResult.Add(false);
}
