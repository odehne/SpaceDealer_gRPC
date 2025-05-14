using Terminal.Gui;


namespace SpaceDealerTerminal
{
    // Defines a top-level window with border and title
    public class GameWindow : Window
    {

        public GameWindow()
        {
            var menu = new MenuBar(new MenuBarItem[]
            {
                new MenuBarItem("_File", new MenuItem[]
                {
                    new MenuItem("_Quit", "Quit game", () => { Application.RequestStop(); })
                }),
                new MenuBarItem("_Help", new MenuItem[]
                {
                    new MenuItem("_About", "Show about dialog", () => { MessageBox.Query("About", "Space Dealer Terminal App", "Ok"); })
                })
            });
            Add(menu);

            var playerFrame = new FrameView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(50),
                Height = Dim.Fill()
            };
            var playerLabel = new Label("Commander: " + Program.CurrentPlayer.Name)
            {
                X = 1,
                Y = 0,
                Width = Dim.Fill(),
                Height = 1
            };
            var shipLabel = new Label("Schiff: " + Program.CurrentShip.ShipName)
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill(),
                Height = 1
            };
            var creditsLabel = new Label("Credits: " + Program.CurrentPlayer.Credits)
            {
                X = 1,
                Y = 2,
                Width = Dim.Fill(), 
                Height = 1
            };
            var cargoLabel = new Label("Cargo: " + Program.CurrentShip.CargoLoad.CalculateSize() + "t")
            {
                X = 1,
                Y = 3,
                Width = Dim.Fill(),
                Height = 1
            };
            var shipLabel2 = new Label("Position: " + OutputHelper.GetCurrentShipPosition(Program.CurrentShip))
            {
                X = 1,
                Y = 4,
                Width = Dim.Fill(),
                Height = 1
            };
            playerFrame.Add(playerLabel);
            playerFrame.Add(shipLabel);
            playerFrame.Add(creditsLabel);
            playerFrame.Add(cargoLabel);
            playerFrame.Add(shipLabel2);
            Add(playerFrame);
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