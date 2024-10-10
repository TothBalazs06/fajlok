using System;
using EntitesLib;

namespace PTPB_FoxAndRabbits
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Üdvözöllek a rókák és nyulak szimulációban!");
            Console.Write("Add meg a rács szélességét: ");
            int width = int.Parse(Console.ReadLine());
            Console.Write("Add meg a rács magasságát: ");
            int height = int.Parse(Console.ReadLine());

            SimulationEngine engine = new SimulationEngine(width, height);
            engine.AddRabbit(1, 1);
            engine.AddRabbit(1, 2);
            engine.AddFox(2, 2);
            engine.AddFox(2, 3);
            engine.AddFox(3, 4);
            Console.WriteLine("Kezdődik a szimuláció! Nyomj Entert a következő körhöz.");
            while (true)
            {
                Console.ReadLine();
                engine.NextTurn();
                engine.DisplayGrid();
            }
        }
    }
}
