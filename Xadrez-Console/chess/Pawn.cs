using board;

namespace chess
{
    class Pawn : Piece
    {
        private ChessMatch _match;

        public Pawn(Color color, Board board, ChessMatch match) : base(color, board) 
        {
            _match = match;
        }

        public override string ToString()
        {
            return "P";
        }

        private bool MightMove(Position pos)
        {
            Piece p = board.Piece(pos);
            return p == null || p.color != color;
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] matrix = new bool[board.lines, board.columns];

            Position pos = new Position(0, 0);

            int direction;
            int enPassantLine;

            if (color == Color.White)
            {
                direction = -1;
                enPassantLine = 3;
            }
            else
            {
                direction = 1;
                enPassantLine = 4;
            }


            // Forward
            pos.SetValues(position.line + direction, position.column);
            if (board.IsValidPosition(pos) && MightMove(pos))
            {
                if (board.Piece(pos) == null)
                    matrix[pos.line, pos.column] = true;
            }

            // Forward x2
            if (movementAmount == 0)
            {
                pos.SetValues(position.line + direction + direction, position.column);
                if (board.IsValidPosition(pos) && MightMove(pos))
                {
                    if (board.Piece(pos) == null)
                        matrix[pos.line, pos.column] = true;
                }
            }

            // Forward-Left
            pos.SetValues(position.line + direction, position.column - 1);
            if (board.IsValidPosition(pos) && MightMove(pos))
            {
                if (board.Piece(pos) != null)
                    matrix[pos.line, pos.column] = true;
            }

            // Forward-Right
            pos.SetValues(position.line + direction, position.column + 1);
            if (board.IsValidPosition(pos) && MightMove(pos))
            {
                if (board.Piece(pos) != null)
                    matrix[pos.line, pos.column] = true;
            }

            // # Special move: en passant
            if (position.line == enPassantLine)
            {
                Position onTheLeft = new Position(position.line, position.column - 1);
                if (board.IsValidPosition(onTheLeft) && board.Piece(onTheLeft) == _match.enPassantVulnerable)
                    matrix[onTheLeft.line + direction, onTheLeft.column] = true;

                Position onTheRight = new Position(position.line, position.column + 1);
                if (board.IsValidPosition(onTheRight) && board.Piece(onTheRight) == _match.enPassantVulnerable)
                    matrix[onTheRight.line + direction, onTheRight.column] = true;
            }

            return matrix;
        }
    }
}