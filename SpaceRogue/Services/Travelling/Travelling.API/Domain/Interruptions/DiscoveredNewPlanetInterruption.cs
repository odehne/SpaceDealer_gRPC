namespace Cope.SpaceRogue.Travelling.API.Domain
{
	public class DiscoveredNewPlanetInterruption : InterruptionBase
	{
		public DiscoveredNewPlanetInterruption(string message)
		{
			Type = InterruptionTypes.DiscoveredNewPlanet;
			Message = message;
		}
	}
}