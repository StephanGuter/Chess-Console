using System;
using System.Collections.Generic;
using board;
namespace chess
{
    class ChessMatch
    {
        public Board board { get; private set; }
        public int turn { get; private set; }
        public bool ended { get; private set; }
        public Color currentPlayer { get; private set; }
        private HashSet<Piece> pieces;
        private HashSet<Piece> capturedPieces;

        public ChessMatch()
        {
            board = new Board(8, 8);
            turn = 1;
            currentPlayer = Color.White;
            ended = false;
            pieces = new HashSet<Piece>();
            capturedPieces = new HashSet<Piece>();
            PlacePieces();
        }

        public void MovePiece(Position origin, Position destination)
        {
            Piece movingPiece = board.PickUpPiece(origin);
            Piece capturedPiece = board.PickUpPiece(destination);
            board.PlacePiece(movingPiece, destination);
            if (capturedPiece != null)
            {
                capturedPieces.Add(capturedPiece);
            }
        }

        public void PerformMove(Position origin, Position destination)
        {
            MovePiece(origin, destination);
            turn++;
            ChangePlayer();
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
