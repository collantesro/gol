using System;
using System.Diagnostics;
using System.Text;

namespace Life
{
    /// <summary>
    /// This class represents the grid board for the simulation.
    /// It's a square grid of SIZE length and width.
    /// </summary>
    public class Universe
    {
        public const int SIZE = 10;
        public bool[,] Grid { get; }

        private Random rng;

        public Universe()
        {
            Grid = new bool[SIZE + 2, SIZE + 2];
            rng = new Random();
        }

        /// <summary>
        /// Randomizes the current board.
        /// </summary>
        public void Randomize()
        {
            for (int i = 1; i < SIZE + 1; i++)
            {
                for (int j = 1; j < SIZE + 1; j++)
                {
                    Grid[i, j] = rng.NextDouble() >= .5;
                }
            }
        }

        /// <summary>
        /// This static method converts a flat string into a Universe board.
        /// </summary>
        /// <returns>Universe object from the flat string.</returns>
        /// <param name="s">Flat string representation of a board</param>
        public static Universe FromString(string s)
        {
            if (s.Length != Universe.SIZE * Universe.SIZE) return null;
            int i = 0;
            Universe u = new Universe();
            foreach (char c in s)
            {
                if (c == 'X')
                    u.Grid[i / Universe.SIZE, i % Universe.SIZE] = true;
                i++;
            }
            return u;
        }

        /// <summary>
        /// This method converts the current board into a flat string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(Universe.SIZE * Universe.SIZE);
            for (int i = 1; i < SIZE + 1; i++)
            {
                for (int j = 1; j < SIZE + 1; j++)
                {
                    if (Grid[i, j]) builder.Append("X");
                    else builder.Append("O");
                }
            }
            Debug.Assert(builder.Length == Universe.SIZE * Universe.SIZE);
            return builder.ToString();
        }

        public void ToggleCell(int x, int y)
        {
            // Since we have a border of cells around, add 1 to both x and y.
            Grid[x + 1, y + 1] = !Grid[x + 1, y + 1];
        }
    }
}
