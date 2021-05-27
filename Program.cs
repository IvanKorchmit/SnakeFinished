using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;
using System.Text;
namespace SnakeFinished
{
	class Program
	{
		public static Tile[,] tiles;
		public static Snake snake;
		public static List<Snake> tail;
		public static List<Leader> leaderboard = new List<Leader>();
		public static int Length = 0;
		public static bool isPlaying;
		private static string apppath;
		public static int Score;
		static void Main(string[] args)
		{
			apppath = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
			Leader[] leaders = (Leader[])JsonSerializer.Deserialize(File.ReadAllText(apppath + "/leaders.json"), typeof(Leader[]));
            foreach (var l in leaders)
            {
				leaderboard.Add(l);
            }
			Random rand = new Random();
			Initialize();
			int x2 = rand.Next(0, tiles.GetLength(0) - 1);
			int y2 = rand.Next(0, tiles.GetLength(1) - 1);
			tiles[x2, y2] = new Food(false, 'F', x2, y2);
			DrawRect(x2/2, y2/2, 10, 10);
			while (isPlaying)
			{
				snake.Move();
				Input();
				render();
				Thread.Sleep(snake.Speed);
			}
		}

		static void Input()
        {
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
		}
		static void Initialize()
		{
			isPlaying = true;
			tail = new List<Snake>();
			tiles = new Tile[40, 40];
			for (int x = 0; x < tiles.GetLength(0); x++)
			{
				for (int y = 0; y < tiles.GetLength(1); y++)
				{
					tiles[y, x] = new Tile(' ', x, y);
					if (x == tiles.GetLength(0) / 2 && y == tiles.GetLength(1) / 2)
					{
						Snake snake = new Snake('▓', true, y, x);
						snake.direction = Direction.LEFT;
						tiles[y, x] = snake;
						Program.snake = snake;
					}
				}
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
            Console.WriteLine("Enter your name into the leaderboard!");
			var sorted = leaderboard.OrderByDescending(x => x.Score).ToList();
            foreach (var item in sorted)
            {
                Console.WriteLine(item.ToString());
            }
			leaderboard.Add(new Leader(Console.ReadLine(), Score));
			Leader[] CopyLeaderBoard = leaderboard.ToArray();
			string json = JsonSerializer.Serialize(CopyLeaderBoard);
			File.WriteAllText(apppath + "/leaders.json",json);
		}
		static void DrawRect(int x, int y, int width, int height)
        {
            for (int i = x; i < x+width; i++)
            {
				tiles[i, y] = new Wall('!', i, y);
            }
            for (int i = y; i < y + height; i++)
            {
				tiles[x, i] = new Wall('!', x, i);
			}
            for (int i = width + x; i >= x; i--)
            {
				tiles[i, height + y] = new Wall('!', i, y);
			}
		}
	}
	public class Snake : Tile
	{
		public bool Head;
		public int Age;
		public Direction direction;
		public int Speed;
		public Snake(char symbol, bool Head, int x, int y) : base(symbol, x, y)
		{
			this.Head = Head;
			Speed = 100;
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
						check();

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
						check();
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
						check();
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
						check();
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
		private void check()
        {
			if (Program.tiles[x, y] is Food f)
			{
				if (!f.isBad)
				{
					Grow();

				}
				else
				{
					UnGrow();
				}
			}
			else if (Program.tiles[x, y] is Snake || Program.tiles[x,y] is Wall)
			{
				Program.GameOver();
			}
		}
		public void Grow()
		{
			Random rand = new Random();
			new Thread(() =>
			{
				Console.Beep();
			}).Start();
			Speed -= 50;
			Speed = Math.Clamp(Speed, 50, 200);
			Program.Length++;
			int x2 = rand.Next(0, Program.tiles.GetLength(0) - 1);
			int y2 = rand.Next(0, Program.tiles.GetLength(1) - 1);
			Program.tiles[x2, y2] = new Food(false, 'F', x2, y2);
			Program.Score += 100;
			if (Program.tail.Count > 0)
				Program.tiles[Program.tail[0].x-1, Program.tail[0].y] =
												new Food(true, 'D', Program.tail.Last().x, Program.tail.Last().y);
			else
				Program.tiles[Program.snake.x, Program.snake.y] =
												new Food(true, 'D', Program.snake.x, Program.snake.y);
			Shorten();

		}
		public void UnGrow()
		{
			Random rand = new Random();
			Program.Length -= 2;
			Shorten();
			Shorten();

			if (Program.Length < 0) Program.GameOver();
			Program.Score -= 100;
		}
		private void Shorten()
		{
			if(Program.tail.Count > 0)
			{
				Program.tiles[Program.tail[0].x, Program.tail[0].y] =
								Program.tiles[Program.tail[0].x, Program.tail[0].y].Empty;
				Program.tail.RemoveAt(0);
			}

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

	class Wall : Tile
	{
		public Wall(char symbol, int x, int y) : base(symbol, x, y)
		{
			this.symbol = symbol;
			this.x = x;
			this.y = y;
		}
	}
	[System.Serializable]
	class Leader
    {
        public int Score { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
			return $"{Name} {Score}";
        }

        public Leader(string Name, int Score) 
        {
			this.Name = Name;
			this.Score = Score;
        }
        public Leader()
        {

        }
    }
}