using Cope.SpaceRogue.InfraStructure;
using Cope.SpaceRogue.Travelling.API.Models;
using Cope.SpaceRogue.Travelling.API;
using Cope.SpaceRogue.Travelling.API.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
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
            var ships = Program.TravelCache.GetShipsInSector(sector);
            var planets = Program.TravelCache.GetPlanetsInSector(sector);

			return new ObjectsInSectorModel
			{
				Planets = planets,
				Ships = ships,
				Sector = sector
			};
		}
     
	}
}