using System;

namespace Cope.SpaceRogue.Galaxy.API.Domain.SeedWork
{
	public class InvalidEntityStateException : Exception 
	{
		public InvalidEntityStateException(object entity, string message) : base($"Entity {entity.GetType().Name} state chnage rejected, {message}")
		{
			// hello 😂

		}
	}
}
