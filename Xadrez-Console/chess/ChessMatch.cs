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

        public Piece enPassantVulnerable { get; private set; }

        private HashSet<Piece> pieces;

        private HashSet<Piece> capturedPieces;

        public ChessMatch()
        {
            board = new Board(8, 8);
            check = false;
            ended = false;
            currentPlayer = Color.White;
            turn = 1;
            enPassantVulnerable = null;
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

            // # Special move: castling
            if (movingPiece is King)
            {
                // Castling king side
                if (destination.column == origin.column + 2)
                {
                    Position rookOrigin = new Position(origin.line, origin.column + 3);
                    Position rookDestination = new Position(origin.line, origin.column + 1);
                    Piece rook = board.PickUpPiece(rookOrigin);
                    board.PlacePiece(rook, rookDestination);
                }

                // Castling queen side
                if (destination.column == origin.column - 2)
                {
                    Position rookOrigin = new Position(origin.line, origin.column - 4);
                    Position rookDestination = new Position(origin.line, origin.column - 1);
                    Piece rook = board.PickUpPiece(rookOrigin);
                    board.PlacePiece(rook, rookDestination);
                }
            }

            // # Special move: en passant
            if (movingPiece is Pawn)
            {
                if (origin.column != destination.column && capturedPiece == null)
                {
                    Position pawnPos;
                    if (movingPiece.color == Color.White)
                        pawnPos = new Position(destination.line + 1, destination.column);
                    else
                        pawnPos = new Position(destination.line - 1, destination.column);
                    capturedPiece = board.PickUpPiece(pawnPos);
                    capturedPieces.Add(capturedPiece);
                }
            }

            return capturedPiece;
        }

        private void UndoMovePiece(Position origin, Position destination, Piece capturedPiece)
        {
            Piece movingPiece = board.PickUpPiece(destination);
            movingPiece.DecreaseMovementAmount();
            if (capturedPiece != null)
            {
                capturedPiece.DecreaseMovementAmount();
                board.PlacePiece(capturedPiece, destination);
                capturedPiece.DecreaseMovementAmount();
                capturedPieces.Remove(capturedPiece);
            }
            board.PlacePiece(movingPiece, origin);
            movingPiece.DecreaseMovementAmount();

            // # Special move: castling
            if (movingPiece is King)
            {
                // Castling king side
                if (destination.column == origin.column + 2)
                {
                    Position rookOrigin = new Position(origin.line, origin.column + 3);
                    Position rookDestination = new Position(origin.line, origin.column + 1);
                    Piece rook = board.PickUpPiece(rookDestination);
                    rook.DecreaseMovementAmount();
                    board.PlacePiece(rook, rookOrigin);
                    rook.DecreaseMovementAmount();
                }

                // Castling queen side
                if (destination.column == origin.column - 2)
                {
                    Position rookOrigin = new Position(origin.line, origin.column - 4);
                    Position rookDestination = new Position(origin.line, origin.column - 1);
                    Piece rook = board.PickUpPiece(rookDestination);
                    rook.DecreaseMovementAmount();
                    board.PlacePiece(rook, rookOrigin);
                    rook.DecreaseMovementAmount();
                }
            }

            // # Special move: en passant
            if (movingPiece is Pawn)
            {
                if (origin.column != destination.column && capturedPiece == enPassantVulnerable)
                {
                    Piece pawn = board.PickUpPiece(destination);
                    Position pawnPos;
                    if (movingPiece.color == Color.White)
                        pawnPos = new Position(3, destination.column);
                    else
                        pawnPos = new Position(4, destination.column);
                    board.PlacePiece(pawn, pawnPos);
                    pawn.DecreaseMovementAmount();
                }
            }
        }

        public void PerformMove(Position origin, Position destination)
        {
            Piece capturedPiece = MovePiece(origin, destination);

            if (IsInCheck(currentPlayer))
            {
                UndoMovePiece(origin, destination, capturedPiece);
                throw new BoardException("Você não pode se colocar em xeque!");
            }

            // # Special move: promotion
            Piece movingPiece = board.Piece(destination);
            if (movingPiece is Pawn)
            {
                if ((movingPiece.color == Color.White && destination.line == 0) || (movingPiece.color == Color.Black && destination.line == 7))
                {
                    movingPiece = board.PickUpPiece(destination);
                    pieces.Remove(movingPiece);
                    Piece queen = new Queen(movingPiece.color, board);
                    board.PlacePiece(queen, destination);
                    pieces.Add(queen);
                }
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

            // # Special move: en passant
            if (movingPiece is Pawn && (destination.line == origin.line - 2 || destination.line == origin.line + 2))
                enPassantVulnerable = movingPiece;
            else
                enPassantVulnerable = null;
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
            if (!board.Piece(origin).PossibleMove(destination))
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
            for (int c = 0; c < board.lines; c++)
            {
                PlaceNewPiece(new Pawn(Color.White, board, this), Convert.ToChar(97 + c), 2);
                PlaceNewPiece(new Pawn(Color.Black, board, this), Convert.ToChar(97 + c), 7);
            }
            PlaceNewPiece(new Rook(Color.White, board), 'a', 1);
            PlaceNewPiece(new Rook(Color.Black, board), 'a', 8);
            PlaceNewPiece(new Rook(Color.White, board), 'h', 1);
            PlaceNewPiece(new Rook(Color.Black, board), 'h', 8);

            PlaceNewPiece(new Knight(Color.White, board), 'b', 1);
            PlaceNewPiece(new Knight(Color.Black, board), 'b', 8);
            PlaceNewPiece(new Knight(Color.White, board), 'g', 1);
            PlaceNewPiece(new Knight(Color.Black, board), 'g', 8);

            PlaceNewPiece(new Bishop(Color.White, board), 'c', 1);
            PlaceNewPiece(new Bishop(Color.Black, board), 'c', 8);
            PlaceNewPiece(new Bishop(Color.White, board), 'f', 1);
            PlaceNewPiece(new Bishop(Color.Black, board), 'f', 8);

            PlaceNewPiece(new Queen(Color.White, board), 'd', 1);
            PlaceNewPiece(new Queen(Color.Black, board), 'd', 8);

            PlaceNewPiece(new King(Color.White, board, this), 'e', 1);
            PlaceNewPiece(new King(Color.Black, board, this), 'e', 8);
        }
    }
}