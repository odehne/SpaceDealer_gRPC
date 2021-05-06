using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Travelling.API.Application.IntegrationEvents
{
	public class JourneyStartedIntegrationEvent : IntegrationEvent
	{
		public string ShipId { get; set; }
		public int TargetPosX { get; set; }
		public int TargetPosY { get; set; }
		public int TargetPosZ { get; set; }
	}

	//public class JourneyStartedIntegrationEventHandler : IIntegrationEventHandler<JourneyStartedIntegrationEvent>
	//{
	//	private readonly IEventBus _eventBus;
	//	private readonly ILogger<JourneyStartedIntegrationEventHandler> _logger;

	//	public JourneyStartedIntegrationEventHandler(IEventBus eventBus, ILogger<JourneyStartedIntegrationEventHandler> logger)
	//	{
	//		_eventBus = eventBus;
	//		_logger = logger;
	//	}

	//	public async Task Handle(JourneyStartedIntegrationEvent @event)
	//	{
	//		_logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
	//		_eventBus.Publish(@event);
	//		await Task.CompletedTask;
	//	}
	//}
	
}
