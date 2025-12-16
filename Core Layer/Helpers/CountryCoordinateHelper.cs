using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core_Layer.Models;

namespace Core_Layer.Helpers
{
    public class CountryCoordinateHelper
    {
        private static readonly List<CountryCoordinate> _countries = new()
        {
            new() { Country = "Fransa",     Latitude = 48.8566, Longitude = 2.3522 },
            new() { Country = "Türkiye",    Latitude = 39.9208, Longitude = 32.8541 },
            new() { Country = "Almanya",    Latitude = 52.5200, Longitude = 13.4050 },
            new() { Country = "İtalya",     Latitude = 41.9028, Longitude = 12.4964 },
            new() { Country = "İspanya",    Latitude = 40.4168, Longitude = -3.7038 },
            new() { Country = "Hollanda",   Latitude = 52.3676, Longitude = 4.9041 },
            new() { Country = "Polonya",    Latitude = 52.2297, Longitude = 21.0122 },
            new() { Country = "Avusturya",  Latitude = 48.2100, Longitude = 16.3700 },
            new() { Country = "Macaristan", Latitude = 47.4979, Longitude = 19.0402 },
            new() { Country = "Belçika",    Latitude = 50.8503, Longitude = 4.3517 },
            new() { Country = "Slovakya",   Latitude = 48.1486, Longitude = 17.1077 },
            new() { Country = "Bulgaristan",Latitude = 42.6977, Longitude = 23.3219 }
        };

        public static double? GetLatitude(string country)
        {
            return _countries
                .FirstOrDefault(x => x.Country == country)
                ?.Latitude;
        }

        public static double? GetLongitude(string country)
        {
            return _countries
                .FirstOrDefault(x => x.Country == country)
                ?.Longitude;
        }
    }
}
