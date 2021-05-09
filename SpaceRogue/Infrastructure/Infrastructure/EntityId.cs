using System;

namespace Cope.SpaceRogue.Infrastructure
{
	public record EntityId
	{
		public readonly Guid Value;
		public EntityId(Guid value)
		{
			if (value == default)
				throw new ArgumentException(nameof(value), "Entity id cannot be empty");
			Value = value;
		}


	}
}