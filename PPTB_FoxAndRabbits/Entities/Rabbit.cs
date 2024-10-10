namespace EntitesLib
{
    public class Rabbit
    {
        public int Age { get; private set; }
        public bool IsAlive { get; private set; }
        private const int MaxAge = 5; // Maximum age for rabbits
        private int energy;

        public Rabbit()
        {
            energy = 10; // Initial energy level
            Age = 0;
            IsAlive = true; // Starts as alive
        }

        public void Move(Cell[,] grid, int x, int y)
        {
            Random rand = new Random();
            int newX = x + rand.Next(-1, 2); // -1, 0, or 1
            int newY = y + rand.Next(-1, 2); // -1, 0, or 1

            if (newX >= 0 && newX < grid.GetLength(0) && newY >= 0 && newY < grid.GetLength(1))
            {
                // Move only if the cell is empty
                if (grid[newX, newY].Rabbit == null && grid[newX, newY].Fox == null)
                {
                    grid[newX, newY].Rabbit = this;
                    grid[x, y].Rabbit = null; // Leave current cell
                }
            }
        }


        public void Eat(GrassState grass)
        {
            if (grass == GrassState.Young)
            {
                Console.WriteLine("Rabbit eats the grass.");
                grass = GrassState.Empty;
                energy += 5; // Increase energy if grass is eaten
            }
            else
            {
                Console.WriteLine("Rabbit cannot eat this grass.");
            }
        }


        public bool Survive()
        {
            // Check survival based on age or other conditions
            Age++;
            energy--; // Decrease energy over time
            if (energy < 0 || Age > MaxAge)
            {
                IsAlive = false; // Rabbit dies if energy is depleted or age exceeds max age
            }
            return IsAlive;
        }

        public Rabbit Reproduce()
        {
            if (IsAlive && Age > 1 && energy > 5) // Reproduce if alive, older than 1, and has enough energy
            {
                Console.WriteLine("Rabbit reproduces!");
                return new Rabbit(); // Return a new rabbit
            }
            return null; // No reproduction
        }
    }
}
