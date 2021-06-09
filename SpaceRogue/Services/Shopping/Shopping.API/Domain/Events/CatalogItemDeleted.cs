using System;

namespace Shopping.API.Domain.Events
{
	public class CatalogItemDeleted
	{
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
	}
}
