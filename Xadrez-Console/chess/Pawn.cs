using board;

namespace chess
{
    class Pawn : Piece
    {
        public Pawn(Color color, Board board) : base(color, board) { }

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

            if (color == Color.White)
                direction = -1;
            else
                direction = 1;


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

            return matrix;
        }
    }
}