using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ship.API.Application.IntegrationEvents.Events
{

	public class FightStartedIntegrationEvent : IntegrationEvent
    {
        public Guid RequestId { get; set; }
        public Guid AttackerId { get; set; }
        public Guid DefenderId { get; set; }

        public FightStartedIntegrationEvent(Guid requestId, Guid attackerId, Guid defenderId)
		{
            RequestId = requestId;
			AttackerId = attackerId;
            DefenderId = defenderId;
		}
	}

}
