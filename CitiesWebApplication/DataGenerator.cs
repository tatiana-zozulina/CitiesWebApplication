using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CitiesWebApplication.Models;
using System.Threading.Tasks;

namespace CitiesWebApplication
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApiContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApiContext>>()))
            {
                if (context.Cities.Any())
                {
                    return;
                }

                context.Cities.AddRange(
                    new City
                    {
                        Id = 1,
                        Name = "Moscow",
                        Population = 11920000,
                        Date = new DateTime(year: 1174, month: 1, day: 1)
                    },
                    new City
                    {
                        Id = 2,
                        Name = "Saint Petersburg",
                        Population = 5351935,
                        Date = new DateTime(year: 1703, month: 7, day: 27)
                    },
                    new City
                    {
                        Id = 3,
                        Name = "Yekaterinburg",
                        Population = 14950556,
                        Date = new DateTime(year: 1723, month: 11, day: 18)
                    },
                    new City
                    {
                        Id = 4,
                        Name = "Novosibirsk",
                        Population = 1612833,
                        Date = new DateTime(year: 1893, month: 1, day: 1)
                    });

                context.SaveChanges();
            }
        }

    }
}
