using System;
using board;
using chess;

namespace chess_console
{
    class Program
    {
        static void Main(string[] args)
        {
            Board b = new Board(8,8);

            ChessPosition pos1 = new ChessPosition('a', 1); // 7, 0
            ChessPosition pos2 = new ChessPosition('f', 3); // 5, 5


            Console.WriteLine(pos1.ToPosition().ToString());
            Console.WriteLine(pos2.ToPosition().ToString());

            //Console.WriteLine(pos);

            Console.ReadLine();
        }
    }
}
