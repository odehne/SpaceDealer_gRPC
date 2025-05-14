using SpaceDealerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDealerTerminal
{
    public static class OutputHelper
    {
        public static string GetCurrentShipPosition(Ship s)
        {
            if(s.Cruise!=null)
                return s.Cruise.Departure.ToPlanetPosition() + " --> " + s.Cruise.Destination.ToPlanetPosition();
            return s.CurrentPlanet.ToPlanetPosition();
        }
        public static void Print(string message, params object[] args)
        {
            Console.WriteLine(string.Format(message, args));
        }
        public static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
