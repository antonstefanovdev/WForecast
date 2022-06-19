namespace WApp.Repository
{
[Table(name: "WeatherForecast")]
    public class WeatherForecast
    {
        [Key]
        public int Id { get; set; }
        public int CityId { get; set; }
        public DateTime Date { get; set; }
        public int TemperatureDayC { get; set; }
        public int TemperatureNightC { get; set; }

        public int WindSpeed { get; set; }
        public WeatherForecast(
            int cityId,
            DateTime date,
            int temperatureDayC,
            int temperatureNightC,
            int windSpeed
            )
        {
            CityId = cityId;
            Date = date;
            TemperatureDayC = temperatureDayC;
            TemperatureNightC = temperatureNightC;
            WindSpeed = windSpeed;
        }
    }
}
