using board;

namespace chess
{
    class King : Piece
    {
        private ChessMatch _match;

        public King(Color color, Board board, ChessMatch match) : base(color, board)
        {
            _match = match;
        }

        public override string ToString()
        {
            return "R";
        }

        private bool MightMove(Position pos) 
        {
            Piece p = board.Piece(pos);
            return p == null || p.color != color;
        }

        private bool RookCanDoCastling(Position pos)
        {
            Piece p = board.Piece(pos);
            return p != null && p is Rook && p.color == color && p.movementAmount == 0;
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] matrix = new bool[board.lines, board.columns];

            Position pos = new Position(0, 0);

            // Up
            pos.SetValues(position.line - 1, position.column);
            if (board.IsValidPosition(pos) && MightMove(pos))
            {
                matrix[pos.line, pos.column] = true;
            }

            // Up-Right
            pos.SetValues(position.line - 1, position.column + 1);
            if (board.IsValidPosition(pos) && MightMove(pos))
            {
                matrix[pos.line, pos.column] = true;
            }

            // Right
            pos.SetValues(position.line, position.column + 1);
            if (board.IsValidPosition(pos) && MightMove(pos))
            {
                matrix[pos.line, pos.column] = true;
            }

            // Down-Right
            pos.SetValues(position.line + 1, position.column + 1);
            if (board.IsValidPosition(pos) && MightMove(pos))
            {
                matrix[pos.line, pos.column] = true;
            }

            // Down
            pos.SetValues(position.line + 1, position.column);
            if (board.IsValidPosition(pos) && MightMove(pos))
            {
                matrix[pos.line, pos.column] = true;
            }

            // Down-Left
            pos.SetValues(position.line + 1, position.column - 1);
            if (board.IsValidPosition(pos) && MightMove(pos))
            {
                matrix[pos.line, pos.column] = true;
            }

            // Left
            pos.SetValues(position.line, position.column - 1);
            if (board.IsValidPosition(pos) && MightMove(pos))
            {
                matrix[pos.line, pos.column] = true;
            }

            // Up-Left
            pos.SetValues(position.line - 1, position.column - 1);
            if (board.IsValidPosition(pos) && MightMove(pos))
            {
                matrix[pos.line, pos.column] = true;
            }

            // # Special move: castling.
            if (movementAmount == 0 && !_match.check)
            {
                // Castling king side.
                Position rookPosKingSide = new Position(position.line, position.column + 3);
                if (RookCanDoCastling(rookPosKingSide))
                {
                    Position firstField = new Position(position.line, position.column + 1);
                    Position secondField = new Position(position.line, position.column + 2);
                    if (board.Piece(firstField) == null && board.Piece(secondField) == null)
                        matrix[secondField.line, secondField.column] = true;
                }

                // Castling queen side.
                Position rookPosQueenSide = new Position(position.line, position.column - 4);
                if (RookCanDoCastling(rookPosQueenSide))
                {
                    Position firstField = new Position(position.line, position.column - 1);
                    Position secondField = new Position(position.line, position.column - 2);
                    Position thirdField = new Position(position.line, position.column - 3);
                    if (board.Piece(firstField) == null && board.Piece(secondField) == null && board.Piece(thirdField) == null)
                        matrix[secondField.line, secondField.column] = true;
                }
            }

            return matrix;
        }
    }
}