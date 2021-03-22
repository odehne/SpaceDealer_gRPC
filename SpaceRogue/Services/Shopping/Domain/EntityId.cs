using System;

namespace Cope.SpaceRogue.Traveling.API.Domain
{
	public class EntityId
	{
		public readonly Guid Value;
		public EntityId(Guid value)
		{
			if (value == default)
				throw new ArgumentException(nameof(value), "Planet id cannot be empty");
			Value = value;
		}
	}
}