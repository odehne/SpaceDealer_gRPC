using Cope.SpaceRogue.Galaxy.Creator.Domain;
using MediatR;
using System;
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
		public IEnumerable<CatalogItemDTO> CatalogItems { get;  set; }

		public CatalogDTO()
		{
		}

		public CatalogDTO(string catalogId, IEnumerable<CatalogItemDTO> catalogItems)
		{
			CatalogId = catalogId;
			CatalogItems = catalogItems;
		}

        internal static CatalogDTO MapToDto(Catalog catalog)
        {
            var dto = new CatalogDTO { CatalogId = catalog.ID.ToString() };
			var lst = new List<CatalogItemDTO>();

			foreach (var item in catalog.CatalogItems)
			{
				lst.Add(CatalogItemDTO.MapToDto(item));
			}

			dto.CatalogItems = lst;
			return dto;
        }
    }
}
