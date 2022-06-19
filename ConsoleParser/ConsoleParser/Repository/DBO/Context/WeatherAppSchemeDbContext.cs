using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleParser.Repository.DBO.Context
{
    public class WeatherAppSchemeDbContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<City> Cities { get; set; }
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }

        public WeatherAppSchemeDbContext(
            string connectionString
            )
        {
            _connectionString = connectionString;
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder
            )
        {
            optionsBuilder.UseMySql(
                _connectionString,
                new MySqlServerVersion(new Version(8, 0, 11))
            );
        }
    }
}
