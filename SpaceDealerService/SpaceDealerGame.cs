using Serilog;
using SpaceDealerModels.Repositories;
using SpaceDealerModels.Units;
using SpaceDealerService;
using SpaceDealerService.API.Abstractions;
using SpaceDealerService.Repos;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace SpaceDealer
{
    public class SpaceDealerGame : ISpaceDealerGame
    {

        public Game Game { get; set; }
        private SqlPersistor _persistor { get; set; }
        public Planets Galaxy { get { return Game.Galaxy; } }
        public Players FleetCommanders { get { return Game.FleetCommanders; } }
        public Sectors ActiveSectors { get { return Game.KnownUniverse; } }
        public string GameId { get { return Game.GameId; } }


        public ILogger Logger { get; set; }
        public SpaceDealerGame(ILogger logger, SqlPersistor persistor)
        {
            Game = new Game();
            Logger = logger;
            _persistor = persistor;

            Repository.Init();

            foreach (var problem in Repository.ValidateIndustryLibrary())
                Logger.Warning($"Industry library: {problem}");

       }

        public void AddPlanets(int amount = 1000)
        {
            for (int i = 0; i < amount; i++)
            {
                var randomPlanet = Repository.GenerateRandomPlanet(DbCoordinates.GenerateRandomCoordniates());
                randomPlanet.Name = $"{randomPlanet.Name}-{i}";
                randomPlanet.PicturePath = ".\\Planets\\image_part_" + Repository.GetRandomNumber(1, 36) + ".jpg";
                Game.Galaxy.AddPlanet(randomPlanet);
            }

            // Erst jetzt, da alle Planeten feststehen, die Bedarfe auf die Umgebung abstimmen.
            var adjusted = 0;
            foreach (var planet in Game.Galaxy.Values)
                adjusted += Repository.AlignNeedsWithNeighbourhood(planet, Game.Galaxy.Values);
            Logger.Information($"Needs aligned with neighbourhood: {adjusted} adjusted.");
        }

        public void Init()
        {
            _persistor.FeaturesRepo.SaveDefaultFeatures();

            Game.KnownUniverse = _persistor.SectorRepo.GetAll();
            Game.Features = _persistor.FeaturesRepo.GetAll();

            foreach (var planet in _persistor.PlanetsRepo.GetAll().Values)
            {
                Game.Galaxy.AddPlanet(planet);
                Game.KnownUniverse.AddPlanet(planet.Sector, planet.Id);
            }

            foreach (var player in _persistor.PlayersRepo.GetAll().Values)
            {
                player.Galaxy = Game.Galaxy;
                var ships = _persistor.ShipsRepo.GetAllShipsByPlayer(player);
                player.Fleet = new Ships(player);
                foreach (var ship in ships.Values)
                {
                    if(ship!= null)
                        player.Fleet.AddShip(ship);
                    else
                        Logger.Warning($"Ship is null for player {player.Id}");
                }
                Game.FleetCommanders.TryAdd(player.Id, player);
            }

            Logger.Verbose($"Planets loaded: " + Game.Galaxy.Count);
            Logger.Verbose($"Fleetcommanders loaded: " + Game.FleetCommanders.Count);
        }

        public string StartNewGame()
        {
            Game = new Game();
            Init();
            
            //Just in case we have not saved the features yet
            _persistor.FeaturesRepo.SaveAll(Game.Features);

            return Game.GameId;
        }

        public void ResetDatabase()
        {
            _persistor.ResetDatabase();
            _persistor.FeaturesRepo.SaveDefaultFeatures();
            AddPlanets();
            AddFleetCommanders();
            Init();
            SaveGameState();
        }

        public void SavePlayers()
        {
            _persistor.SavePlayers(Game.FleetCommanders);
        }

        public void SaveGalaxy()
        {
            _persistor.SaveGalaxy(Game.Galaxy);
        }

        public void Play()
        {
            do
            {
                Game.Update();
                Thread.Sleep(1000);
            } while (true);
        }


        public bool SaveGameState()
        {
            try
            {
                _persistor.SavePlayers(Game.FleetCommanders);
                _persistor.SaveGalaxy(Game.Galaxy);
                _persistor.SaveSectors(Game.KnownUniverse);
                _persistor.SaveFeatures(Game.Features);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to save game state: {ex.Message}");
                return false;
            }
        }

        internal object GetGameStats()
        {
            var stats = new
            {
                GameUID = Game.GameId,
                PlanetsCount = Game.Galaxy.Count,
                FleetCommandersCount = Game.FleetCommanders.Count,
                ActiveSectorsCount = Game.KnownUniverse.Count
            };
            return stats;
        }

        public void AddFleetCommanders(int amount = 250)
        {
            if (Game.FleetCommanders == null)
                Game.FleetCommanders = new Players();

            for (int i = 0; i < amount; i++)
            {
                var planet = Game.Galaxy.GetRandomPlanet();
                var discoveredPlanets = Game.Galaxy.GetRandomPlanets(5);
                var fcn = Repository.GetRandomFleetCommanderName();
                var player = new DbPlayer(fcn, planet, discoveredPlanets, Game);
                var shipName = Repository.GetRandomShipName();
                var ship = new DbShip(shipName, player, Game.Features.GetFeatureSet([FeatureNames.SignalRange_1]), player.Fleet)
                {
                    CargoSize = 30,
                    Parent = player.Fleet,
                    PlayerId = player.Id,
                    PicturePath = ".\\Spaceships\\MediumFrighter.jpg"
                };
                player.PlayerType = Enums.PlayerTypes.NPC;
                player.Fleet.AddShip(ship);
                Game.FleetCommanders.TryAdd(player.Id, player);
            }

        }

        public DbPlayer GetPlayerByName(string name)
        {
            return _persistor.PlayersRepo.GetItemByName(name);
        }

        public DbPlayer GetPlayerById(string id)
        {
            return _persistor.PlayersRepo.GetItemById(id);
        }

        public DbPlanet GetPlanetById(string id)
        {
            return _persistor.PlanetsRepo.GetItemById(id);
        }

        public Planets GetDiscoveredPlanetsByPlayerId(string playerId)
        {
            return _persistor.DiscoveredPlanetsRepo.GetDiscoveredPlanets(playerId);
        }

        public Ships GetAllShipsByPlayer(DbPlayer player)
        {
            return _persistor.ShipsRepo.GetAllShipsByPlayer(player);
        }

        public ConcurrentDictionary<string, DbFeature> GetAllFeaturesByShipId(string shipId)
        {
            return _persistor.FeaturesRepo.GetAll(shipId);
        }

        public DbPlanet GetPlanetByName(string name)
        {
            return _persistor.PlanetsRepo.GetItemByName(name);
        }

        public void SavePlanet(DbPlanet planet)
        {
            _persistor.PlanetsRepo.Save(planet);
        }

        public void SavePlayer(DbPlayer player)
        {
            _persistor.PlayersRepo.Save(player);
        }

        internal void LoadGame(string gameUID)
        {
            throw new NotImplementedException();
        }
    }
}
