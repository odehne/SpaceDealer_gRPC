using Cope.SpaceRogue.Maintenance.API.InfraStructure;
using System;
using System.Collections.Generic;

namespace Cope.SpaceRogue.Maintenance.API.Domain
{
	public class Ship : Entity
	{
		public EntityId ShipId { get; init; }
		public EntityId PlayerId { get; init; }

		public int Hull { get; set; }
		public int Shields { get; set; }

		public double LoadCapacity { get; set; }
		public List<Feature> ShipFeatures { get; set; }
		public List<Payload> Cargo { get; set; }

		public Freighter(string shipId, string playerId, Position currentPosition)
		{
			ShipId = shipId;
			PlayerId = playerId;
			CurrentPosition = currentPosition;
			CurrentJourney = new Journey() { Source = CurrentPosition };
			Cargo = new List<Payload>();
			ShipFeatures = new List<Feature>();
		}

		protected override void EnsureValidState()
		{
			throw new NotImplementedException();
		}
	}

}
