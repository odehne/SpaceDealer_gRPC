using System;

namespace Cope.SpaceRogue.Galaxy.Creator.IntegrationEvents
{

	public class CatalogItemDeleted
	{
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
	}
}
