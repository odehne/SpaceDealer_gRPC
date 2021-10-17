using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Travelling.API.Domain;
using Cope.SpaceRogue.Travelling.API.Models;
using System;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Travelling.API
{
	public interface IEngine
	{
		GalaxyModel Galaxy { get; set; }
		Journeys Journeys { get; set; }
		Journey AddJourney(Guid shipId, Position sourcePosition, Position currentPosition, Position destinationPosition, DestinationTypes destinationType, int speed = 1);
		Task Init();
		void Play();
	}
}