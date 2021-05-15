using System;
using System.Collections.Generic;
using board;
namespace chess
{
    class ChessMatch
    {
        public Board board { get; private set; }

        public bool check { get; private set; }

        public bool ended { get; private set; }

        public Color currentPlayer { get; private set; }

        public int turn { get; private set; }

        private HashSet<Piece> pieces;

        private HashSet<Piece> capturedPieces;

        public ChessMatch()
        {
            board = new Board(8, 8);
            check = false;
            ended = false;
            currentPlayer = Color.White;
            turn = 1;
            pieces = new HashSet<Piece>();
            capturedPieces = new HashSet<Piece>();
            PlacePieces();
        }

        public Piece MovePiece(Position origin, Position destination)
        {
            Piece movingPiece = board.PickUpPiece(origin);
            Piece capturedPiece = board.PickUpPiece(destination);
            board.PlacePiece(movingPiece, destination);
            if (capturedPiece != null)
                capturedPieces.Add(capturedPiece);
            return capturedPiece;
        }

        private void UndoMovePiece(Position origin, Position destination, Piece capturedPiece)
        {
            Piece p = board.PickUpPiece(destination);
            p.DecreaseMovementAmount();
            if (capturedPiece != null) {
                board.PlacePiece(capturedPiece, destination);
                capturedPieces.Remove(capturedPiece);
            }
            board.PlacePiece(p, origin);
        }

        public void PerformMove(Position origin, Position destination)
        {
            Piece capturedPiece = MovePiece(origin, destination);

            if (IsInCheck(currentPlayer))
            {
                UndoMovePiece(origin, destination, capturedPiece);
                throw new BoardException("Você não pode se colocar em xeque!");
            }

            if (IsInCheck(Adversary(currentPlayer)))
                check = true;
            else
                check = false;

            if (IsCheckmate(Adversary(currentPlayer)))
                ended = true;
            else
            {
                turn++;
                ChangePlayer();
            }
        }

        public void ValidateOriginPosition(Position pos)
        {
            if (board.Piece(pos) == null)
            {
                throw new BoardException("Não existe peça na posição de origem escolhida!");
            }

            if (currentPlayer != board.Piece(pos).color)
            {
                throw new BoardException("A peça de origem escolhida não é sua!");
            }

            if (!board.Piece(pos).PossibleMovementsExists())
            {
                throw new BoardException("Não há movimentos possiveis para a peça de origem escolhida!");
            }
        }

        public void ValidateDestinationPosition(Position origin, Position destination)
        {
            if (!board.Piece(origin).MightMoveTo(destination))
            {
                throw new BoardException("Posição de destino inválida!");
            }
        }

        private void ChangePlayer()
        {
            if (currentPlayer == Color.White)
                currentPlayer = Color.Black;           
            else
                currentPlayer = Color.White;
        }

        public string ColorName(Color color)
        {
            if (color == Color.White)
                return "Brancas";
            else
                return "Pretas";
        }

        public HashSet<Piece> CapturedPieces(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece p in capturedPieces)
            {
                if (p.color == color)
                {
                    aux.Add(p);
                }
            }
            return aux;
        }

        public HashSet<Piece> InGamePieces(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece p in pieces)
            {
                if (p.color == color)
                {
                    aux.Add(p);
                }
            }
            aux.ExceptWith(CapturedPieces(color));
            return aux;
        }

        private Color Adversary(Color color)
        {
            if (color == Color.White)
                return Color.Black;
            else
                return Color.White;
        }

        private Piece King(Color color)
        {
            foreach (Piece p in InGamePieces(color))
                if (p is King)
                    return p;
            return null;
        }

        public bool IsInCheck(Color color)
        {
            Piece king = King(color);
            if (king == null)
                throw new BoardException("Não há rei de peças " + ColorName(color) + " no tabuleiro. Partida inválida.");

            foreach (Piece p in InGamePieces(Adversary(color)))
            {
                bool[,] matrix = p.PossibleMovements();
                if (matrix[king.position.line, king.position.column])
                    return true;              
            }
            return false;
        }

        public bool IsCheckmate(Color color)
        {
            if (!IsInCheck(color))
                return false;

            foreach (Piece p in InGamePieces(color))
            {
                bool[,] matrix = p.PossibleMovements();
                for (int i = 0; i < board.lines; i++)
                {
                    for (int j = 0; j < board.columns; j++)
                    {
                        if (matrix[i, j])
                        {
                            Position origin = p.position;
                            Position destination = new Position(i, j);
                            Piece capturedPiece = MovePiece(origin, destination);
                            bool isInCheck = IsInCheck(color);
                            UndoMovePiece(origin, destination, capturedPiece);
                            if (!isInCheck)
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        public void PlaceNewPiece(Piece piece, char column, int line)
        {
            board.PlacePiece(piece, new ChessPosition(column, line).ToPosition());
            pieces.Add(piece);
        }

        public void PlacePieces()
        {
            PlaceNewPiece(new King(Color.Black, board), 'd', 8);
            PlaceNewPiece(new Rook(Color.Black, board), 'c', 8);
            PlaceNewPiece(new Rook(Color.Black, board), 'c', 7);
            PlaceNewPiece(new Rook(Color.Black, board), 'd', 7);
            PlaceNewPiece(new Rook(Color.Black, board), 'e', 7);
            PlaceNewPiece(new Rook(Color.Black, board), 'e', 8);

            PlaceNewPiece(new King(Color.White, board), 'd', 1);
            PlaceNewPiece(new Rook(Color.White, board), 'c', 1);
            PlaceNewPiece(new Rook(Color.White, board), 'c', 2);
            PlaceNewPiece(new Rook(Color.White, board), 'd', 2);
            PlaceNewPiece(new Rook(Color.White, board), 'e', 2);
            PlaceNewPiece(new Rook(Color.White, board), 'e', 1);
        }
    }
}
