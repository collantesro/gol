using System;
using System.Threading;

namespace Life
{

    public delegate void BoardChangedCallback(string message);
    public class Game
    {

        // This delegate will be called at the end of Tick()
        public BoardChangedCallback boardChanged { get; set; }
        private static Game instance;

        // Lock variable for initialization of this Singleton,
        // in case multpile threads try to initialize Game at the same time.
        private static readonly object init_lock = new object();

        // Lock variable used for changing the board.
        private static readonly object runtime_lock = new object();

        private int _msToSleep = 1_000;
        public int MsToSleep
        {
            get
            {
                return _msToSleep;
            }
            set
            {
                // Min of 100ms, Max of 10,000 ms
                if (value < 100)
                    _msToSleep = 100;
                else if (value > 10_000)
                    _msToSleep = 10_000;
                else
                    _msToSleep = value;
            }
        }

        private Universe universe;

        public Universe GetUniverse()
        {
            return universe;
        }

        // This is the subthread that will tick along, updating the universe.
        private Thread subthread;
        private Game()
        {

            // Initial state for a repeating pattern.  This requries a larger than 10x10 universe.
            //string initial =
            //"                    " +
            //"                    " +
            //"                    " +
            //"                    " +
            //"      XXX   XXX     " +
            //"                    " +
            //"    X    X X    X   " +
            //"    X    X X    X   " +
            //"    X    X X    X   " +
            //"      XXXOOOXXX     " +
            //"         OOO        " +
            //"      XXXOOOXXX     " +
            //"    X    X X    X   " +
            //"    X    X X    X   " +
            //"    X    X X    X   " +
            //"                    " +
            //"      XXX   XXX     " +
            //"                    " +
            //"                    " +
            //"                    ";
            //universe = null; //Universe.FromString(initial);
            //if(universe == null) {
            //    Console.Error.Write("FromString failed");
            //    universe = new Universe();
            //    universe.Randomize();
            //}

            lock (runtime_lock)
            {
                universe = new Universe();
                universe.Randomize();
            }

            subthread = new Thread(GameLoop);
            // Background thread allows it to be killed when main thread has finished.
            // It won't hold up the shutdown of the program.
            subthread.IsBackground = true;
            subthread.Name = "GameLoop";
            subthread.Start();
        }

        private void GameLoop()
        {
            while (true)
            {
                Tick();
                Thread.Sleep(MsToSleep); // Default is 1 second.  May be modified
            }
        }

        private void Tick()
        {
            lock (runtime_lock)
            {
                Universe next = new Universe();
                for (int i = 1; i < Universe.SIZE + 1; i++)
                {
                    for (int j = 1; j < Universe.SIZE + 1; j++)
                    {
                        int livingNeighbors = 0;

                        // These loops will go from the Top Left (i - 1, j - 1)
                        // to Bottom Right (i + 1, j + 1).  It skips the actual square
                        for (int x = -1; x <= 1; x++)
                        {
                            for (int y = -1; y <= 1; y++)
                            {
                                // If we're looking at ourself, skip.  Just want neighbors.
                                if (x == 0 && y == 0) continue;
                                if (universe.Grid[i + x, j + y])
                                {
                                    livingNeighbors++;
                                }
                            }
                        }

                        if (universe.Grid[i, j])
                        { // current cell is alive
                            if (livingNeighbors == 2 || livingNeighbors == 3)
                            {
                                next.Grid[i, j] = true;
                            }
                        }
                        else
                        { // current cell is dead
                            if (livingNeighbors == 3)
                                next.Grid[i, j] = true;
                        }
                    }
                }
                universe = next;
            }

            boardChanged?.Invoke(universe.ToString());
        }

        public static Game GetInstance()
        {
            if (instance == null)
            {
                lock (init_lock)
                {
                    if (instance == null)
                        instance = new Game();
                }
            }

            return instance;
        }

        public void Randomize()
        {
            lock (runtime_lock)
            {
                universe.Randomize();
            }
            boardChanged?.Invoke(universe.ToString());
        }

        public void ToggleCell(int x, int y)
        {
            if (x >= Universe.SIZE || x < 0 || y >= Universe.SIZE || y < 0)
                return;
            else
            {
                lock (runtime_lock)
                {
                    universe.ToggleCell(x, y);
                }
                boardChanged?.Invoke(universe.ToString());
            }
        }
    }
}
