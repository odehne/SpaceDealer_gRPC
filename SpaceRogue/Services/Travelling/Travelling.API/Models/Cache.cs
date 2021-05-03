using Cope.SpaceRogue.InfraStructure;
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
	public class Cache
	{
		public IEnumerable<PlanetModel> Planets { get; set; }
		public List<ShipModel> Ships { get; set; }

		private IMediator _mediator { get; set; }

		public Cache(IMediator mediator)
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

		public IEnumerable<ShipModel> GetShipsInRange(Position sector, int sensorRange)
		{
			return Ships.Where(x => x.Sector.InSensorRange(sector, sensorRange)).ToList();
		}

		public IEnumerable<ShipModel> GetShipsInSector(Position sector)
		{
			return Ships.Where(x => x.Sector.Equals(sector)).ToList();
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
			ship.Sector = newPosition;
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