using System;
using System.Text;

namespace Life{
    public class Universe{
        public const int SIZE = 10;
        public bool[,] Grid {get;}

        private Random rng;

        public Universe(){
            Grid = new bool[SIZE+2, SIZE+2];
            rng = new Random();
        }

        // This method randomizes the board.
        public void Randomize(){
            for(int i = 1; i < SIZE + 1; i++){
                for(int j = 1; j < SIZE+1; j++){
                    Grid[i,j] = rng.NextDouble() >= .5;
                }
            }
        }

        // This method converts a string into a board.
        //Can be used to initialize the board into a specific state.
        public static Universe FromString(string s){
            if(s.Length != Universe.SIZE*Universe.SIZE) return null;
            int i = 0;
            Universe u = new Universe();
            foreach(char c in s){
                if(c == 'X')
                    u.Grid[i / Universe.SIZE, i % Universe.SIZE] = true;
                i++;
            }
            return u;
        }

        // This method converts the board into a string with alphabetical columns and rows.
        public override string ToString(){
            string o = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder builder = new StringBuilder("\t  ");
            for(int i = 0; i < SIZE; i++){
                builder.Append(o[i]);
            }
            builder.AppendLine();
            for(int i = 1; i < SIZE + 1; i++){
                builder.AppendFormat("\t{0}:", o[i-1]);
                for(int j = 1; j < SIZE + 1; j++){
                    if(Grid[i,j]) builder.Append("X");
                    else builder.Append(" ");
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}