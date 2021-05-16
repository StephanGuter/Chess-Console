using board;

namespace chess
{
    class ChessPosition
    {
        public char column { get; set; }
        public int line { get; set; }

        public ChessPosition(char column, int line)
        {
            this.column = column;
            this.line = line;
        }

        //public Position ToPosition(Board board)
        //{
        //    int l;
        //    int c;

        //    byte asc = Convert.ToByte(column);

        //    if (asc > 64 && asc < 91)
        //    {
        //        c = asc - 65;
        //    }
        //    else
        //    {
        //        if (asc > 96 && asc < 123)
        //        {
        //            c = asc - 97;
        //        }
        //        else
        //        {
        //            c = -1;
        //        }
        //    }

        //    l = board.lines - line;

        //    return new Position(l, c);
        //}

        public Position ToPosition()
        {
            return new Position(8 - line, column - 'a');
        }

        public override string ToString()
        {
            return "" + column + line;
        }
    }
}