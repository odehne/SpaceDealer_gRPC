using MediatR;
using System.Runtime.Serialization;

namespace Cope.SpaceRogue.Galaxy.Creator.Application.Commands
{
	public class AddCatalogItemCommand : IRequest<CatalogItemDTO>
	{
		[DataMember]
		public string CatalogId { get; private set; }
		[DataMember]
		public string ProductId { get; private set; }
		[DataMember]
		public string Title { get; private set; }
		[DataMember]
		public double Price { get; private set; }

		public AddCatalogItemCommand(string catalogId, string productId, string title, double price)
		{
			CatalogId = catalogId;
			ProductId = productId;
			Title = title;
			Price = price;
		}
	}

	public class CatalogItemDTO
	{
		public string CatalogItemId { get; set; }
		public string CatalogId { get; set; }
		public string ProductId { get; set; }
		public string Title { get; set; }
		public double Price { get; set; }

		public CatalogItemDTO()
		{
		}

		public CatalogItemDTO(string catalogItemId, string catalogId, string productId, string title, double price)
		{
			CatalogItemId = catalogItemId;
			CatalogId = catalogId;
			ProductId = productId;
			Title = title;
			Price = price;
		}
	}
}
