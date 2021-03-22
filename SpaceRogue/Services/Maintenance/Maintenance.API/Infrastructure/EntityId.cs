using System;

namespace Cope.SpaceRogue.Maintenance.API.InfraStructure
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