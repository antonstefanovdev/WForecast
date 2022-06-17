using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WApi.Repository
{
    [Table(name: "WeatherForecast")]
    public class WeatherForecast
    {
        [Key]
        public int Id { get; set; }
        public int CityId { get; set; }
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }

        public string Summary { get; set; } = string.Empty;
        public WeatherForecast(
            int cityId,
            DateTime date,
            int temperatureC,
            string summary
            )
        {
            CityId = cityId;
            Date = date;
            TemperatureC = temperatureC;
            Summary = summary;
        }
    }
}