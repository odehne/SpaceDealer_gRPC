using Cope.SpaceRogue.Infrastructure.Domain;
using System.Collections.Generic;

namespace Cope.SpaceRogue.Shopping.API.Models
{
	public class CatalogModel
	{
		public string ID { get;  set; }
		public ICollection<CatalogItemModel> CatalogItems { get;  set; }

		public CatalogModel()
		{
		}

		public CatalogModel(string catalogId, ICollection<CatalogItemModel> catalogItems)
		{
			ID = catalogId;
			CatalogItems = catalogItems;
		}

        internal static CatalogModel MapToDto(Catalog catalog)
        {
            var dto = new CatalogModel { ID = catalog.ID.ToString() };
			var lst = new List<CatalogItemModel>();

			foreach (var itm in catalog.CatalogItems)
			{
				lst.Add(new CatalogItemModel { ID = itm.ID.ToString(), Price = (double)itm.Price, ProductId = itm.ProductId.ToString(), Title = itm.Title });
			}

			dto.CatalogItems = lst;
			return dto;
        }
    }
}
