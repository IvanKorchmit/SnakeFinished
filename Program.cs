using System;
using System.Threading;
using System.Collections.Generic;
namespace SnakeFinished
{
    class Program
    {
        public static Tile[,] tiles;
        public static Snake snake;
        public static List<Snake> tail;
        public static int Length = 0;
        public static bool isPlaying;
        public static int Score;
        static void Main(string[] args)
        {
            isPlaying = true;
            tail = new List<Snake>();
            Random rand = new Random();
            tiles = new Tile[40, 40];
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    tiles[y, x] = new Tile(' ', x, y);
                    if (x == tiles.GetLength(0) / 2 && y == tiles.GetLength(1) / 2)
                    {
                        Snake snake = new Snake('S', true, y, x);
                        snake.direction = Direction.LEFT;
                        tiles[y, x] = snake;
                        Program.snake = snake;
                    }
                }
            }
            int x2 = rand.Next(0, tiles.GetLength(0) - 1);
            int y2 = rand.Next(0, tiles.GetLength(1) - 1);
            tiles[x2, y2] = new Food(false, 'F', x2, y2);

            x2 = rand.Next(0, tiles.GetLength(0) - 1);
            y2 = rand.Next(0, tiles.GetLength(1) - 1);
            tiles[x2, y2] = new Food(false, 'F', x2, y2);
            while (isPlaying)
            {
                snake.Move();
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo k = Console.ReadKey();
                    if (k.Key == ConsoleKey.RightArrow)
                    {
                        snake.direction = Direction.RIGHT;

                    }
                    else if (k.Key == ConsoleKey.LeftArrow)
                    {
                        snake.direction = Direction.LEFT;
                    }
                    else if (k.Key == ConsoleKey.UpArrow)
                    {
                        snake.direction = Direction.UP;
                    }
                    else if (k.Key == ConsoleKey.DownArrow)
                    {
                        snake.direction = Direction.DOWN;
                    }
                }
                render();
                Thread.Sleep(100);
            }
        }
        static void render()
        {
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    Console.Write(tiles[x, y].symbol);
                }
                Console.WriteLine();
            }
        }
        public static void GameOver()
        {
            isPlaying = false;
            Console.Clear();
            Console.WriteLine("Game Over");
            Console.WriteLine($"Score: {Score}");
            Console.ReadLine();
        }
    }
    public class Tile
    {
        public char symbol;
        public int x, y;
        public Direction prevDirection;
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
            this.prevDirection = prevDirection;
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
                return new Tile(' ', x, y, prevDirection);
            }
        }
    }
    public class Snake : Tile
    {
        public bool Head;
        public int Age;
        public Direction direction;
        public Snake(char symbol, bool Head, int x, int y) : base(symbol, x, y)
        {
            this.Head = Head;
        }
        public void Move()
        {
            if (Head)
            {
                switch (direction)
                {
                    case Direction.UP:
                        Snake newSnake = new Snake(symbol, Head, x, y);
                        Program.tiles[x, y] = newSnake;
                        x = (x - 1 + Program.tiles.GetLength(0)) % Program.tiles.GetLength(0);
                        if (Program.tiles[x, y] is Food)
                        {
                            Grow();
                        }
                        else if (Program.tiles[x, y] is Snake)
                        {
                            Program.GameOver();
                        }

                        Program.tail.Add(newSnake);
                        Program.tiles[x, y] = this;
                        if (Program.tail.Count > Program.Length)
                        {
                            Shorten();
                        }
                        break;
                    case Direction.RIGHT:
                        Snake newSnake2 = new Snake(symbol, Head, x, y);
                        Program.tiles[x, y] = newSnake2;
                        y = (y + 1 + Program.tiles.GetLength(1)) % Program.tiles.GetLength(1);
                        if (Program.tiles[x, y] is Food)
                        {
                            Grow();
                        }
                        else if (Program.tiles[x, y] is Snake)
                        {
                            Program.GameOver();
                        }
                        Program.tail.Add(newSnake2);
                        Program.tiles[x, y] = this;
                        if (Program.tail.Count > Program.Length)
                        {
                            Shorten();


                        }
                        break;
                    case Direction.LEFT:
                        Snake newSnake3 = new Snake(symbol, Head, x, y);
                        Program.tiles[x, y] = newSnake3;
                        y = (y - 1 + Program.tiles.GetLength(1)) % Program.tiles.GetLength(1);
                        y = y % Program.tiles.GetLength(1);
                        if (Program.tiles[x, y] is Food)
                        {
                            Grow();
                        }
                        else if (Program.tiles[x, y] is Snake)
                        {
                            Program.GameOver();
                        }
                        Program.tail.Add(newSnake3);
                        Program.tiles[x, y] = this;
                        if (Program.tail.Count > Program.Length)
                        {
                            Shorten();


                        }
                        break;
                    case Direction.DOWN:
                        Snake newSnake4 = new Snake(symbol, Head, x, y);
                        Program.tiles[x, y] = newSnake4;
                        x = (x + 1 + Program.tiles.GetLength(0)) % Program.tiles.GetLength(0);
                        if (Program.tiles[x, y] is Food)
                        {
                            Grow();
                        }
                        else if (Program.tiles[x, y] is Snake)
                        {
                            Program.GameOver();
                        }
                        Program.tail.Add(newSnake4);
                        Program.tiles[x, y] = this;
                        if (Program.tail.Count > Program.Length)
                        {
                            Shorten();

                        }
                        break;
                    default:
                        break;
                }
            }
        }
        public void Grow()
        {
            Random rand = new Random();
            Console.Beep();
            Program.Length++;
            int x2 = rand.Next(0, Program.tiles.GetLength(0) - 1);
            int y2 = rand.Next(0, Program.tiles.GetLength(1) - 1);
            Program.tiles[x2, y2] = new Food(false, 'F', x2, y2);
            Program.Score += 100;
        }
        private void Shorten()
        {
            Program.tiles[Program.tail[0].x, Program.tail[0].y] =
                                            Program.tiles[Program.tail[0].x, Program.tail[0].y].Empty;
            Program.tail.RemoveAt(0);
        }
    }
    public class Food : Tile
    {
        public bool isBad;
        public Food(bool isBad, char symbol, int x, int y) : base(symbol, x, y)
        {
            this.isBad = isBad;
        }
    }
    public enum Direction
    {
        UP, RIGHT, LEFT, DOWN
    }


}