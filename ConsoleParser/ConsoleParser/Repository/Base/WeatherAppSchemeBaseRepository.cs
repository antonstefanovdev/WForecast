using ConsoleParser.Repository.DBO.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleParser.Repository.Base
{
    public abstract class WeatherAppSchemeBaseRepository<T> : BaseRepository<T> where T : class
    {
        protected WeatherAppSchemeBaseRepository(WeatherAppSchemeDbContext context) : base(context)
        {
        }
    }
}
