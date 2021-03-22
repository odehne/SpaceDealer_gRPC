using Cope.SpaceRogue.InfraStructure;


namespace Cope.SpaceRogue.Traveling.API.Domain
{

	public class Journey : Entity
	{
		public EntityId Id { get; }
		public EntityId ShipId { get; }
		public Position Source { get; set; }
		public Position Destination { get; set; }
		public Position CurrentPosition { get; set; }
		public int Speed { get; set; }

		public Journey(EntityId id, EntityId shipId, Position source, Position destination, Position currentPosition, int speed)
		{
			Id = id;
			ShipId = shipId;
			Source = source;
			Destination = destination;
			CurrentPosition = currentPosition;
			Speed = speed;

			if (currentPosition == destination)
				Speed = 0;
		}

		public JourneyStates Move()
		{

		}

		protected override void EnsureValidState()
		{
			throw new System.NotImplementedException();
		}
	}
}