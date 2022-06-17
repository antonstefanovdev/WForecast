using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WApp.Models;
using WApp.Repository;

namespace WApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configurationManager;
        private readonly ForecastViewModel _viewModel;
        private readonly HttpClient _client;
        private readonly Uri _baseAddress;

        public HomeController(
            ILogger<HomeController> logger,
            IConfiguration configurationManager,
            ForecastViewModel viewModel
            )
        {
            _logger = logger;
            _configurationManager = configurationManager;

            _baseAddress = new Uri(
                _configurationManager
                .GetConnectionString("DefaultConnection")
                );

            _client = new HttpClient();
            _client.BaseAddress = _baseAddress;

            _viewModel = viewModel;
        }

        private async Task<IEnumerable<City>> GetCitiesAsync()
        {
            try
            {
                HttpResponseMessage response = await _client
                    .GetAsync($"{_client.BaseAddress}/GetCities");

                if (response.IsSuccessStatusCode)
                {
                    var citiesJson = await response.Content.ReadAsStringAsync();
                    var cities = Newtonsoft.Json.JsonConvert
                        .DeserializeObject<List<City>>(citiesJson);
                    if (cities != null && cities.Any()) 
                        return cities;
                }
            }
            catch
            {
                _logger.Log(LogLevel.Error,
                    "Error accessing API or empty result passed",
                    "Method GetCitiesAsync cannot get actual list of cities");
            }
            return new List<City>();
        }

        private async Task<WeatherForecast?> GetForecastAsync(int cityId, DateTime date)
        {
            try
            {
                HttpResponseMessage response = await _client
                    .GetAsync(
                        $"{_client.BaseAddress}/GetForecast/"+
                        $"{cityId}/{date.ToString("dd-MM-yyyy")}"
                        );

                if (response.IsSuccessStatusCode)
                {
                    var forecastsJson = await response.Content.ReadAsStringAsync();
                    var forecasts = Newtonsoft.Json.JsonConvert
                        .DeserializeObject<List<WeatherForecast>>(forecastsJson);

                    if (forecasts != null && forecasts.Any())
                        return forecasts.First();
                }
            }
            catch
            {
                _logger.Log(LogLevel.Error,
                    "Error accessing API or empty result passed",
                    "Method GetForecastAsync cannot get actual forecast");
            }
            return null;
        }

        public async Task<IActionResult> Index(string? cityName, DateTime? date)
        {
            _viewModel.Forecast = null;
            _viewModel.Message = null;
            _viewModel.SelectedCity = null;
            _viewModel.SelectedDate = null;

            try
            {
                var cities = await GetCitiesAsync();

                _viewModel.Cities = cities;

                if (cityName != null && date != null)
                {
                    _viewModel.SelectedDate = date.Value.ToString("yyyy-MM-dd");

                    var city = cities.FirstOrDefault(x => x.Name == cityName);
                    if (city != null)
                    {
                        _viewModel.SelectedCity = city.Name;
                        var forecast = await GetForecastAsync(
                            city.Id,
                            date ?? new DateTime());

                        if (forecast == null)
                            _viewModel.Message =
                                $"Cannot get forecast for {cityName}. Please, checkout selected date.";
                        else
                            _viewModel.Forecast = forecast;
                    }
                }
            }
            catch
            {
                _viewModel.Message = "Error access forecast data";
            }

            return View(_viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}