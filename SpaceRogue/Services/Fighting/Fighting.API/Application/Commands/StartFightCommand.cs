using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ship.API.Application.Commands
{

	public class StartFightCommand : IRequest<bool>
	{
		[DataMember]
		public Guid AttackerShipId { get; set; }
		[DataMember]
		public Guid DefenderShipId { get; set; }

		public StartFightCommand()
		{

		}

		public StartFightCommand(Guid attackerShipId, Guid defenderShipId)
		{
			AttackerShipId = attackerShipId;
			DefenderShipId = defenderShipId;
		}
	}
}
