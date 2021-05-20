using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Fighting.API.Application.IntegrationEvents.Events
{

	public class ShipSuccessfullyEscapedIntegrationEvent : IntegrationEvent
	{
		public string ShipId { get; set; }
		public int CurrentPosX { get; set; }
		public int CurrentPosY { get; set; }
		public int CurrentPosZ { get; set; }

		public ShipSuccessfullyEscapedIntegrationEvent(string shipId, int currentPosX, int currentPosY, int currentPosZ)
		{
			ShipId = shipId;
			CurrentPosX = currentPosX;
			CurrentPosY = currentPosY;
			CurrentPosZ = currentPosZ;
		}
	}
}