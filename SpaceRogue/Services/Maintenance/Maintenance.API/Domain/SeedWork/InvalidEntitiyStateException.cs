using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Maintenance.API.Domain.SeedWork
{
	public class InvalidEntityStateException : Exception 
	{
		public InvalidEntityStateException(object entity, string message) : base($"Entity {entity.GetType().Name} state chnage rejected, {message}")
		{
			// hello 😂

		}
	}
}
