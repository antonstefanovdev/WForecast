using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WApi.Repository
{
    public class ApplicationContext : DbContext
    {
        private readonly IConfiguration _configurationManager;

        public DbSet<City> Cities { get; set; }
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }

        public ApplicationContext(
            IConfiguration configurationManager
            )
        {
            _configurationManager = configurationManager;
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder
            )
        {
            var connectionString = _configurationManager
                .GetConnectionString("DefaultConnection");

            optionsBuilder.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 11))
            );
        }
    }
}
