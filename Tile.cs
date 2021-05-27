namespace SnakeFinished
{
    public class Tile
    {
        public char symbol;
        public int x, y;
        public Tile(char symbol, int x, int y)
        {
            this.symbol = symbol;
            this.x = x;
            this.y = y;
        }
        public Tile(char symbol, int x, int y, Direction prevDirection)
        {
            this.symbol = symbol;
            this.x = x;
            this.y = y;
        }
        public Tile(Tile tile)
        {
            symbol = tile.symbol;
            x = tile.x;
            y = tile.y;
        }
        public Tile Empty
        {
            get
            {
                return new Tile(' ', x, y);
            }
        }
    }


}