using Serilog;
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
            Logger.Information($"OPEN [{CurrentlyOpenConnections}] - {stackTrace.GetFrame(1).GetMethod().ReflectedType.Name}:{stackTrace.GetFrame(1).GetMethod().Name}");
        }

        public bool ResetDatabase()
        {
            try
            {
                Connection.Close();
                File.Delete(DbPath);
                CreateDatabase();
                Connection = new SQLiteConnection("Data Source=" + DbPath);
                OpenConnection();
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"Failed to reset database {e.Message}");
                return false;
            }
        }

        public bool SaveGalaxy(Planets galaxy)
        {
            
            foreach (var planet in galaxy.Values)
            {
                PlanetsRepo.Save(planet);

                foreach (var np in planet.Industry.ProductsNeeded)
                {
                    var prod = ProductRepo.GetItem(np.Name, null);
                    var npId = np.Id;
                    if (prod != null)
                        npId = prod.Id;
                    ProductRepo.Save(np);
                    NeededProductsRepo.SaveNeededProduct(planet.Id, npId);
                }
                
                foreach (var np in planet.Industry.GeneratedProducts)
                {
                    var prod = ProductRepo.GetItem(np.Name, null);
                    var npId = np.Id;
                    if (prod != null)
                        npId = prod.Id;
                    ProductRepo.Save(np);
                    GeneratedProductsRepo.SaveGeneratedProduct(planet.Id, npId);
                }

                //MarketRepo.SaveMarket(planet.Id, planet.Market);
            }

            Logger.Information($"Galaxy persisted: {galaxy.Count} planets.");
            return true;
        }


        public bool SaveSectors(Sectors sectors)
        {
            SectorRepo.SaveSectors(sectors);
            Logger.Information($"Known universe persisted: {sectors.Count} sectors.");
            return true;
        }


        public bool SavePlayers(Players players)
        {
            try
            {
                foreach (var player in players.Values)
                {
                    SavePlayer(player);
                }
                Logger.Information($"Players persisted: {players.Count}.");
            }
            catch (Exception e)
            {
                Logger.Error($"Failed to persist players {e.Message}.");
                return false;
            }
         
            return true;
        }

        public void SavePlayer(DbPlayer player)
        {
            PlayersRepo.Save(player);
            foreach (var ship in player.Fleet.Values)
            {
                ShipsRepo.Save(ship);
            }

            if (player.DiscoveredPlanets.Values != null)
            {
                foreach (var planet in player.DiscoveredPlanets.Values)
                {
                    // ensure that the discovered planet is also in the planets table,
                    // otherwise we will have a foreign key violation
                    var p = PlanetsRepo.GetItem(planet.Name, null);
                    if(p == null)
                        PlanetsRepo.Save(planet);
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
                            Logger.Information($"Table created.");
                        }
                        catch (Exception e)
                        {
                            Logger.Error($"Failed to create table {e.Message}");
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
                Logger.Error($"Failed to create table{e.Message}");
            }

        }


    }
}
