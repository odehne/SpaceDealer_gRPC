using SpaceDealer.Enums;

namespace SpaceDealerModels.Units
{
	public class Result
	{
		public ResultState State { get; set; }
		public string Message { get; set; }

		public Result(ResultState state, string message)
		{
			State = state;
			Message = message;
		}
	}
}