using System;

namespace Cope.SpaceRogue.Galaxy.API.InfraStructure
{
	public class Position
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }

		public Position(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public override string ToString()
		{
			return $"[{X},{Y},{Z}]";
		}

		public static Position GetDistanceVector(Position source, Position destination)
		{
			return new Position(destination.X - source.X, destination.Y - source.Y, destination.Z - source.Z);
		}

		public static double GetDistanceLength(Position source, Position destination)
		{
			var v = GetDistanceVector(source, destination);
			if (v.X < 0) v.X *= -1;
			if (v.Y < 0) v.Y *= -1;
			if (v.Z < 0) v.Z *= -1;
			return Math.Sqrt(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2) + Math.Pow(v.Z, 2));
		}

		public static Position Move(Position currentPosition, Position destination)
		{
			return new Position(
				CalculateNewPosition(currentPosition.X, destination.X),
				CalculateNewPosition(currentPosition.Y, destination.Y),
				CalculateNewPosition(currentPosition.Z, destination.Z)
				);
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


		public override bool Equals(object obj)
		{
			var target = (Position)obj;
			return (target.X == X & target.Y == Y & target.Z == Z);
		}
	}
}