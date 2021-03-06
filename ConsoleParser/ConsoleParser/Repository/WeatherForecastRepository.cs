using ConsoleParser.Repository.Base;
using ConsoleParser.Repository.DBO;
using ConsoleParser.Repository.DBO.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleParser.Repository
{
    internal class WeatherForecastRepository : WeatherAppSchemeBaseRepository<WeatherForecast>
    {
        public WeatherForecastRepository(WeatherAppSchemeDbContext context) : base(context)
        {

        }
    }
}
