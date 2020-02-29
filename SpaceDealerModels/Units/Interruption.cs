using SpaceDealer.Enums;

namespace SpaceDealerModels.Units
{
	public class Interruption
	{
		public InterruptionType Type { get; set; }
		public string Message { get; set; }

		public Interruption(InterruptionType type, string message)
		{
			Type = type;
			Message = message; 
		}
	}
}