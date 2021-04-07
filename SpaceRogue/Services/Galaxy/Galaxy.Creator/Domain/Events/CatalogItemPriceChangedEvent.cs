using System;

namespace Cope.SpaceRogue.Galaxy.Creator.Domain.Events
{
	public class CatalogItemPriceChangedEvent
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
	}
}
