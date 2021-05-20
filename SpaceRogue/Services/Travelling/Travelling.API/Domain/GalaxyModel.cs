using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Travelling.Application.Commands;
using Cope.SpaceRogue.Travelling.API.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cope.SpaceRogue.Travelling.Application.Queries;

namespace Cope.SpaceRogue.Travelling.API.Models
{
	public class GalaxyModel
	{
		public List<PlanetModel> Planets { get; set; }
		public List<ShipModel> Ships { get; set; }

		private IMediator _mediator { get; set; }

		public GalaxyModel(IMediator mediator)
		{
			_mediator = mediator;
			Planets = new List<PlanetModel>();
			Ships = new List<ShipModel>();
		}

		public async Task Load()
		{
			Planets = await _mediator.Send(new PlanetsQuery());
			Ships = await _mediator.Send(new ShipsQuery());
		}

		public ShipModel GetShip(Guid shipId)
		{
			return Ships.FirstOrDefault(x => x.ShipId.Equals(shipId));
		}


		public IEnumerable<ShipModel> GetShipsInRange(Position sector, int sensorRange)
		{
			return Ships.Where(x => x.CurrentSector.InSensorRange(sector, sensorRange)).ToList();
		}

		public IEnumerable<ShipModel> GetShipsInSector(Position sector)
		{
			return Ships.Where(x => x.CurrentSector.Equals(sector)).ToList();
		}

		public IEnumerable<PlanetModel> GetPlanetsInRange(Position sector, int sensorRange)
		{
			return Planets.Where(x => x.Sector.InSensorRange(sector, sensorRange)).ToList();
		}

		public IEnumerable<PlanetModel> GetPlanetsInSector(Position sector)
		{
			return Planets.Where(x => x.Sector.Equals(sector)).ToList();
		}

		public void UpdateShipPosition(Guid shipId, Position newPosition)
		{
			var ship = Ships.FirstOrDefault(x => x.ShipId.Equals(shipId));
			ship.CurrentSector = newPosition;
		}

		public void AddPlanet(PlanetModel newPlanet)
		{
			var orgPlanet = Planets.FirstOrDefault(x => x.PlanetId.Equals(newPlanet.PlanetId));
			if (orgPlanet == null)
			{
				Planets.Add(newPlanet);
			}
		}

		public void AddShip(ShipModel newShip)
		{
			var orgShip = Ships.FirstOrDefault(x => x.ShipId.Equals(newShip.ShipId));
			if (orgShip == null)
			{
				Ships.Add(newShip);
			}
		}
	}

}