using System;

namespace EntitesLib
{
    public class Cell
    {
        public GrassState Grass { get; set; }
        public Rabbit Rabbit { get; set; }
        public Fox Fox { get; set; }

        public Cell()
        {
            Grass = GrassState.Young; // Initial state of grass
            Rabbit = null; // initially no rabbit
            Fox = null; // initially no fox
        }

        public void UpdateGrass()
        {
            if (Grass == GrassState.Young)
            {
                Grass = GrassState.Mature; // Grows to mature if young
            }
            else if (Grass == GrassState.Mature)
            {
                Grass = GrassState.Old; // Grows to old if mature
            }
            else if (Grass == GrassState.Old)
            {
                Grass = GrassState.Young; // Resets to young after being old
            }
        }
    }
}
