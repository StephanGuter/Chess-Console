using board;

namespace chess
{
    class King : Piece
    {
        public King(Color color, Board board) : base(color, board) { }

        public override string ToString()
        {
            return "R";
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

            return matrix;
        }
    }
}
