using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Travelling.API.Application.IntegrationEvents.Events
{
	public class  JourneyStartedIntegrationEvent : IntegrationEvent
    {
        public string ShipId { get; set; }

        public JourneyStartedIntegrationEvent(string shipId)
            => ShipId = shipId;
    }
}
