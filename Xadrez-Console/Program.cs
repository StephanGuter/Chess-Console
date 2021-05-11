using System;
using board;
using chess;

namespace chess_console
{
    class Program
    {
        static void Main(string[] args)
        {
            Board b = new Board(8, 8);

            b.PlacePiece(new King(Color.Black, b), new Position(1, 3));
            b.PlacePiece(new King(Color.White, b), new Position(3, 5));
            b.PlacePiece(new Queen(Color.White, b), new Position(5, 6));

            Display.printBoard(b);
        }
    }
}
