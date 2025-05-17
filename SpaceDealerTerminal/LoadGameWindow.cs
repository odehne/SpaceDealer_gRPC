using Terminal.Gui;


namespace SpaceDealerTerminal
{
    public class LoadGameWindow : Window
    {
        public static string CommanderName { get; set; }
        public LoadGameWindow()
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
           
            // Create login button
            var btnLogin = new Button
            {
                Text = "Spiel laden",
                Y = Pos.Bottom(commanderNameLbl) + 1,
                // center the login button horizontally
                X = Pos.Center(),
                IsDefault = true
            };
            // When login button is clicked display a message popup
            // Replace the incorrect 'Accepting' event with the correct 'Clicked' event for the Button class.
            btnLogin.Clicked += async () =>
            {
                await LoadGame(commanderNameText);
            };
            // Add all components to the window
            Add(commanderNameLbl, commanderNameText, btnLogin);
        }

        private static async Task LoadGame(TextField commanderNameText)
        {
            CommanderName = commanderNameText.Text.ToString();
          
            var player = await GameProxy.GetPlayer(CommanderName);
            if (player == null)
            {
                MessageBox.ErrorQuery("Spiel laden", $"Spieler [{CommanderName}] nicht gefunden.", "Ok");
                return;
            }

            var ship = player.Ships[0];

            Program.CurrentPlayer = player;
            Program.CurrentShip = ship;

            MessageBox.Query($"Willkommen, Commander {Program.CurrentPlayer.Name}!", "Das Spiel kann fortgesetzt werden!", "Ok");

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