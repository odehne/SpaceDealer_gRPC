using System;

namespace Cope.SpaceRogue.Galaxy.API.Domain.Events
{
	public class CatalogItemAddedEvent
	{
		public Guid ProductId { get; set; }
		public Guid CatalogItemId { get; set; }
		public string Title { get; set; }
		public decimal Price { get; set; }
	}

}
