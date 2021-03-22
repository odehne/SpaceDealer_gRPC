using System;

namespace Cope.SpaceRogue.Galaxy.API.InfraStructure
{
	public record EntityId
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