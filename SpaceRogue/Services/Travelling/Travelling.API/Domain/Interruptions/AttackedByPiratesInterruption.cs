namespace Cope.SpaceRogue.Travelling.API.Domain
{
	public class AttackedByPiratesInterruption : InterruptionBase
	{
		public AttackedByPiratesInterruption(string message)
		{
			Type = InterruptionTypes.AttackedByPirates;
			Message = message;
		}
	}
}