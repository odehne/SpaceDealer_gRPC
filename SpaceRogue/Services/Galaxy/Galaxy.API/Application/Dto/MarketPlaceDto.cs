using Cope.SpaceRogue.Infrastructure.Domain;
using System;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	public class MarketPlaceDto
	{
		public string ID { get; private set; }
		public string Name { get; set; }

		public CatalogDto Offerings {get; set;} 
		public CatalogDto Demands {get; set;}

		public MarketPlaceDto(string id, string name)
		{
			ID = id;
			Name = name;
		}

		public MarketPlaceDto(string name)
		{
			ID = Guid.NewGuid().ToString();
			Name = name;
		}

        internal static MarketPlaceDto MapToDto(MarketPlace itm)
        {
			var offerings = CatalogDto.MapToDto(itm.ProductOfferings);
        	var demands = CatalogDto.MapToDto(itm.ProductOfferings);
			var market = new MarketPlaceDto(itm.ID.ToString(), itm.Name);

			market.Offerings = offerings;
			market.Demands = demands;
			
            return market;
        }
    }
}
