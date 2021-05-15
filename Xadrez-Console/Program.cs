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
                    try
                    {
                        Console.Clear();
                        Display.PrintMatch(match);

                        Console.WriteLine();
                        Console.Write("Origem: ");
                        Position origin = Display.ReadChessPosition().ToPosition();
                        match.ValidateOriginPosition(origin);

                        Console.Clear();
                        bool[,] possiblePositions = match.board.Piece(origin).PossibleMovements();
                        Display.PrintBoard(match.board, possiblePositions);

                        Console.WriteLine();
                        Console.Write("Destino: ");
                        Position destination = Display.ReadChessPosition().ToPosition();
                        match.ValidateDestinationPosition(origin, destination);

                        match.PerformMove(origin, destination);
                    }
                    catch (BoardException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }
                }

                Console.Clear();
                Display.PrintMatch(match);
            }
            catch (BoardException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
