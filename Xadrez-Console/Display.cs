using board;
using chess;
using System;
using System.Collections.Generic;

namespace chess_console
{
    class Display
    {
        public static void PrintMatch(ChessMatch match)
        {
            Display.PrintBoard(match.board);
            Console.WriteLine();
            PrintCapturedPieces(match);
            Console.WriteLine();
            Console.WriteLine("Turno: " + match.turn);

            string colorName;
            if (match.currentPlayer == Color.White)
                colorName = "Brancas";
            else
                colorName = "Pretas";
            
            Console.WriteLine("Aguardando jogada: " + colorName);
        }

        public static void PrintCapturedPieces(ChessMatch match)
        {
            Console.WriteLine("Peças capturadas:");

            Console.Write("Brancas: ");
            PrintSet(match.CapturedPieces(Color.White));

            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Pretas:  ");
            PrintSet(match.CapturedPieces(Color.Black));
            Console.ForegroundColor = aux;
        }

        public static void PrintSet(HashSet<Piece> set)
        {
            Console.Write("[ ");
            foreach (Piece p in set)
            {
                Console.Write(p + " ");
            }
            Console.WriteLine(" ]");
        }

        public static void PrintBoard(Board board)
        {
            for (int i = 0; i < board.lines; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < board.columns; j++)
                {
                    PrintPiece(board.Piece(i, j));
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h ");
        }

        public static void PrintBoard(Board board, bool[,] possiblePositions)
        {
            ConsoleColor orbg = Console.BackgroundColor;
            ConsoleColor ppbg = ConsoleColor.DarkGray;

            for (int i = 0; i < board.lines; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < board.columns; j++)
                {
                    if (possiblePositions[i, j])
                    {
                        Console.BackgroundColor = ppbg;
                    }
                    else
                    {
                        Console.BackgroundColor = orbg;
                    }
                    PrintPiece(board.Piece(i, j));
                    Console.BackgroundColor = orbg;
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h ");
        }

        public static void PrintPiece(Piece piece)
        {
            if (piece == null)
            {
                Console.Write("- ");
            }
            else
            {
                if (piece.color == Color.White)
                {
                    Console.Write(piece);
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(piece);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }

        public static ChessPosition ReadChessPosition()
        {
            string pos = Console.ReadLine();
            char column = pos[0];
            int line = int.Parse(pos[1] + "");
            return new ChessPosition(column, line);
        }
    }
}
