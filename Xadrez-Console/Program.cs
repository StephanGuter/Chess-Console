using System;
using board;

namespace chess_console
{
    class Program
    {
        static void Main(string[] args)
        {
            Board B = new Board(8, 8);

            Display.printBoard(B);
        }
    }
}
