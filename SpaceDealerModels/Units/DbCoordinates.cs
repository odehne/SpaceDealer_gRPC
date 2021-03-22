using Newtonsoft.Json;
using System;

namespace SpaceDealerModels.Units
{
	public class DbCoordinates 
	{
		[JsonProperty("x")]
		public double X { get; set; }
		[JsonProperty("y")]
		public double Y { get; set; }
		[JsonProperty("z")]
		public double Z { get; set; }

		public DbCoordinates()
		{
		}

		public DbCoordinates(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public override bool Equals(object obj)
		{
			var target = (DbCoordinates)obj;
			return (target.X == X & target.Y == Y & target.Z == Z);
		}

		public static DbCoordinates GerRandomCoordniates()
		{
			return new DbCoordinates(GetRandomNumber(0, 100), GetRandomNumber(0, 100), GetRandomNumber(0, 100));
		}


		public static DbCoordinates GetDistanceVector(DbCoordinates source, DbCoordinates destination)
		{
			return new DbCoordinates(destination.X - source.X, destination.Y - source.Y, destination.Z - source.Z);
		}

		public static double GetDistanceLength(DbCoordinates source, DbCoordinates destination)
		{
			var v = GetDistanceVector(source, destination);
			if (v.X < 0) v.X *= -1;
			if (v.Y < 0) v.Y *= -1;
			if (v.Z < 0) v.Z *= -1;
			return Math.Sqrt(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2) + Math.Pow(v.Z, 2));
		}

		private static DbCoordinates Clone(DbCoordinates origin)
		{
			return new DbCoordinates(origin.X, origin.Y, origin.Z);
		}

		private static double CalculateNewPosition(double currentValue, double destinationValue, double step = 1)
		{
			if (currentValue == destinationValue) 
				return currentValue;
			if (currentValue < destinationValue)
			{
				return currentValue += step;
			}
			else
			{
				return currentValue -= step;
			}
		}

		public static DbCoordinates Move(DbCoordinates currentPosition, DbCoordinates destination)
		{
			return new DbCoordinates(
				CalculateNewPosition(currentPosition.X, destination.X),
				CalculateNewPosition(currentPosition.Y, destination.Y),
				CalculateNewPosition(currentPosition.Z, destination.Z)
				);
		}

		public override string ToString()
		{
			return $"[{X},{Y},{Z}]";
		}

		public static int GetRandomNumber(int lowerBound, int upperBound)
		{
			Random random = new Random();
			return random.Next(lowerBound, upperBound + 1);
		}
	}
}
