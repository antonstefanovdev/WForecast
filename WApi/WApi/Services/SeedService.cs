using Microsoft.EntityFrameworkCore;
using WApi.Repository;
using WApi.Services.Static;

namespace WApi.Services
{
    public class SeedService
    {
        private readonly ILogger<SeedService> _logger;
        private readonly ApplicationContext _context;
        private readonly Random _random;

        public SeedService(
            ILogger<SeedService> logger,
            ApplicationContext context,
            Random random)
        {
            _random = random;
            _logger = logger;
            _context = context;
        }

        public async Task InitDatabaseAsync()
        {
            try
            {
                if (!await _context.Cities.AnyAsync())
                {
                    var cities = new List<City>();

                    cities.Add(new City(23, MainCities.city23));
                    cities.Add(new City(78, MainCities.city78));
                    cities.Add(new City(116, MainCities.city116));

                    await _context.AddRangeAsync(cities);
                    await _context.SaveChangesAsync();


                    _logger.Log(LogLevel.Warning,
                        "Database had no cities, seed method used to initialize City table",
                        "InitDatabaseAsync added 3 records into database");
                }
            }
            catch
            {
                _logger.Log(LogLevel.Error,
                    "Cannot access City table in database",
                    "InitDatabaseAsync cannot add initial list of cities because of database error");
            }

            try
            {
                if (!await _context.WeatherForecasts.AnyAsync())
                {
                    var cities = await _context.Cities.ToListAsync();

                    if (cities.Any())
                    {
                        var forecasts = new List<WeatherForecast>();
                        foreach (var city in cities)
                        {
                            var date = DateTime.Today;
                            for (int days = 1; days <= 10; days++)
                                forecasts.Add(new WeatherForecast(
                                    city.Id,
                                    date.AddDays(days),
                                    _random.Next(54) - 25,
                                    _random.Next(43) - 18,
                                    _random.Next(18)
                                    ));
                        }

                        await _context.AddRangeAsync(forecasts);
                        await _context.SaveChangesAsync();

                        _logger.Log(LogLevel.Warning,
                        "Database had no forecasts, seed method used to initialize WeatherForecast table",
                        $"InitDatabaseAsync added {forecasts.Count} records into database");
                    }                   
                }
            }
            catch
            {
                _logger.Log(LogLevel.Error,
                    "Cannot access WeatherForecast table in database",
                    "InitDatabaseAsync cannot add initial list of forecasts because of database error");
            }
        }
    }
}
