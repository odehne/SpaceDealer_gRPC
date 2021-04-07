using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cope.SpaceRogue.Galaxy.Creator.Application.Commands
{
	public class AddCatalogCommand : IRequest<CatalogDTO>
	{
		[DataMember]
		public string CatalogId { get; private set; }
		[DataMember]
		public string MarketPlaceId { get; private set; }
		[DataMember]
		public List<CatalogItemDTO> CatalogItems { get; private set; }

		public AddCatalogCommand(string catalogId, string marketPlaceId, List<CatalogItemDTO> catalogItems)
		{
			CatalogId = catalogId;
			MarketPlaceId = marketPlaceId;
			CatalogItems = catalogItems;
		}
	}


	public class CatalogDTO
	{
		public string CatalogId { get;  set; }
		public string MarketPlaceId { get;  set; }
		public IEnumerable<CatalogItemDTO> CatalogItems { get;  set; }

		public CatalogDTO()
		{
		}

		public CatalogDTO(string catalogId, string marketPlaceId, IEnumerable<CatalogItemDTO> catalogItems)
		{
			CatalogId = catalogId;
			MarketPlaceId = marketPlaceId;
			CatalogItems = catalogItems;
		}
	}
}
