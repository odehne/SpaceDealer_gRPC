using System;

namespace SpaceDealer.Units
{
	public class Coordinates 
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }

		public Coordinates(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public override bool Equals(object obj)
		{
			var target = (Coordinates)obj;
			return (target.X == X & target.Y == Y & target.Z == Z);
		}

		public static Coordinates GetDistanceVector(Coordinates source, Coordinates destination)
		{
			return new Coordinates(destination.X - source.X, destination.Y - source.Y, destination.Z - source.Z);
		}

		public static double GetDistanceLength(Coordinates source, Coordinates destination)
		{
			var v = GetDistanceVector(source, destination);
			if (v.X < 0) v.X *= -1;
			if (v.Y < 0) v.Y *= -1;
			if (v.Z < 0) v.Z *= -1;
			return Math.Sqrt(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2) + Math.Pow(v.Z, 2));
		}

		private static Coordinates Clone(Coordinates origin)
		{
			return new Coordinates(origin.X, origin.Y, origin.Z);
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

		public static Coordinates Move(Coordinates currentPosition, Coordinates destination)
		{
			return new Coordinates(
				CalculateNewPosition(currentPosition.X, destination.X),
				CalculateNewPosition(currentPosition.Y, destination.Y),
				CalculateNewPosition(currentPosition.Z, destination.Z)
				);
		}

		public override string ToString()
		{
			return $"[{X},{Y},{Z}]";
		}
	}
}
