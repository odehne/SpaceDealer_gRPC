using System;

namespace Cope.SpaceRogue.Galaxy.API.Domain.Events
{
	public class CatalogItemPriceChangedEvent
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
	}
}
