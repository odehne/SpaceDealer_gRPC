using Cope.SpaceRogue.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Cope.SpaceRogue.Travelling.API.Domain
{
	public class Journeys : ConcurrentDictionary<Guid, Journey>
	{
		public event ArrivedAtDestination Arrived;
		public delegate void ArrivedAtDestination(Guid journeyId, string message, Position newPosition, Guid shipId);
		public event JourneyInterrupted Interrupted;
		public delegate void JourneyInterrupted(Guid journeyId, InterruptionTypes interruptionType, string message, Position newPosition, Guid shipId);

		public Journey AddJourney(Journey journey)
		{
			TryAdd(journey.Id, journey);
			journey.Interrupted += Journey_Interrupted;
			journey.Arrived += Journey_Arrived;
			return journey;
		}

		public bool RemoveJourney(Guid id)
		{
			if(TryGetValue(id, out var j))
				return TryRemove(new KeyValuePair<Guid, Journey>(id, j));
			return false;
		}

		private void Journey_Interrupted(Guid journeyId, InterruptionTypes interruptionType, string message, Position newPosition, Guid shipId)
		{
			Interrupted?.Invoke(journeyId, interruptionType, message, newPosition, shipId);
		}

		private void Journey_Arrived(Guid journeyId, string message, Position newPosition, Guid shipId)
		{
			Arrived?.Invoke(journeyId, message, newPosition, shipId);
		}

		public override string ToString()
		{
			var ret = "";
			foreach (var j in this)
			{
				ret += j.ToString() + "\n";
			}
			return ret.TrimEnd('\n');
		}
	}
}