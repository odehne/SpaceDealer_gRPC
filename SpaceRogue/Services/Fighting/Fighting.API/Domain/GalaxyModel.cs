using Cope.SpaceRogue.Fighting.Application.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Fighting.API.Models
{
	public class GalaxyModel
	{
		public List<ShipModel> Ships { get; set; }
		public List<FightModel> Fights { get; set; }

		private IMediator _mediator { get; set; }

		public GalaxyModel(IMediator mediator)
		{
			_mediator = mediator;
			Ships = new List<ShipModel>();
			Fights = new List<FightModel>();
		}

		public FightModel AddFight(ShipModel attacker, ShipModel defender)
		{
			var fight = new FightModel(0, attacker, defender);
			Fights.Add(fight);
			return fight;
		}

		public FightModel GetFight(Guid fightId)
		{
			return Fights.FirstOrDefault(x => x.ID.Equals(fightId));
		}

		public bool RemoveFight(Guid fightId)
		{
			var fgt = Fights.FirstOrDefault(x => x.ID.Equals(fightId));
			if(fgt!=null)
				return Fights.Remove(fgt);
			return false;
		}

		public async Task Load()
		{
			Ships = await _mediator.Send(new ShipsQuery());
			Fights = await _mediator.Send(new FightsQuery());
		}

		public ShipModel GetShip(Guid shipId)
		{
			return Ships.FirstOrDefault(s => s.ShipId.Equals(shipId));
		}
		
		public void AddShip(ShipModel newShip)
		{
			var ship = Ships.FirstOrDefault(s => s.ShipId.Equals(newShip.ShipId));
			if (ship != null)
			{
				//adds the ship
				Ships.Add(newShip);
			}
			else
			{
				//updates the current position only
				ship.CurrentSector = newShip.CurrentSector;
			}
		}

	}
}