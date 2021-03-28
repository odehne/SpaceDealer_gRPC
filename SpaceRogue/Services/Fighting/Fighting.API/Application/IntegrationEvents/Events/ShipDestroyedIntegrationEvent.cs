﻿using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Ship.API.Application.IntegrationEvents.Events
{
	public class ShipDestroyedIntegrationEvent : IntegrationEvent
	{
		public string ShipId { get; set; }

		public ShipDestroyedIntegrationEvent(string shipId) => ShipId = shipId;
	}

}
