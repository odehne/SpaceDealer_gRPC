using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	public class AddCatalogItemCommand : IRequest<CatalogItemDto>
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
}
