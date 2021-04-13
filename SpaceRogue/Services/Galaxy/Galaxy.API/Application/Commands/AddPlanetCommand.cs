using MediatR;
using System.Runtime.Serialization;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	public class AddPlanetCommand : IRequest<PlanetDto>
	{
		[DataMember]
		public string PlanetId { get; private set; }
		[DataMember]
		public string PlanetName { get; private set; }
		[DataMember]
		public int PosX { get; private set; }
		[DataMember]
		public int PosY { get; private set; }
		[DataMember]
		public int PosZ { get; private set; }
		[DataMember]
		public MarketPlaceDto MarketPlace { get; private set; }
		
	}
}
