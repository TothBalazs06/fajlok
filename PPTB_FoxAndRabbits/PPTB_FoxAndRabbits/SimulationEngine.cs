using EntitesLib;
using System;

namespace PTPB_FoxAndRabbits
{
    public class SimulationEngine
    {
        private readonly Cell[,] grid;
        private readonly int width;
        private readonly int height;

        public SimulationEngine(int width, int height)
        {
            this.width = width;
            this.height = height;
            grid = new Cell[width, height];

            // A grid betöltése
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    grid[i, j] = new Cell
                    {
                        Grass = GrassState.Young, // a fű alapállapota
                        Rabbit = null,
                        Fox = null
                    };
                }
            }
            grid[0, 0].Rabbit = new Rabbit(); // A (0, 0)-rá új nyúl betöltése

        }

        //Ez a metódus hozzáad egy nyulat X,Y koordinátára
        public void AddRabbit(int x, int y)
        {
            if (IsWithinBounds(x, y) && grid[x, y].Rabbit == null)
            {
                grid[x, y].Rabbit = new Rabbit();
            }
        }

        //Ez a metódus hozzáad egy rókát X,Y koordinátára
        public void AddFox(int x, int y)
        {
            if (IsWithinBounds(x, y) && grid[x, y].Fox == null)
            {
                grid[x, y].Fox = new Fox();
            }
        }

        //Új kör
        public void NextTurn()
        {
            try
            {
                //Mozgás és lépések
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        var cell = grid[i, j];

                        // Bebugging kiírás
                        //Console.WriteLine($"Processing cell ({i}, {j}): Grass = {cell.Grass}, Rabbit = {cell.Rabbit}, Fox = {cell.Fox}");

                        // Nyúl logikája
                        if (cell.Rabbit != null) // Csak akkor történjen bármi ha a cellában van egy nyúl
                        {
                            // A nyúl mozgatása
                            cell.Rabbit.Move(grid, i, j);

                            // Nyúl evési mechanikája
                            Console.WriteLine($"Nyúl a ({i}, {j}) koordinátán enni próbál: '{cell.Grass}' füvet");
                            if ((cell.Grass == GrassState.Young || cell.Grass == GrassState.Mature || cell.Grass == GrassState.Old) && cell.Rabbit != null) // Csak akkor eszik a nyúl ha a grassState young
                            {
                                cell.Rabbit.Eat(cell.Grass); // Evés
                                cell.Grass = GrassState.Empty; // Az evés után megváltoztatjuk a füvet
                            }
                            else
                            {
                                Console.WriteLine($"Nyúl a ({i}, {j}) koordinátán nem tudott enni füvet, mivel nincs mit ennie.");
                            }

                            // Meghívjuk a Survive() metódust mely megnézi hogy él-e a nyúl
                            if (cell.Rabbit != null && !cell.Rabbit.Survive())
                            {
                                Console.WriteLine($"Nyúl a ({i}, {j}) koordinátán elpusztult.");
                                cell.Rabbit = null; // A nyúlat töröljük
                            }
                            else if (cell.Rabbit != null) // Ha viszont még él akkor megpróbál reprodukálódni 
                            {
                                Rabbit newRabbit = cell.Rabbit.Reproduce();
                                if (newRabbit != null)
                                {
                                    // Megpróbáljuk lehelyezni egy közeli cellába
                                    AddRabbitToAdjacentCell(i, j);
                                }
                            }

                            cell.UpdateGrass(); // Frissítjük a füvet a celllán
                        }

                        if (cell.Fox != null) // Csak akkor történik bármi ha van róka a cellában
                        {
                            // Róka mozgatása
                            cell.Fox.Move(grid, i, j);

                            // Az evéshez ellenőárizzük hogy van-e nyúl a közelben
                            if (TryFindRabbitInAdjacentCells(i, j, out int rabbitX, out int rabbitY) && cell.Fox != null)
                            {
                                cell.Fox.EatRabbit(); // A róka megeszi a nyulat
                                Console.WriteLine($"Róka a ({i}, {j}) koordinátán megevett egy nyulat ({rabbitX}, {rabbitY}).");
                                grid[rabbitX, rabbitY].Rabbit = null; //Nyulat töröljük ha megették
                            }
                            else
                            {
                                Console.WriteLine($"Róka a ({i}, {j}) koordinátán nem talált nyulat az evéshez.");
                            }

                            // Meghívjuk a Survive() metódust mely megnézi hogy él-e a róka
                            if (cell.Fox != null && !cell.Fox.Survive())
                            {
                                Console.WriteLine($"Róka a ({i}, {j}) koordinátán elpusztult");
                                cell.Fox = null; // A rókát töröljük ha már nem él
                            }
                            else if (cell.Fox != null) // Ha viszont még él akkor megpróbál reprodukálódni 
                            {
                                Fox newFox = cell.Fox.Reproduce();
                                if (newFox != null)
                                {
                                    // Megpróbáljuk lehelyezni egy közeli cellába
                                    AddFoxToAdjacentCell(i, j);
                                }
                            }
                        }

                        // Frissítsjük a füvet az adott cellán
                        cell.UpdateGrass();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }
        }

        private bool TryFindRabbitInAdjacentCells(int x, int y, out int rabbitX, out int rabbitY)
        {
            // Megnézzük hogy van-e a közelben nyúl (fel, le, balra, jobbra)
            int[,] directions = new int[,] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
            for (int d = 0; d < directions.GetLength(0); d++)
            {
                int newX = x + directions[d, 0];
                int newY = y + directions[d, 1];

                if (IsWithinBounds(newX, newY) && grid[newX, newY].Rabbit != null)
                {
                    rabbitX = newX;
                    rabbitY = newY;
                    return true; // Találtunk nyulat
                }
            }

            rabbitX = -1;
            rabbitY = -1;
            return false; // Nem találtunk
        }


        private void AddFoxToAdjacentCell(int x, int y)
        {
            // Megnézzük hogy le tudunk-e rakni rókát (fel, le, balra, jobbra)
            int[,] directions = new int[,] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
            for (int d = 0; d < directions.GetLength(0); d++)
            {
                int newX = x + directions[d, 0];
                int newY = y + directions[d, 1];

                if (IsWithinBounds(newX, newY) && grid[newX, newY].Fox == null)
                {
                    grid[newX, newY].Fox = new Fox();
                    Console.WriteLine($"Új rójka a ({newX}, {newY}) koordinátán");
                    return;
                }
            }
            //Console.WriteLine("Nincs hely új rókának.");
        }
        private void AddRabbitToAdjacentCell(int x, int y)
        {
            // Ugyanaz mint a rókánál
            int[,] directions = new int[,] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
            for (int d = 0; d < directions.GetLength(0); d++)
            {
                int newX = x + directions[d, 0];
                int newY = y + directions[d, 1];

                if (IsWithinBounds(newX, newY) && grid[newX, newY].Rabbit == null)
                {
                    grid[newX, newY].Rabbit = new Rabbit();
                    Console.WriteLine($"Új nyúl született a ({newX}, {newY}) koordinátán.");
                    return;
                }
            }
            //Console.WriteLine("Nem volt hely ");
        }
        // a nyúl és a rókák hozzáadásához szükséges, ugyanis ha a felh. megadott szamok kisebbek mint az alap akkor hibas lenne a kod
        private bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        // a grid megjelenítése
        public void DisplayGrid()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var cell = grid[i, j];
                    if (cell.Rabbit != null)
                    {
                        Console.Write("R ");
                    }
                    else if (cell.Fox != null)
                    {
                        Console.Write("F ");
                    }
                    else
                    {
                        Console.Write(". ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
