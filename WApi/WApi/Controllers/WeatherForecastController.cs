using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WApi.Repository;
using WApi.Services;

namespace WApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ApplicationContext _context;
        private readonly SeedService _seed;

        public WeatherForecastController
            (
            ILogger<WeatherForecastController> logger,
            ApplicationContext context,
            SeedService seed
            )
        {
            _context = context;
            _logger = logger;
            _seed = seed;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            await _seed.InitDatabaseAsync();
            try
            {
                var cities = await _context.Cities.ToListAsync();

                if(!cities.Any())
                {
                    _logger.Log(LogLevel.Warning,
                        "There is no cities in database",
                        "GetCitiesAsync cannot found any city in database");
                }

                return cities;
            }
            catch
            {
                _logger.Log(LogLevel.Error,
                    "Database access error occured",
                    "GetCitiesAsync cannot get actual list of cities from database");

                return new List<City>();
            }
        }
       
        [HttpGet]
        [Route("[action]/{cityId}/{date}")]
        public async Task<IEnumerable<WeatherForecast>> GetForecastAsync
            (
            int cityId, 
            string date
            )
        {
            await _seed.InitDatabaseAsync();

            var result = new List<WeatherForecast>();

            DateOnly forecastDate;
            if (DateOnly.TryParse(date, out forecastDate))
            {
                var forecastDateTime = new DateTime(forecastDate.Year, forecastDate.Month, forecastDate.Day);

                try
                {
                    var forecast = await _context.WeatherForecasts
                        .FirstOrDefaultAsync(x => x.CityId == cityId
                        && x.Date == forecastDateTime);

                    if (forecast != null)
                        result.Add(forecast);
                }
                catch
                {
                    _logger.Log(LogLevel.Error, 
                        "Database access error occured", 
                        "GetForecastAsync cannot get actual forecast data from database");
                }                
            }
            else
            {
                _logger.Log(LogLevel.Error,
                    "Incorrect value for 'date' parameter passed",
                    $"GetForecastAsync cannot convert {date} into correct DateOnly value");
            }

            return result;
        }
    }
}