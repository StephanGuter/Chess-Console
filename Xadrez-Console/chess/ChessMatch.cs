using System;
using board;
namespace chess
{
    class ChessMatch
    {
        public Board board { get; private set; }
        public int turn { get; private set; }
        public bool ended { get; private set; }
        public Color currentPlayer { get; private set; }

        public ChessMatch()
        {
            board = new Board(8, 8);
            turn = 1;
            currentPlayer = Color.White;
            ended = false;
            PlacePieces();
        }

        public void MovePiece(Position origin, Position destination)
        {
            Piece movingPiece = board.PickUpPiece(origin);
            Piece capturedPiece = board.PickUpPiece(destination);
            board.PlacePiece(movingPiece, destination);
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

        public void PlacePieces()
        {
            board.PlacePiece(new King(Color.Black, board), new ChessPosition('d', 8).ToPosition());
            board.PlacePiece(new Rook(Color.Black, board), new ChessPosition('c', 8).ToPosition());
            board.PlacePiece(new Rook(Color.Black, board), new ChessPosition('c', 7).ToPosition());
            board.PlacePiece(new Rook(Color.Black, board), new ChessPosition('d', 7).ToPosition());
            board.PlacePiece(new Rook(Color.Black, board), new ChessPosition('e', 7).ToPosition());
            board.PlacePiece(new Rook(Color.Black, board), new ChessPosition('e', 8).ToPosition());

            board.PlacePiece(new King(Color.White, board), new ChessPosition('d', 1).ToPosition());
            board.PlacePiece(new Rook(Color.White, board), new ChessPosition('c', 1).ToPosition());
            board.PlacePiece(new Rook(Color.White, board), new ChessPosition('c', 2).ToPosition());
            board.PlacePiece(new Rook(Color.White, board), new ChessPosition('d', 2).ToPosition());
            board.PlacePiece(new Rook(Color.White, board), new ChessPosition('e', 2).ToPosition());
            board.PlacePiece(new Rook(Color.White, board), new ChessPosition('e', 1).ToPosition());
        }
    }
}
