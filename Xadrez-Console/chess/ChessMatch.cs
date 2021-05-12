using System;
using board;
namespace chess
{
    class ChessMatch
    {
        public Board board { get; private set; }
        public int _turn;
        private Color _currentPlayer;
        public bool ended { get; private set; }

        public ChessMatch()
        {
            board = new Board(8, 8);
            _turn = 1;
            _currentPlayer = Color.White;
            ended = false;
            PlacePieces();
        }

        public void MovePiece(Position origin, Position destination)
        {
            Piece movingPiece = board.PickUpPiece(origin);
            Piece capturedPiece = board.PickUpPiece(destination);
            board.PlacePiece(movingPiece, destination);
        }

        public void PlacePieces()
        {
            board.PlacePiece(new King(Color.Black, board), new ChessPosition('d', 8).ToPosition());
            board.PlacePiece(new Queen(Color.Black, board), new ChessPosition('c', 8).ToPosition());
            board.PlacePiece(new Queen(Color.Black, board), new ChessPosition('c', 7).ToPosition());
            board.PlacePiece(new Queen(Color.Black, board), new ChessPosition('d', 7).ToPosition());
            board.PlacePiece(new Queen(Color.Black, board), new ChessPosition('e', 7).ToPosition());
            board.PlacePiece(new Queen(Color.Black, board), new ChessPosition('e', 8).ToPosition());

            board.PlacePiece(new King(Color.White, board), new ChessPosition('d', 1).ToPosition());
            board.PlacePiece(new Queen(Color.White, board), new ChessPosition('c', 1).ToPosition());
            board.PlacePiece(new Queen(Color.White, board), new ChessPosition('c', 2).ToPosition());
            board.PlacePiece(new Queen(Color.White, board), new ChessPosition('d', 2).ToPosition());
            board.PlacePiece(new Queen(Color.White, board), new ChessPosition('e', 2).ToPosition());
            board.PlacePiece(new Queen(Color.White, board), new ChessPosition('e', 1).ToPosition());
        }
    }
}
