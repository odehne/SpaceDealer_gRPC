using System;

namespace Shopping.API.Domain.Events
{
	public static partial class Events
	{
		public class CatalogItemAdded
		{
			public Guid Id { get; set; }
			public Guid ProductId { get; set; }
			public decimal Price { get; set; }

		}
	}
	
}
