namespace Cope.SpaceRogue.Travelling.API.Domain
{
	public class AnotherShipWantsToTradeInterruption : InterruptionBase
	{
		public AnotherShipWantsToTradeInterruption(string message)
		{
			Type = InterruptionTypes.AnotherShipWantsToTrade;
			Message = message;
		}
	}
}