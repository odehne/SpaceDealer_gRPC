using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Cope.SpaceRogue.Galaxy.Creator.Domain.SeedWork;
using Cope.SpaceRogue.InfraStructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cope.SpaceRogue.Galaxy.API.Model
{
	public class Planet : Entity
	{
		[Key]
		public Guid ID { get; set; }

		public virtual MarketPlace Market { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }

		public int PosX { get; set; }
		public int PosY { get; set; }
		public int PosZ { get; set; }
		//public PlanetStates PlanetState { get; private set; }

		public enum PlanetStates
		{
			Created,
			Spawned,
			MarketOpen,
		}

		public Planet()
		{
		}

		public Planet(MarketPlace market, string name, string description, int posX, int posY, int posZ)
		{
			ID = Guid.NewGuid();
			if (market == null)
				throw new ArgumentException("Market cannot be null");
			if(string.IsNullOrEmpty(name))
				throw new ArgumentException("The planet's name cannot be empty");
			Market = market;
			Name = name;
			Description = description;
			PosX = posX;
			PosY = posY; 
			PosZ = posZ;
			//PlanetState = PlanetStates.Created;
		}

		public void ChangeState(PlanetStates newState)
		{
			//PlanetState = newState;
		}

		protected override void EnsureValidState()
		{
			var valid = ID != default &&
				Market != null &&
				!string.IsNullOrEmpty(Name) &&
				PosX >= 0 &&
				PosY > 0 &&
				PosZ >= 0;

			if (!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}
	}
}
