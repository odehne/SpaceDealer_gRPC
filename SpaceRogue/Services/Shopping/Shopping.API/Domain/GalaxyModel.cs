using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Shopping.Application.Commands;
using Cope.SpaceRogue.Shopping.API.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cope.SpaceRogue.Shopping.Application.Queries;

namespace Cope.SpaceRogue.Shopping.API.Models
{
	public class GalaxyModel
	{
		public List<PlanetModel> Planets { get; set; }
		public List<PlayerModel> Players { get; set; }
		public List<ProductModel> Products { get; set; }

		private IMediator _mediator { get; set; }

		public GalaxyModel(IMediator mediator)
		{
			_mediator = mediator;
			Planets = new List<PlanetModel>();
			Players = new List<PlayerModel>();
			Products = new List<ProductModel>();
		}

		public async Task Load()
		{
			Planets = await _mediator.Send(new PlanetsQuery());
			Players = await _mediator.Send(new PlayersQuery());
			Products = await _mediator.Send(new ProductsQuery());
		}

		public IEnumerable<PlanetModel> GetPlanetsInRange(Position sector, int sensorRange)
		{
			return Planets.Where(x => x.Sector.InSensorRange(sector, sensorRange)).ToList();
		}

		public IEnumerable<PlanetModel> GetPlanetsInSector(Position sector)
		{
			return Planets.Where(x => x.Sector.Equals(sector)).ToList();
		}

		public void AddPlanet(PlanetModel newPlanet)
		{
			var orgPlanet = Planets.FirstOrDefault(x => x.ID.Equals(newPlanet.ID));
			if (orgPlanet == null)
			{
				Planets.Add(newPlanet);
			}
		}

		public ShipModel GetShip(Guid shipId)
		{
			foreach (var p in Players)
			{
				foreach (var s in p.Fleet)
				{
					if (s.ID.Equals(shipId))
						return s;
				}
			}
			return null;
		}
		
		public void AddShip(ShipModel newShip)
		{
			var player = Players.FirstOrDefault(p => p.ID.Equals(newShip.PlayerId));
			if (player != null)
			{
				var orgShip = player.Fleet.FirstOrDefault(x => x.ID.Equals(newShip.ID));
				if (orgShip == null)
				{
					player.Fleet.Add(newShip);
				}
			}
		}

		internal MarketPlaceModel GetMarketPlace(Guid marketPlaceId)
		{
			return Planets.FirstOrDefault(m => m.Market.ID.Equals(marketPlaceId)).Market;
		}

		internal ProductModel GetProduct(MarketPlaceModel market, Guid catalogItemId)
		{
			var ci = market.ProductDemands.CatalogItems.FirstOrDefault(m => m.ID.Equals(catalogItemId));
			if (ci == null)
			{
				ci = market.ProductOfferings.CatalogItems.FirstOrDefault(m => m.ID.Equals(catalogItemId));
			}
			return Products.FirstOrDefault(x => x.ID.Equals(ci.ProductId));
		}


		internal double GetDemandedProductPrice(MarketPlaceModel marketPlace, Guid catalogItemId)
		{
			var p = marketPlace.ProductDemands.CatalogItems.FirstOrDefault(ci => ci.ID.Equals(catalogItemId));
			if (p != null)
				return p.Price;
			return 0.0;
		}

		internal double GetOfferedProductPrice(MarketPlaceModel marketPlace, Guid catalogItemId)
		{
			var p = marketPlace.ProductOfferings.CatalogItems.FirstOrDefault(ci => ci.ID.Equals(catalogItemId));
			if (p == null)
				throw new ArgumentException($"The selected product doesn't get sold on this planet.");

			return p.Price;
		}

		internal PlayerModel GetPlayer(Guid playerId)
		{
			return Players.FirstOrDefault(p => p.ID.Equals(playerId));
		}

		public void UpdatePosition(ShipModel ship)
		{
			var player = Players.FirstOrDefault(p => p.ID.Equals(ship.PlayerId));
			if (player != null)
			{
				var orgShip = player.Fleet.FirstOrDefault(x => x.ID.Equals(ship.ID));
				if (orgShip != null)
				{
					orgShip.CurrentSector = ship.CurrentSector;
				}
			}
		}
	}
}