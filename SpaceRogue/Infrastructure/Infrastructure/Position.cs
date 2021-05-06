using System;

namespace Cope.SpaceRogue.InfraStructure
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

		public static Position GetPositionByString(string positionString)
		{
			if (string.IsNullOrEmpty(positionString))
				throw new ArgumentException("String must have a value in the format 'x,y,z'");
			positionString = positionString.Replace(" ", "");
			positionString = positionString.Trim(' ');
			var s = positionString.Split(',');
			if (s.Length < 2)
				throw new ArgumentException("String must have a value in the format 'x,y,z'");
			return new Position(double.Parse(s[0]), double.Parse(s[1]), double.Parse(s[2]));
		}

		public override string ToString()
		{
			return $"[{X},{Y},{Z}]";
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
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

		public static Position Move(Position currentPosition, Position destination, double speed = 1)
		{
			return new Position(
				CalculateNewPosition(currentPosition.X, destination.X, speed),
				CalculateNewPosition(currentPosition.Y, destination.Y, speed),
				CalculateNewPosition(currentPosition.Z, destination.Z, speed)
				);
		}

		private static double CalculateNewPosition(double currentValue, double destinationValue, double speed = 1)
		{
			if (currentValue == destinationValue)
				return currentValue;
			if (currentValue < destinationValue)
			{
				return currentValue += speed;
			}
			else
			{
				return currentValue -= speed;
			}
		}


		public override bool Equals(object obj)
		{
			var target = (Position)obj;
			return (target.X == X & target.Y == Y & target.Z == Z);
		}

		public bool InSensorRange(Position target, int offset)
		{
			return (target.X >= target.X - offset | target.X <= target.X + offset |
					target.Y >= target.Y - offset | target.Y <= target.Y + offset |
					target.Z >= target.Z - offset | target.Z <= target.Z + offset );
		}
	}
}