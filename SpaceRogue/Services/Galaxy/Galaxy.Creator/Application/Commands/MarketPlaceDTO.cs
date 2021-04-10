using Cope.SpaceRogue.Galaxy.Creator.Domain;

namespace Cope.SpaceRogue.Galaxy.Creator.Application.Commands
{
    public class MarketPlaceDTO
	{
		public string MarketPlaceId { get; private set; }
		public string Name { get; set; }

		public CatalogDTO Offerings {get; set;} 
		public CatalogDTO Demands {get; set;} 
		

		public MarketPlaceDTO(string marketPlaceId, string name)
		{
			MarketPlaceId = marketPlaceId;
			Name = name;
		}

        internal static MarketPlaceDTO MapToDto(MarketPlace itm)
        {
			var offerings = CatalogDTO.MapToDto(itm.ProductOfferings);
        	var demands = CatalogDTO.MapToDto(itm.ProductOfferings);
			var market = new MarketPlaceDTO(itm.ID.ToString(), itm.Name);

			market.Offerings = offerings;
			market.Demands = demands;
			
            return market;
        }
    }
}
