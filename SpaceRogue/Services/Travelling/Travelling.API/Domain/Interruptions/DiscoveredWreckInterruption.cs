namespace Cope.SpaceRogue.Travelling.API.Domain
{
	public class DiscoveredWreckInterruption : InterruptionBase
	{
		public DiscoveredWreckInterruption(string message)
		{
			Type = InterruptionTypes.DiscoveredWreck;
			Message = message;
		}
	}
}