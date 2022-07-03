using SpaceDealer;
using SpaceDealerModels.Units;
using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace SpaceDealerService.Repos
{
    public class SqlPersistor
	{
        private int CurrentlyOpenConnections = 0;

        public string DbPath { get; set; }
        public ILogger Logger { get; set; }

        public PlanetsRepository PlanetsRepo { get; set; }
        public PlayersRepository PlayersRepo { get; set; }
        public ShipsRepository ShipsRepo { get; set; }
        public ProductsRepository ProductRepo { get; set; }
        public NeededProductsRepository NeededProductsRepo { get; set; }
        public GeneratedProductsRepository GeneratedProductsRepo { get; set; }
        public DiscoveredPlanetsRepository DiscoveredPlanetsRepo { get; set; }
        public FeaturesRepository FeaturesRepo { get; set; }
        public MarketRepository MarketRepo { get; set; }
        public SQLiteConnection Connection { get; set; }
        public SectorRepository SectorRepo { get; set; }
        public SqlPersistor(ILogger logger, string dbPath)
        {
            Logger = logger;
            DbPath = dbPath;

            PlanetsRepo = new PlanetsRepository(this);
            PlayersRepo = new PlayersRepository(this);
            ShipsRepo = new ShipsRepository(this);
            ProductRepo = new ProductsRepository(this);
            NeededProductsRepo = new NeededProductsRepository(this);
            GeneratedProductsRepo = new GeneratedProductsRepository(this);
            DiscoveredPlanetsRepo = new DiscoveredPlanetsRepository(this);
            MarketRepo = new MarketRepository(this);
            FeaturesRepo = new FeaturesRepository(this);
            SectorRepo = new SectorRepository(this);

            if (!File.Exists(DbPath))
                CreateDatabase();

            Connection = new SQLiteConnection("Data Source=" + DbPath);
            OpenConnection();
        }

        private void OpenConnection() 
        {
            Connection.Open();
            CurrentlyOpenConnections += 1;
            StackTrace stackTrace = new StackTrace();
            Logger.Log($"OPEN [{CurrentlyOpenConnections}] - {stackTrace.GetFrame(1).GetMethod().ReflectedType.Name}:{stackTrace.GetFrame(1).GetMethod().Name}", TraceEventType.Start);
        }

        private void CloseConnection()
        {
            CurrentlyOpenConnections -= 1;
            Connection.Close();
            StackTrace stackTrace = new StackTrace();
            Logger.Log($"CLOSE [{CurrentlyOpenConnections}] - {stackTrace.GetFrame(1).GetMethod().ReflectedType.Name}:{stackTrace.GetFrame(1).GetMethod().Name}", TraceEventType.Stop);
        }

        public bool SaveGalaxy(Planets galaxy)
        {
            
            foreach (var planet in galaxy)
            {
                PlanetsRepo.Save(planet);

                foreach (var np in planet.Industry.ProductsNeeded)
                {
                    ProductRepo.Save(np);
                    NeededProductsRepo.SaveNeededProduct(planet.Id, np.Id);
                }
                
                foreach (var np in planet.Industry.GeneratedProducts)
                {
                    ProductRepo.Save(np);
                    GeneratedProductsRepo.SaveGeneratedProduct(planet.Id, np.Id);
                }

                MarketRepo.SaveMarket(planet.Id, planet.Market);
            }
            return true;
        }

        public bool SavePlayers(Players players)
        {
            try
            {
                foreach (var player in players)
                {
                    SavePlayer(player);
                }
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to persist players {e.Message}.", TraceEventType.Error);
                return false;
            }
         
            return true;
        }

        public void SavePlayer(DbPlayer player)
        {
            PlayersRepo.Save(player);
            foreach (var ship in player.Fleet)
            {
                ShipsRepo.Save(ship);
            }

            if (player.DiscoveredPlanets != null)
            {
                foreach (var planet in player.DiscoveredPlanets)
                {
                    DiscoveredPlanetsRepo.SaveDiscoveredPlanet(player.Id, planet.Id);
                }
            }
        }

        public void CreateDatabase()
        {
            SQLiteConnection.CreateFile(DbPath);

            var dir = Path.GetDirectoryName(DbPath);
            var fils = Directory.GetFiles(dir, "Create*.sql");
            foreach (var fil in fils)
            {
                var sql = File.ReadAllText(fil);
                CreateTable(sql);
            }
        }

        public void CreateTable(string sql)
        {
            try
            {
                using (var connection = new SQLiteConnection("Data Source=" + DbPath))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(connection))
                    {
                        // Erstellen der Tabelle, sofern diese noch nicht existiert.
                        command.CommandText = sql;
                        try
                        {
                            command.ExecuteNonQuery();
                            Logger.Log($"Table creted.", TraceEventType.Information);
                        }
                        catch (Exception e)
                        {
                            Logger.Log($"Failed to create table {e.Message}", TraceEventType.Error);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Logger.Log($"Failed to create table{e.Message}", TraceEventType.Error);
            }

        }


    }
}
