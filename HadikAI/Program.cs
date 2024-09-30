using ReinforcementLearningModels;

namespace HadikAI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Inicializace herního prostředí
            int width = 20;
            int height = 15;
            var snakeGame = new Snake(width, height);
            var actions = new List<(int x, int y)> { (0, -1), (0, 1), (-1, 0), (1, 0) }; // Nahoru, dolů, doleva, doprava

            // Inicializace Q-learning agenta
            var agent = new QLearningAgent<(int x, int y), (int x, int y)>(0.1, 0.9, 1.0, 0.1, 0.995);

            for (int episode = 0; episode < 1000; episode++) // 1000 epizod tréninku
            {
                snakeGame = new Snake(width, height); // Nová hra
                bool gameOver = false;

                while (!gameOver)
                {
                    // Získání akce od agenta
                    var action = agent.GetAction((snakeGame.Body[0].x, snakeGame.Body[0].y), actions);

                    // Aktualizace směru hada
                    snakeGame.Direction = action;

                    try
                    {
                        snakeGame.Move();
                        double reward = (snakeGame.Body[0] == snakeGame.Food) ? 10 : -0.1; // Odměna za jídlo
                        agent.UpdateQValue((snakeGame.Body[0].x, snakeGame.Body[0].y), action, reward, (snakeGame.Body[0].x, snakeGame.Body[0].y), actions);
                    }
                    catch (InvalidOperationException)
                    {
                        reward = -10; // Trest za kolizi
                        gameOver = true; // Konec hry
                    }
                }

                agent.UpdateExplorationRate(); // Aktualizace míry explorace
                Console.WriteLine($"Episode {episode + 1} finished.");
            }

            // Testování agenta
            snakeGame = new Snake(width, height); // Nová hra pro testování
            bool testing = true;
            while (testing)
            {
                var action = agent.GetAction((snakeGame.Body[0].x, snakeGame.Body[0].y), actions);
                snakeGame.Direction = action;

                try
                {
                    snakeGame.Move();
                    Console.WriteLine($"Snake position: {snakeGame.Body[0].x}, {snakeGame.Body[0].y} | Food position: {snakeGame.Food.x}, {snakeGame.Food.y}");
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Game Over!");
                    testing = false; // Konec testování
                }

                Thread.Sleep(100); // Krátké zpoždění pro pozorování
            }
        }
    }
}
