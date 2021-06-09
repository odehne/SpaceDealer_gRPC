namespace Cope.SpaceRogue.Shopping.API.Models
{
	public class CatalogItemModel
	{
		public string ID { get; set; }
		public string ProductId { get; set; }
		public string Title { get; set; }
		public double Price { get; set; }

		public CatalogItemModel()
		{
		}

		public CatalogItemModel(string catalogItemId, string productId, string title, double price)
		{
			ID = catalogItemId;
			ProductId = productId;
			Title = title;
			Price = price;
		}

    }
}
