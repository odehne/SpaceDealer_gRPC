﻿using System;

namespace Cope.SpaceRogue.Infrastructure.Domain.SeedWork
{
	public class InvalidEntityStateException : Exception 
	{
		public InvalidEntityStateException(object entity, string message) : base($"Entity {entity.GetType().Name} state chnage rejected, {message}")
		{
			// hello 😂

		}
	}
}
