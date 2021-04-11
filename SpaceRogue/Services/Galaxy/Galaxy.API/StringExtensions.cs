using System;

namespace Cope.SpaceRogue.Galaxy.API
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
