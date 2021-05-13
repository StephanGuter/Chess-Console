using System;
using board;
using chess;

namespace chess_console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ChessMatch match = new ChessMatch();

                while (!match.ended)
                {
                    Console.Clear();
                    Display.PrintBoard(match.board);

                    Console.Write("Origem: ");
                    Position origin = Display.ReadChessPosition().ToPosition();

                    bool[,] possiblePositions = match.board.Piece(origin).PossibleMovements();

                    Console.Clear();
                    Display.PrintBoard(match.board, possiblePositions);

                    Console.Write("Destino: ");
                    Position destination = Display.ReadChessPosition().ToPosition();

                    match.MovePiece(origin, destination);
                }
            }
            catch (BoardException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
