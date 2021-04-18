using Cope.SpaceRogue.Infrastructure.Model;
using Cope.SpaceRogue.InfraStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cope.SpaceRogue.Infrastructure.Domain
{
	public class Player : Entity
	{
		public enum PlayerTypes
		{
			NPC,
			Human
		}

		[Key]
		public Guid ID { get; private set; }
		public string Name { get; set; }
		[ForeignKey("Planet")]
		public Planet HomePlanet { get; set; }
		public decimal Credits { get; set; }
		public PlayerTypes PlayerType { get; set; }
		public List<Ship> Fleet { get; set; }

		public Player()
		{
		}

		public Player(string name, Planet homePlanet, decimal credits, PlayerTypes playerType)
		{
			ID = Guid.NewGuid();
			Name = name;
			HomePlanet = homePlanet;
			Credits = credits;
			PlayerType = playerType;
			Fleet = new List<Ship>();
		}

		protected override void EnsureValidState()
		{
			throw new NotImplementedException();
		}
	}


}