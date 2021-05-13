using board;

namespace chess
{
    class Rook : Piece
    {
        public Rook(Color color, Board board) : base(color, board) { }

        public override string ToString()
        {
            return "T";
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

            // Up
            pos.SetValues(position.line - 1, position.column);
            while (board.IsValidPosition(pos) && MightMove(pos))
            {
                matrix[pos.line, pos.column] = true;
                if (board.Piece(pos) != null && board.Piece(pos).color != color)
                {
                    break;
                }
                pos.line -= 1;
            }

            // Right
            pos.SetValues(position.line, position.column + 1);
            while (board.IsValidPosition(pos) && MightMove(pos))
            {
                matrix[pos.line, pos.column] = true;
                if (board.Piece(pos) != null && board.Piece(pos).color != color)
                {
                    break;
                }
                pos.column += 1;
            }

            // Down
            pos.SetValues(position.line + 1, position.column);
            while (board.IsValidPosition(pos) && MightMove(pos))
            {
                matrix[pos.line, pos.column] = true;
                if (board.Piece(pos) != null && board.Piece(pos).color != color)
                {
                    break;
                }
                pos.line += 1;
            }

            // Right
            pos.SetValues(position.line, position.column - 1);
            while (board.IsValidPosition(pos) && MightMove(pos))
            {
                matrix[pos.line, pos.column] = true;
                if (board.Piece(pos) != null && board.Piece(pos).color != color)
                {
                    break;
                }
                pos.column -= 1;
            }

            return matrix;
        }
    }
}