using Terminal.Gui;


namespace SpaceDealerTerminal
{
    // Defines a top-level window with border and title
    public class GameWindow : Window
    {

        public GameWindow()
        {
            var menu = new MenuBar(
            [
                new MenuBarItem("_File",
                [
                    new MenuItem("_Quit", "Quit game", () => { Application.RequestStop(); })
                ]),
                new MenuBarItem("_Help",
                [
                    new MenuItem("_About", "Show about dialog", () => { MessageBox.Query("About", "Space Dealer Terminal App", "Ok"); })
                ])
            ]);
            Add(menu);

            var playerFrame = new FrameView()
            {
                Text = "Commander",
                Width = Dim.Percent(50),
                Height = Dim.Percent(25),
            };

            var shipFrame = new FrameView()
            {
                Text = "Schiff",
                Y = Pos.Bottom(playerFrame),
                Width = Dim.Percent(50),
                Height = Dim.Percent(75),
            };

            var actionFrame = new FrameView()
            {
                Text = "Aktionen",
                X = Pos.Right(playerFrame),
                Width = Dim.Percent(50),
                Height = Dim.Fill(),
            };

            Add(playerFrame);
            Add(shipFrame);
            Add(actionFrame);

            var playerLabel = new Label("Name: " + Program.CurrentPlayer.Name)
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill(),
                Height = 1
            };
            var hpLabel = new Label("HeimatPlanet: " + Program.CurrentPlayer.HomePlanet)
            {
                X = 1,
                Y = 2,
                Width = Dim.Fill(),
                Height = 1
            };
            var creditsLabel = new Label("Credits: " + Program.CurrentPlayer.Credits)
            {
                X = 1,
                Y = 3,
                Width = Dim.Fill(),
                Height = 1
            };
            var fleetSizeLabel = new Label("Anzahl Schiffe: " + Program.CurrentPlayer.Ships.Count)
            {
                X = 1,
                Y = 4,
                Width = Dim.Fill(),
                Height = 1
            };

            var shipLabel = new Label("Name: " + Program.CurrentShip.ShipName)
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill(),
                Height = 1
            };
            
            var cargoLabel = new Label("Cargo: " + Program.CurrentShip.CargoLoad.CalculateSize() + "t")
            {
                X = 1,
                Y = 2,
                Width = Dim.Fill(),
                Height = 1
            };

            var shipLabel2 = new Label("Position: " + OutputHelper.GetCurrentShipPosition(Program.CurrentShip))
            {
                X = 1,
                Y = 3,
                Width = Dim.Fill(),
                Height = 1
            };
            
            playerFrame.Add(playerLabel);
            playerFrame.Add(hpLabel);
            playerFrame.Add(creditsLabel);
            playerFrame.Add(fleetSizeLabel);


            shipFrame.Add(shipLabel);
            shipFrame.Add(cargoLabel);
            shipFrame.Add(shipLabel2);
            
            
            
           
        }
    }
}

//Application.Init();

//try
//{
//    var exampleWindow = new ExampleWindow
//    {
//        // Set the size of the window
//        Width = Dim.Fill(),
//        Height = Dim.Fill()
//    };
//    Application.Run(exampleWindow);
//}
//finally
//{
//    Application.Shutdown();
//    // To see this output on the screen it must be done after shutdown,
//    // which restores the previous screen.
//    Console.WriteLine($@"Username: {ExampleWindow.UserName}");
//}