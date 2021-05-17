using Cope.SpaceRogue.Fighting.API.Models;
using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Infrastructure.Domain;
using Infrastructure.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Fighting.API.Repositories
{

	public interface IFightRepository
	{
		GalaxyDbContext Context { get; }
		Task<Fight> GetItem(Guid id);
		Task<List<Fight>> GetItems();
		Task<bool> AddItem(Fight item);
		Task<bool> DeleteItem(Fight item);
		Task<Fight> UpdateItem(Fight item);

		Task<FightStates> Flee(Fight fight, ShipModel defender);
		Task<FightStates> Battle(Fight fight, ShipModel attacker, ShipModel defender);
	}

	public class FightRepository : IFightRepository
	{

		public GalaxyDbContext Context { get; }

		public FightRepository(GalaxyDbContext context)
		{
			Context = context;
		}


		public async Task<Fight> GetItem(Guid id)
		{
			return await Context.Fights.FirstOrDefaultAsync(x => x.ID.Equals(id));
		}

		public async Task<List<Fight>> GetItems()
		{
			return await Context.Fights.ToListAsync();
		}

		public async Task<bool> AddItem(Fight item)
		{
			if (item.ID == default)
				throw new ArgumentException("Fight must have an Id.");

			if (item.Attacker==null)
				throw new ArgumentException("Attacker must be defined.");

			if (item.Defender == null)
				throw new ArgumentException("Defender must be defined.");

			var pn = await GetItem(item.ID);
			if (pn == null)
			{
				Context.Fights.Add(item);
				await Context.SaveChangesAsync();
			}
			return true;
		}

		public async Task<bool> DeleteItem(Fight item)
		{
			var itm = await Context.Fights.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				Context.Fights.Remove(itm);
				await Context.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<Fight> UpdateItem(Fight item)
		{
			var itm = await Context.Fights.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				itm.Attacker = item.Attacker;
				itm.Defender = item.Defender;
				itm.State = item.State;
				await Context.SaveChangesAsync();
			}
			return item;
		}

		public async Task<FightStates> Flee(Fight fight, ShipModel defender)
		{
			fight = SwapDefender(fight, defender.ShipId);
			int fleeRoll = SimpleDiceRoller.Roll(DiceType.d12, defender.SpeedValue);
			if (fleeRoll > 6)
			{
				fight.State = FightStates.TargetFled;
			}
			fight.State = FightStates.Missed;
			await UpdateItem(fight);
			return fight.State;
		}

		public async Task<FightStates> Battle(Fight fight, ShipModel attacker, ShipModel defender)
		{
			fight = SwapAttacker(fight, attacker.ShipId);
			
			int attackValue = 1;
			int criticalHitBonus = 0;
			int attackRoll = SimpleDiceRoller.Roll(DiceType.d20, attacker.AttackValue);
			int defenceRoll = SimpleDiceRoller.Roll(DiceType.d20, defender.DefenceValue);

			if (attackRoll <= defenceRoll)
			{
				return FightStates.Missed;
			}
			else
			{
				if (attackRoll == 1)
				{
					criticalHitBonus = -1;
				}

				if (attackRoll == 20)
				{
					criticalHitBonus = 1;
				}

				if (defender.ShieldsValue > 0)
				{
					defender.ShieldsValue = defender.ShieldsValue - attackValue - criticalHitBonus;
					defender.State = defender.ShieldsValue == 0 ? ShipModel.ShipStates.ShieldsDestroyed : ShipModel.ShipStates.ShieldsDamaged;
				}
				else
				{
					if (defender.HullValue > 0)
					{
						defender.HullValue = defender.HullValue - attackValue - criticalHitBonus;
						defender.State = defender.HullValue < 0 ? ShipModel.ShipStates.Destroyed : ShipModel.ShipStates.HullDamaged;
					}
					else
					{
						defender.State = ShipModel.ShipStates.Destroyed;
					}
				}

				if (defender.State == ShipModel.ShipStates.Destroyed)
					fight.State = FightStates.ShipDestroyed;

				fight.State = FightStates.Hit;

				await UpdateItem(fight);
				return fight.State;
			}
		}


		private Fight SwapDefender(Fight fight, Guid newDefenderId)
		{
			if (!fight.Defender.ID.Equals(newDefenderId))
			{
				var temp = fight.Attacker;
				fight.Attacker = fight.Defender;
				fight.Defender = temp;
			}
			return fight;
		}
		private Fight SwapAttacker(Fight fight, Guid newAttackerId)
		{
			if (!fight.Attacker.ID.Equals(newAttackerId))
			{
				var temp = fight.Attacker;
				fight.Attacker = fight.Defender;
				fight.Defender = temp;
			}
			return fight;
		}

	}
}
