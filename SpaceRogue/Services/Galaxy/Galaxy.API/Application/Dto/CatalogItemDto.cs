using Cope.SpaceRogue.Galaxy.API.Domain;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	public class CatalogItemDto
	{
		public string ID { get; set; }
		public string ProductId { get; set; }
		public string Title { get; set; }
		public double Price { get; set; }

		public CatalogItemDto()
		{
		}

		public CatalogItemDto(string catalogItemId, string productId, string title, double price)
		{
			ID = catalogItemId;
			ProductId = productId;
			Title = title;
			Price = price;
		}

        internal static CatalogItemDto MapToDto(CatalogItem itm)
        {
            return new CatalogItemDto(itm.ID.ToString(), itm.ProductId.ToString(), itm.Title, (double)itm.Price);
        }
    }
}
