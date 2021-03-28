using System;

namespace Cope.SpaceRogue.Galaxy.Creator.IntegrationEvents
{ 
	public class CatalogItemAdded
	{
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
		public decimal Price { get; set; }

	}
}
