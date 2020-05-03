using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceDealerModels
{
	public static class Tools
	{
		public static double AddPercent(double value, double percent)
		{
			var percentage = (percent * value) / 100;
			return value + percentage;
		}

	}
}
