using Serilog;
using SpaceDealerModels.Events;
using System;

namespace SpaceDealerModels.Units
{

    public class Game 
    {
        public string GameId { get; set; }
        public Players FleetCommanders { get; set; }
        public Planets Galaxy { get; set; }
        public Sectors KnownUniverse { get; set; }

        public IGameEvents Events { get; } = new GameEvents();

        public Game()
        {
            GameId = Guid.NewGuid().ToString();
            FleetCommanders = new Players();
            Galaxy = new Planets();
            KnownUniverse = new Sectors();

            Events.Arrived += FleetCommanders_Arrived;
            Events.Interrupted += FleetCommanders_Interrupted;
            Events.Scanned += FleetCommanders_Scanned;
        }

        private void FleetCommanders_Interrupted(JourneyInterruptedEvent @event)
        {
            Log.Verbose("FleetCommanders_Interrupted: {Message}, InterruptionType: {InterruptionType}, Ship: {Ship}, Player: {Player}, NewPosition: {NewPosition}", @event.Message, @event.Type, @event.Ship, @event.Player, @event.Position);
            @event.Player.HandleInterruption(@event.Type, @event.Message, @event.Ship, @event.Ship.Cruise.CurrentSector);
        }

        private void FleetCommanders_Scanned(AreaScannedEvent @event)
        {
            Log.Verbose("FleetCommanders_Scanned: {Message}, Ship: {Ship}, Player: {Player}, ScannedPlanets: {ScannedPlanets}, ScannedShips: {ScannedShips}", @event.Message, @event.Ship, @event.Player, @event.Planets, @event.Ships);
            @event.Player.HandleAreaScanned(@event.Type, @event.Message, @event.Ship, @event.Player, @event.Planets, @event.Ships);
        }

        private void FleetCommanders_Arrived(JourneyArrivedEvent @event)
        {
            Log.Verbose("FleetCommanders_Arrived: {Message}, NewPosition: {NewPosition}, Ship: {Ship}, Player: {Player}", @event.Message, @event.Position, @event.Ship, @event.Player);
            @event.Player.HandleArrival(@event.Message, @event.Position, @event.Ship);
        }
        
        public Ships ScanShipsInSector(DbCoordinates sector)
        {
            var shipsInSector = new Ships();
            foreach (var commander in FleetCommanders)
            {
                foreach (var ship in commander.Value.Fleet.Values)
                {
                    if (ship.Cruise.CurrentSector.Equals(sector))
                    {
                        shipsInSector.TryAdd(ship.Id, ship);
                    }
                }
            }
            return shipsInSector;
        }

        public void Update()
        {
            foreach (var planet in Galaxy.Values)
            {
                planet.Update();
                //	Logger.Log(planet.ToString(), TraceEventType.Verbose);
            }
            foreach (var commander in FleetCommanders)
            {
                commander.Value.Update();
                FleetCommanders.TryUpdate(commander.Key, commander.Value, commander.Value);
            }
        }
    }
}
