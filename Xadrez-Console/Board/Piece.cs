namespace board
{
    class Piece
    {
        public Chessman chessman { get; set; }
        public Position position { get; set; }
        public Color color { get; set; }
        public Board board { get; set; }
        public int MovementAmount { get; set; }

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
