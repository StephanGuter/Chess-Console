namespace board
{
    abstract class Piece
    {
        public Position position { get; set; }
        public Color color { get; protected set; }
        public Board board { get; protected set; }
        public int movementAmount { get; protected set; }

        public Piece(Color color, Board board)
        {
            this.position = null;
            this.color = color;
            this.board = board;
            movementAmount = -1;
        }

        public void IncreaseMovementAmount()
        {
            movementAmount++;
        }

        public void DecreaseMovementAmount()
        {
            movementAmount--;
        }

        public bool PossibleMovementsExists()
        {
            bool[,] matrix = PossibleMovements();
            for (int i = 0; i < board.lines; i++)
            {
                for (int j = 0; j < board.columns; j++)
                {
                    if (matrix[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool PossibleMove(Position pos)
        {
            return PossibleMovements()[pos.line, pos.column];
        }

        public abstract bool[,] PossibleMovements();
    }
}