using System;
using System.Threading;

namespace Life{
    public class Game{

        private static Game instance;
        
        // Lock variable for initialization of this Singleton,
        // in case multpile threads try to initialize Game at the same time.
        private static readonly object l = new object();

        private Universe universe;

        public Universe GetUniverse(){
            return universe;
        }

        // This is the subthread that will tick along, updating the universe.
        private Thread subthread;
        private Game(){

            // Initial state for a repeating pattern.  This requries a larger than 10x10 universe.
            string initial =
            "                    "+
            "                    "+
            "                    "+
            "                    "+
            "      XXX   XXX     "+
            "                    "+
            "    X    X X    X   "+
            "    X    X X    X   "+
            "    X    X X    X   "+
            "      XXXOOOXXX     "+
            "         OOO        "+
            "      XXXOOOXXX     "+
            "    X    X X    X   "+
            "    X    X X    X   "+
            "    X    X X    X   "+
            "                    "+
            "      XXX   XXX     "+
            "                    "+
            "                    "+
            "                    ";
            universe = null; //Universe.FromString(initial);
            if(universe == null){
                Console.Error.Write("FromString failed");
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

        private void GameLoop(){
            while(true){
                Tick();
                Thread.Sleep(1000); // 1 Second
            }
        }

        private void Tick(){
            Universe next = new Universe();
            for(int i = 1; i < Universe.SIZE + 1; i++){
                for(int j = 1; j < Universe.SIZE+1; j++){
                    int livingNeighbors = 0;
                    //TODO: Clean this up with, perhaps, another set of for-loops
                    if(universe.Grid[i-1,j-1]) livingNeighbors++; // top left
                    if(universe.Grid[i,j-1]) livingNeighbors++; // top middle
                    if(universe.Grid[i+1,j-1]) livingNeighbors++; // top right
                    if(universe.Grid[i-1,j]) livingNeighbors++; // left
                    if(universe.Grid[i+1,j]) livingNeighbors++; // right
                    if(universe.Grid[i-1,j+1]) livingNeighbors++; // bottom left
                    if(universe.Grid[i,j+1]) livingNeighbors++; // bottom middle
                    if(universe.Grid[i+1,j+1]) livingNeighbors++; // bottom right

                    if(universe.Grid[i,j]){ // current cell is alive
                        if(livingNeighbors == 2 || livingNeighbors == 3){
                            next.Grid[i,j] = true;
                        }
                    } else { // current cell is dead
                        if(livingNeighbors == 3)
                            next.Grid[i,j] = true;
                    }
                }
            }
            universe = next;
        }

        public static Game GetInstance(){
            if(instance == null){
                lock(l){
                    if(instance == null)
                        instance = new Game();
                }
            }

            return instance;
        }
    }
}