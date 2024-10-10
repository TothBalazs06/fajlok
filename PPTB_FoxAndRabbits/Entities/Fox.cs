using System;

namespace EntitesLib
{
    public class Fox
    {

        private int energy;
        public int Age { get; private set; }
        public bool IsAlive { get; private set; }
        private const int MaxAge = 6; // maximum életkor
        private const int ReproductionEnergyThreshold = 15; // ennyi energiára van szüksége a rókának a reprodukáláshoz
        private const int MinimumEnergyToSurvive = 0; // ha a róka energiája kisebb mint ez akkor meghal

        public Fox()
        {
            Age = 0;
            IsAlive = true; // élőként indul 
            energy = 10; // alap energia
        }

        // Mozgás
        public void Move(Cell[,] grid, int x, int y)
        {
            Random rand = new Random();
            int newX = x + rand.Next(-1, 2);
            int newY = y + rand.Next(-1, 2);

            if (newX >= 0 && newX < grid.GetLength(0) && newY >= 0 && newY < grid.GetLength(1))
            {
                // Move only if the cell is empty
                if (grid[newX, newY].Fox == null && grid[newX, newY].Rabbit == null)
                {
                    grid[newX, newY].Fox = this;
                    grid[x, y].Fox = null; // Leave current cell
                }
            }
        }

        public void EatRabbit()
        {
            energy += 10; // Increase energy when eating a rabbit
        }

        public bool Survive()
        {
            // Check survival based on age or other conditions
            Age++;
            energy--; // Decrease energy over time

            if (energy < MinimumEnergyToSurvive || Age > MaxAge)
            {
                IsAlive = false; // Fox dies if energy is depleted or age exceeds max age
            }
            return IsAlive;
        }

        public Fox Reproduce()
        {
            // Fox can reproduce if it has enough energy and is older than 2 years
            if (IsAlive && Age > 2 && energy > ReproductionEnergyThreshold)
            {
                energy -= 5; // Reproduction consumes energy
                return new Fox(); // Return a new fox
            }
            return null; // No reproduction
        }
        //private int energy;
        //public int Age { get; private set; }
        //public bool IsAlive { get; private set; }
        //private const int MaxAge = 6; // Example age limit for foxes

        //public Fox()
        //{
        //    Age = 0;
        //    IsAlive = true; // Starts as alive
        //    energy = 10; // Initial energy level
        //}

        //public void Move(Cell[,] grid, int x, int y)
        //{
        //    Random rand = new Random();
        //    int newX = x + rand.Next(-1, 2);
        //    int newY = y + rand.Next(-1, 2);

        //    if (newX >= 0 && newX < grid.GetLength(0) && newY >= 0 && newY < grid.GetLength(1))
        //    {
        //        // Move only if the cell is empty
        //        if (grid[newX, newY].Fox == null && grid[newX, newY].Rabbit == null)
        //        {
        //            grid[newX, newY].Fox = this;
        //            grid[x, y].Fox = null; // Leave current cell
        //        }
        //    }
        //}

        //public void EatRabbit()
        //{
        //    energy += 10; // Increase energy when eating a rabbit
        //}

        //public bool Survive()
        //{
        //    // Check survival based on age or other conditions
        //    Age++;
        //    if (Age > MaxAge)
        //    {
        //        IsAlive = false; // Fox dies if age exceeds max age
        //    }
        //    return IsAlive;
        //}

        //public Fox Reproduce()
        //{
        //    // Example reproduction logic
        //    if (IsAlive && Age > 2) // Can reproduce if older than 2 years
        //    {
        //        return new Fox(); // Return a new fox
        //    }
        //    return null; // No reproduction
        //}
    }
}
