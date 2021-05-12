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

            b.PlacePiece(new King(Color.Black, b), new Position(1, 1));
            b.PlacePiece(new Queen(Color.White, b), new Position(5, 7));

            Display.PrintBoard(b);

            Console.ReadLine();
        }
    }
}
