namespace Cope.SpaceRogue.Travelling.API.Domain
{
	public class DistresscallInterruption : InterruptionBase
	{
		public DistresscallInterruption(string message)
		{
			Type = InterruptionTypes.DistressCall;
			Message = message;
		}
	}
}