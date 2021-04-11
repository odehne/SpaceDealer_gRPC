using System;

namespace Cope.SpaceRogue.Galaxy.API.IntegrationEvents
{

	public class CatalogItemDeleted
	{
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
	}
}
