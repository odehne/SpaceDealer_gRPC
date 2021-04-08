﻿using MediatR;
using System.Runtime.Serialization;

namespace Cope.SpaceRogue.Galaxy.Creator.Application.Commands
{
	public class AddPlanetCommand : IRequest<PlanetDTO>
	{
		[DataMember]
		public string PlanetId { get; private set; }
		[DataMember]
		public string PlanetName { get; private set; }
		[DataMember]
		public int PosX { get; private set; }
		[DataMember]
		public int PosY { get; private set; }
		[DataMember]
		public int PosZ { get; private set; }
		[DataMember]
		public MarketPlaceDTO MarketPlace { get; private set; }
		
	}

	public class PlanetDTO
	{
		public string PlanetId { get; private set; }
		public string PlanetName { get; private set; }
		public int PosX { get; private set; }
		public int PosY { get; private set; }
		public int PosZ { get; private set; }
		public MarketPlaceDTO MarketPlace { get; private set; }

	}

	public class AddMarketPlaceCommand : IRequest<MarketPlaceDTO>
	{
		[DataMember]
		public string MarketPlaceId { get; set; }
		[DataMember]
		public string MarketPlaceName { get; set; }
		[DataMember]
		public string PlanetId { get; set; }
		[DataMember]
		public CatalogDTO Offerings { get; set; }
		[DataMember]
		public CatalogDTO Demands { get; set; }

		public AddMarketPlaceCommand()
		{
		}

		public AddMarketPlaceCommand(string marketPlaceId, string marketPlaceName, string planetId, CatalogDTO offerings, CatalogDTO demands)
		{
			MarketPlaceId = marketPlaceId;
			MarketPlaceName = marketPlaceName;
			PlanetId = planetId;
			Offerings = offerings;
			Demands = demands;
		}
	}
}