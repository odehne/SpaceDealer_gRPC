using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System;

namespace Ship.API.Application.IntegrationEvents.Events
{
	public class PlayerStartedJourneyIntegrationEvent : IntegrationEvent
	{
		public Guid RequestId { get; set; }
		public string PlayerId { get; init; }
		public string ShipId { get; init; }
		public int TargetX { get;  }
		public int TargetY { get; }
		public int TargetZ { get; }

		public PlayerStartedJourneyIntegrationEvent(string playerId, string shipId, Guid requestId, int targetX, int targetY, int targetZ)
		{
			RequestId = requestId;
			PlayerId = playerId;
			ShipId = shipId;
			TargetX = targetX;
			TargetY = targetY;
			TargetZ = targetZ;
		}
	}
}
