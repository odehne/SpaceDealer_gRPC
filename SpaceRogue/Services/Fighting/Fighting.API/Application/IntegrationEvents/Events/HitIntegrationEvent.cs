﻿using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Ship.API.Application.IntegrationEvents.Events
{
	public class HitIntegrationEvent : IntegrationEvent
	{
		public string ShipId { get; set; }

		public HitIntegrationEvent(string shipId) => ShipId = shipId;
	}

}