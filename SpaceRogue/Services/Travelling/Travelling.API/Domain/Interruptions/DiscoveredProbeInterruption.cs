namespace Cope.SpaceRogue.Travelling.API.Domain
{
	public class DiscoveredProbeInterruption : InterruptionBase
	{
		public DiscoveredProbeInterruption(string message)
		{
			Type = InterruptionTypes.DiscoveredProbe;
			Message = message;
		}
	}
}