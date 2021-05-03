using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Cope.SpaceRogue.Travelling.API.Application.Commands
{

	public class StartJourneyCommand : IRequest<bool>
	{
		[DataMember]
		public string ShipId { get; set; }

		[DataMember]
		public int TargetX { get; set; }

		[DataMember]
		public int TargetY { get; set; }

		[DataMember]
		public int TargetZ { get; set; }

		public StartJourneyCommand()
		{

		}

		public StartJourneyCommand(string shipId, int targetX, int targetY, int targetZ)
		{
			ShipId = shipId;
			TargetX = targetX;
			TargetY = targetY;
			TargetZ = targetZ;
		}
	}
}
