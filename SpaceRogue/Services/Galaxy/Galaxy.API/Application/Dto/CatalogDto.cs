using Cope.SpaceRogue.Infrastructure.Domain;
using System.Collections.Generic;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	public class CatalogDto
	{
		public string ID { get;  set; }
		public IEnumerable<CatalogItemDto> CatalogItems { get;  set; }

		public CatalogDto()
		{
		}

		public CatalogDto(string catalogId, IEnumerable<CatalogItemDto> catalogItems)
		{
			ID = catalogId;
			CatalogItems = catalogItems;
		}

        internal static CatalogDto MapToDto(Catalog catalog)
        {
            var dto = new CatalogDto { ID = catalog.ID.ToString() };
			var lst = new List<CatalogItemDto>();

			foreach (var itm in catalog.CatalogItems)
			{
				lst.Add(AutoMap.Mapper.Map<CatalogItemDto>(itm));
			}

			dto.CatalogItems = lst;
			return dto;
        }
    }
}
