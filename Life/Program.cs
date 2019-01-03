using System;
using System.Collections.Generic;
using System.Threading;

namespace Life
{
    class Program
    {
        static void SimulationMain(string[] args)
        {
            Game game = Game.GetInstance();
            int prev = 0;
            Universe old = game.GetUniverse();
            HashSet<string> history = new HashSet<string>();
            history.Add(old.ToString());
            Console.Write(old);

            while (prev != history.Count)
            {
                Universe universe = game.GetUniverse();
                if (universe != old)
                {
                    old = universe;
                    prev = history.Count;
                    history.Add(old.ToString());
                    Console.WriteLine(old);
                }
                Thread.Sleep(250);
            }
            Console.WriteLine("State no longer changing");
        }
    }
}
