using System.Formats.Asn1;
using Terminal.Gui;


namespace SpaceDealerTerminal
{
    // Defines a top-level window with border and title
    public class NewGameWindow : Window
    {
        public static string CommanderName { get; set; }
        public static string ShipName { get; set; }
        public NewGameWindow()
        {
            // Create input components and labels
            var commanderNameLbl = new Label { Text = "Name des Commanders:" };
            var commanderNameText = new TextField
            {
                // Position text field adjacent to the label
                X = Pos.Right(commanderNameLbl) + 1,
                // Fill remaining horizontal space
                Width = Dim.Fill()
            };
            var shipNameLbl = new Label
            {
                Text = "Name des Schiffs:",
                X = Pos.Left(commanderNameLbl),
                Y = Pos.Bottom(commanderNameLbl) + 1
            };
            var shipNameText = new TextField
            {
                // align with the text box above
                X = Pos.Left(commanderNameText),
                Y = Pos.Top(shipNameLbl),
                Width = Dim.Fill()
            };
            // Create login button
            var btnLogin = new Button
            {
                Text = "Neues Spiel",
                Y = Pos.Bottom(shipNameLbl) + 1,
                // center the login button horizontally
                X = Pos.Center(),
                IsDefault = true
            };
            // When login button is clicked display a message popup
            // Replace the incorrect 'Accepting' event with the correct 'Clicked' event for the Button class.
            btnLogin.Clicked += async () =>
            {
                await StartNewGame(commanderNameText, shipNameText);
            };
            // Add all components to the window
            Add(commanderNameLbl, commanderNameText, shipNameLbl, shipNameText, btnLogin);
        }

        private static async Task StartNewGame(TextField commanderNameText, TextField shipNameText)
        {
            CommanderName = commanderNameText.Text.ToString();
            ShipName = shipNameText.Text.ToString();

            var playerNameExists = await GameProxy.PlayerNameTaken(CommanderName);
            if (playerNameExists == true)
            {
                MessageBox.ErrorQuery("Neues Spiel", "Der Spielername ist schon vergeben.", "Ok");
                return;
            }

            var shipNameExists = await GameProxy.ShipNameTaken(CommanderName, ShipName);
            if (shipNameExists == true)
            {
                MessageBox.ErrorQuery("Neues Spiel", "Der Name des Schiffs ist schon vergeben.", "Ok");
                return;
            }

            Program.CurrentPlayer = await GameProxy.AddPlayer(CommanderName, ShipName);
            var ship = GameProxy.AddShip(CommanderName, ShipName).Result;
            var result = GameProxy.SaveGame(CommanderName).Result;
            Program.CurrentPlayer.Ships.Add(ship);
            Program.CurrentShip = ship;

            MessageBox.Query($"Willkommen, Commander {Program.CurrentPlayer.Name}!", "Ein neues Spiel kann beginnen!", "Ok");

            var gameWindow = new GameWindow();
            Application.Run(gameWindow);
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