using SpaceDealerModels.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDealerModels.Extensions
{
	public static class ModelExtensions
	{
		public static DbCoordinates ToDCoordinates(this Coordinates str)
		{
			return new DbCoordinates
			{
				X = str.X,
				Y = str.Y,
				Z = str.Z
			};
		}

	}

	public static class DbModelExtensions
	{
		public static Coordinates ToDCoordinates(this DbCoordinates str)
		{
			return new Coordinates
			{
				X = str.X,
				Y = str.Y,
				Z = str.Z
			};
		}

	}
}
