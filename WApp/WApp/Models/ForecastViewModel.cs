using WApp.Repository;

namespace WApp.Models
{
    public class ForecastViewModel
    {
        public WeatherForecast? Forecast { get; set; }
        public IEnumerable<City>? Cities { get; set; }
        public string? SelectedCity { get; set; }
        public string? SelectedDate { get; set; }
        public string? Message { get; set; }
    }
}
