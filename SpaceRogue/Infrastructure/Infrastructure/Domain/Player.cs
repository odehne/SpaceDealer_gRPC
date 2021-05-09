using Cope.SpaceRogue.Infrastructure.Model;
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
			Human=0,
			NPC=1
		}

		[Key]
		public Guid ID { get; set; }
		public string Name { get; set; }
		[ForeignKey("Planet")]
		public Planet HomePlanet { get; set; }
		public decimal Credits { get; set; }
		public PlayerTypes PlayerType { get; set; }
		public List<Ship> Fleet { get; set; }

		public Player()
		{

		}

		public Player(Guid id, string name, Planet homePlanet, decimal credits, PlayerTypes playerType)
		{
			ID = id;
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