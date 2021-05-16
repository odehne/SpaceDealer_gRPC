using System;

namespace Cope.SpaceRogue.Fighting.API
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
