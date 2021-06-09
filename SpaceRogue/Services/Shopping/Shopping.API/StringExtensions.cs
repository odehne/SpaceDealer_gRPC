using System;

namespace Cope.SpaceRogue.Shopping.API
{
    public static class StringExtensions
	{
		public static Guid ToGuid(this string input)
		{
			Guid.TryParse(input, out Guid g);
			return g;
		}
	}
   
}
