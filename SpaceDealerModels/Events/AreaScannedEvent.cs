using SpaceDealer.Enums;
using SpaceDealerModels.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceDealerModels.Events
{

    public interface IGameEvents
    {
        event Action<AreaScannedEvent> Scanned;
        event Action<JourneyArrivedEvent> Arrived;
        event Action<JourneyInterruptedEvent> Interrupted;
        void Publish(ProbeSignalLostEvent e);
        void Publish(AreaScannedEvent e);
        void Publish(JourneyArrivedEvent e);
        void Publish(JourneyInterruptedEvent e);
    }

    public class ProbeSignalLostEvent
    {
        public InterruptionType Type { get; }
        public string Message { get; }
        public DbShip Ship { get; }
        public Probe Probe { get; }
        public DbPlayer Player { get; }
        
        public ProbeSignalLostEvent(InterruptionType type, string message, Probe probe, DbShip ship, DbPlayer player)
        {
            Probe = probe;
            Type = type;
            Message = message;
            Ship = ship;
            Player = player;
        }
    }


    public class AreaScannedEvent
    {
        public InterruptionType Type { get; }
        public string Message { get; }
        public DbShip Ship { get; }
        public Probe Probe { get; }
        public DbPlayer Player { get; }
        public Planets Planets { get; }
        public Ships Ships { get; }

        public AreaScannedEvent(InterruptionType type, string message, Probe probe, DbShip ship, DbPlayer player, Planets planets, Ships ships)
        {
            Probe = probe;
            Type = type;
            Message = message;
            Ship = ship;
            Player = player;
            Planets = planets;
            Ships = ships;
        }
    }

    public class JourneyArrivedEvent
    {
        public InterruptionType Type { get; } = InterruptionType.ArrivedAtDestination;
        public DbPlayer Player { get; }
        public string Message { get; }
        public DbCoordinates Position { get; }
        public DbShip Ship { get; }
        public JourneyArrivedEvent( string message, DbPlayer player, DbCoordinates position, DbShip ship)
        {
            Message = message;
            Position = position;
            Player = player;
            Ship = ship;
        }
  
    }

    public class JourneyInterruptedEvent
    {

        public DbPlayer Player { get; }
        public InterruptionType Type { get; }
        public string Message { get; }
        public DbShip Ship { get; }
        public DbCoordinates Position { get; }
        public JourneyInterruptedEvent(InterruptionType type, DbPlayer player, string message, DbShip ship, DbCoordinates position)
        {
            Type = type;
            Player = player;
            Message = message;
            Ship = ship;
            Position = position;
        }

    }
}


