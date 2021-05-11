namespace board
{
    class Piece
    {
        public Chessman chessman { get; protected set; }
        public Position position { get; set; }
        public Color color { get; protected set; }
        public Board board { get; protected set; }
        public int MovementAmount { get; protected set; }

        public Piece(Chessman chessman, Position position, Color color, Board board)
        {
            this.chessman = chessman;
            this.position = position;
            this.color = color;
            this.board = board;
            MovementAmount = 0;
        }
    }
}
