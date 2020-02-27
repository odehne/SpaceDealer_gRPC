using Grpc.Net.Client;
using SpaceDealerService;
using System;
using System.Threading.Tasks;

namespace SpaceDealerUI
{
	class Program
	{
        static async Task Main(string[] args)
        {
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Game.GameClient(channel);
            var reply = await client.GetShipsAsync(new ShipsRequest { PlayerName = "oliver" });
       
            foreach (var ship in reply.Ships)
            {
                Console.WriteLine($"Ship: {ship.ShipName} [{ship.Cruise.Departure.X},{ship.Cruise.Departure.Y},{ship.Cruise.Departure.Z}] Cargo Size: {ship.CargoSize} Load: {ship.CargoLoad.CargoName}");
            }

            Console.WriteLine();

            var reply2 = await client.GetPlanetsAsync(new PlanetsRequest());
            foreach (var planet in reply2.Planets)
            {
                Console.WriteLine($"Planet: {planet.PlanetName} [{planet.Sector.X},{planet.Sector.Y},{planet.Sector.Z}]");
            }

            var cruiseStarted = await client.StartCruiseAsync(new CruiseRequest { DestinationPlanetName = "tatooine", PlayerName = "oliver", ShipName = "Dark Star" });

            Console.WriteLine("Dark star started: " + cruiseStarted.OnItsWay);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

       
    }
}
