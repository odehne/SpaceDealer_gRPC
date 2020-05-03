using SpaceDealerService;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SpaceDealerCoreUi
{
    public static class MyExtensions
    {
        public static string ToDecimalString(this double value)
        {
            return value.ToString("0.##", CultureInfo.InvariantCulture);
        }

        public static string ToPlanetPosition(this Planet planet)
        {
            return $"{planet.PlanetName} [{planet.Sector.X},{planet.Sector.Y},{planet.Sector.Z}]";
        }

        public static string Tabyfy(this string str)
        {
            if (str.Length < 8)
            {
                return str + "\t\t";
            }
            return str + "\t";
        }
    }
}
