using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Travelling.API;
using MediatR;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Traveling.API.Controllers
{
	public class ObjectsInSectorQuery : IRequest<ObjectsInSectorModel> 
    {
        [DataMember]
        public int PosX { get; private set; }
        [DataMember]
        public int PosY { get; private set; }
        [DataMember]
        public int PosZ { get; private set; }

        public ObjectsInSectorQuery(int posX, int posY, int posZ)
        {
            PosX = posX;
            PosY = posY;
            PosZ = posZ;
        }
    }

	public class ObjectsInSectorQueryHandler : IRequestHandler<ObjectsInSectorQuery, ObjectsInSectorModel>
	{
      
        public ObjectsInSectorQueryHandler()
        {
        }

        public async Task<ObjectsInSectorModel> Handle(ObjectsInSectorQuery request, CancellationToken cancellationToken)
        {
            var sector = new Position(request.PosX, request.PosY, request.PosZ);
            var ships = Engine.Galaxy.GetShipsInSector(sector);
            var planets = Engine.Galaxy.GetPlanetsInSector(sector);

			return new ObjectsInSectorModel
			{
				Planets = planets,
				Ships = ships,
				Sector = sector
			};
		}
     
	}
}