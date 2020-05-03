using SpaceDealerService;
using System;

namespace SpaceDealerCoreUi
{
	public static class Tools
    {
        public static int GetRandomNumber(int lowerBound, int upperBound)
        {
            Random random = new Random();
            return random.Next(lowerBound, upperBound + 1);
        }

        public static string ToPosition(Coordinates coordinates)
        {
            return $"[{coordinates.X},{coordinates.Y},{coordinates.Z}]";
        }

    }
}
