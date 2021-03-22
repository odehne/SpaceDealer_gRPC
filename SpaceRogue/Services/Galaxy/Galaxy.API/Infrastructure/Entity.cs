using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.InfraStructure
{
	public abstract class Entity
	{
		private readonly List<object> _events;
		protected Entity() => _events = new List<object>();

		protected void Raise(object @event) => _events.Add(@event);

		public IEnumerable<object> GetChanges() => _events.AsEnumerable();

		public void ClearChanges() => _events.Clear();

		protected abstract void EnsureValidState();

	}
}
