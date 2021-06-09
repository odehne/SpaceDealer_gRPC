using System;

namespace Shopping.API.Domain.Events
{


	public class CatalogItemSold
	{
		public Guid Id { get; set; }
		public Guid MarketId { get; set; }
		public Guid BuyerId { get; set; }
		public decimal TotalPrice { get; set; }
		public decimal Amount { get; set; }
	}
}
