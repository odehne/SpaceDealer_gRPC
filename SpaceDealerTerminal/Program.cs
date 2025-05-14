<<<<<<< HEAD
﻿using SpaceDealerService;
using SpaceDealerTerminal;
using Terminal.Gui;


namespace SpaceDealerTerminal
{

    public class Program
    {

        public static Player CurrentPlayer { get; set; }
        public static Ship CurrentShip { get; set; }
        public static Planet CurrentPlanet { get; set; }

        private delegate void GameUpdateDelegate(Ship ship, UpdateStates updateState);


        // Main entry point for the application
        public static void Main(string[] args)
        {
            // Initialize the application
            Application.Init();

            var updateThread = new Thread(GetGameUpdates) { IsBackground = true };
            updateThread.Start();

            // Create and run the main window
            var mainWindow = new Window
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            var menu = new MenuBar(new MenuBarItem[]
            {
                new MenuBarItem("_File", new MenuItem[]
                {
                    new MenuItem("_New", "New game", () => 
                    { 
                        var ngw = new NewGameWindow();
                        Application.Run(ngw);
                       
                    }),
                    new MenuItem("_Quit", "Quit game", () => { Application.RequestStop(); })
                }),
                new MenuBarItem("_Help", new MenuItem[]
                {
                    new MenuItem("_About", "Show about dialog", () => { MessageBox.Query("About", "Space Dealer Terminal App", "Ok"); })
                })
            });

            mainWindow.Add(menu);
            Application.Run(mainWindow);
        }

        private static void GetGameUpdates()
        {
            do
            {
                if (Program.CurrentPlayer != null)
                {
                    var updates = GameProxy.GetUpdates(Program.CurrentPlayer.Name).Result;
                    if (updates != null)
                    {
                        foreach (var u in updates)
                        {
                            UpdateShip(u.Ship, u.UpdateState);
                        }
                    }
                }
                Thread.Sleep(1000);
            } while (true);

        }


        private static void UpdateShip(Ship ship, UpdateStates updateState)
        {
            switch (updateState)
            {
                case UpdateStates.ArrivedOnTarget:
                    ArrivedAtTarget(ship);
                    //	TheMenu.ShowPlanetInfo(u.Ship);
                    break;
                case UpdateStates.NewPlanetDiscovered:
                    FoundNewPlanet(ship);
                    break;
                case UpdateStates.UnderAttack:
                    //	TheMenu.ShowAttackMenu(u.Ship);
                    UnderAttack(ship);
                    break;
            }
        }

        public static void ClearMessagePanel()
        {
            //for (int i = fp1.Controls.Count - 1; i > 0; i--)
            //{
            //    if (fp1.Controls[i].GetType() == typeof(TravellingControl))
            //        fp1.Controls.Remove(fp1.Controls[i]);
            //}
        }

        private static void UnderAttack(Ship ship)
        {
            ClearMessagePanel();
            var position = ship.Cruise.CurrentSector.ToPosition();
            //var mc1 = new DistressCallControl();
            //mc1.SetMessage(Program.PeopleAssets.GetRandomAsset().Path, $"Roter Alarm!", $"Wir machen deinen Mini-Frachter fertig! Du hast keine Chance gegen uns. Gib lieber gleich auf oder wir zermalmen dich!\nPosition: {position}.", ship.Cruise.CurrentSector);
            //fp1.Controls.Add(mc1);
        }

        private static void ArrivedAtTarget(Ship ship)
        {
            ClearMessagePanel();
            var planetName = ship.CurrentPlanet.PlanetName;
            var position = ship.Cruise.Destination.ToPlanetPosition();
            //var mc1 = new FoundNewPlanetControl();
            //mc1.SetMessage(Program.PlanetAssets.GetRandomAsset().Path, $"Willkommen in {planetName}", $"Dein Frachter {ship.ShipName} befindet sich im Orbit des Zielplanetens.\nPosition: {position}."); ;
            //fp1.Controls.Add(mc1);
        }

        private static void FoundNewPlanet(Ship ship)
        {
            ClearMessagePanel();
            var planetName = ship.Cruise.NewPlanetDiscovered.PlanetName;
            var position = ship.Cruise.NewPlanetDiscovered.ToPlanetPosition();
            //var mc1 = new FoundNewPlanetControl();
            //mc1.SetMessage(Program.PlanetAssets.GetRandomAsset().Path, $"Willkommen in {planetName}", $"Dein Frachter {ship.ShipName} hat einen neuen Planeten entdeckt.\nPosition: {position}.");
            //fp1.Controls.Add(mc1);
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
=======
﻿
using Terminal.Gui;

var win = new Window()
{
    Width = Dim.Fill(),
    Height = Dim.Fill()
};

Application.Init();

Application.Top.Add(win);
//var button = new Button
//{
//    Text = "Hello"
//};
//win.Add(button);


var frame1 = new FrameView("Frame 1")
{
    Width = Dim.Percent(50),
    Height = Dim.Fill(),
};

var frame2 = new FrameView("Frame 2")
{
    X = Pos.Right(frame1),
    Width = Dim.Percent(50),
    Height = Dim.Fill(),
};

var lbl1 = new Label("Frame contents 1")
{
    Height =1,
    Width = 20,
};

var lbl2= new Label("Frame contents 2")
{
    Height = 1,
    Width = 20,
};

frame1.Add(lbl1);
frame2.Add(lbl2);

win.Add(frame1, frame2);

Application.Run();
>>>>>>> dbd57fff9c962da63d94361fc58baa3c51357c6a
