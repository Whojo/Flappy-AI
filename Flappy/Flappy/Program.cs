using System;
using System.Threading;

namespace Flappy
{
    internal class Program
    {
        /// <summary>
        /// The main function:
        /// - Register the managers
        /// - Register the drawers
        /// - Register the birds
        /// - Initialize the game
        /// - Play
        /// </summary>
        public static void Main()
        {
            // Hide the cursor
            Console.CursorVisible = false;

            // Get one random generator for all the game
            Random rnd = new Random();

            // Initialize the keyboard manager, which will handle the keyboard input
            KeyboardManager manager = new KeyboardManager();

            // Initialize the console drawer, which will handle the console output
            ConsoleDrawer drawer = new ConsoleDrawer(Console.WindowWidth, Console.WindowHeight);

            // Initialize the game with the random generator and the output drawer
            Game game = new Game(rnd, drawer);
            // Add the keyboard manager to the game
            game.Add(manager);

            // Create a player
            Bird player = new Bird(new KeyboardController(manager, ConsoleKey.Spacebar));
            // Associate the player with a color in the console drawer
            drawer.Associate(player, ConsoleColor.Red);

            // Create an AI
            Bird ai = new Bird(new BestController());

            // Associate the ai with a color in the console drawer
            drawer.Associate(ai, ConsoleColor.Blue);

            // Add the player and the ai to the game
            game.Add(player);
            game.Add(ai);

            // Start the game and draw it once
            game.Start();
            game.Draw();
            
            // While there is someone alive, continue
            while (game.Continue)
            {
                // Game loop : update, draw and sleep
                game.Update();
                game.Draw();
                game.Sleep();
            }
            // Stop the game
            game.Stop();
            Console.Clear();

            // Write the scores
            Console.WriteLine("Player scored : " + player.Score);
            Console.WriteLine("AI scored : " + ai.Score);
            
            // Read a key (for external terminal users)
            Console.Read();
        }
    }
}