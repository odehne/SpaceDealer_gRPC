using Cope.SpaceRogue.InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cope.SpaceRogue.Traveling.API.Domain
{
	public enum JourneyStates
	{
		Idle,
		JourneyStarted,
		ArrivedAtPosition,
		ArrivedAtPlanet,
		ArrivedAtShipyard,
		BuyingAndSelling,
		Fighting,
		RecievedDistressCall,
		FoundNewPlanet,
		RecievedInformationOffering,
		JourneyFinished
	}

	public class Journeys : Entity
	{
		public List<Journey> Items { get; set; }

		public Journeys()
		{
			Items = new List<Journey>();
		}

		public JourneyStates StartJourney(Journey journey)
		{
			try
			{
				Items.Add(journey);
				return JourneyStates.JourneyStarted;
			}
			catch (Exception e)
			{
				throw;
			}
		}

		public JourneyStates FinishJourney(EntityId journeyId)
		{
			if(journeyId.Value==default)
				throw new ArgumentException("JourneyId is not valid.");

			var journey = Items.FirstOrDefault(x => x.Id.Equals(journeyId));
			if(journey==null)
			{
				throw new ArgumentException("Journey not found.");
			}
			else
			{
				Items.Remove(journey);
				return JourneyStates.JourneyFinished;
			}
		}

		public void Move()
		{
			foreach (var journey in Items)
			{
				journey.Move();
			}
		}

		protected override void EnsureValidState()
		{
			throw new NotImplementedException();
		}
	}
}