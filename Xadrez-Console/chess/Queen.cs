using board;

namespace chess
{
    class Queen : Piece
    {
        public Queen(Color color, Board board) : base(color, board) { }

        public override string ToString()
        {
            return "D";
        }
    }
}
