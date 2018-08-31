using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode22
{
    class Program
    {
        static List<string> readInput(string filename)
        {
            List<string> result = new List<string>();
            string s;
            using (var sr = File.OpenText(filename))
            {
                while ((s = sr.ReadLine()) != null)
                {
                    result.Add(s);
                }
            }
            return result;
        }
        const int InfiniteMapSize = 999;
        const int inputSize = 25;
        static char[,] Map = new char[InfiniteMapSize, InfiniteMapSize];

        class Carrier
        {
            public Point Position { get; set; }
            public Point Direction { get; set; }
        }

        static void Main(string[] args)
        {
            for (int i = 0; i < InfiniteMapSize; i++)
            {
                for (int j = 0; j < InfiniteMapSize; j++)
                {
                    Map[i, j] = '.';
                }
            }
            var inputList = readInput("input.txt");
            var center = InfiniteMapSize / 2;
            var inputStartPoint = center - inputSize / 2;
            for (int i = 0; i < inputList.Count; i++)
            {
                for (int j = 0; j < inputList[i].Length; j++)
                {
                    Map[i + inputStartPoint, j + inputStartPoint] = inputList[i][j];
                }
            }
            var carrier = new Carrier
            {
                Direction = new Point(-1, 0),
                Position = new Point(center, center)
            };
            var counter = 0;
            for (int i = 0; i < 10000000; i++)
            {
                counter += Burst(carrier);
            }
            Console.WriteLine(counter);
            Console.ReadLine();
        }

        static int Burst(Carrier carrier)
        {
            var currentChar = Map[carrier.Position.X, carrier.Position.Y];
            if (currentChar == '#')
            {
                carrier.Direction = Turn(carrier.Direction, true);
                Map[carrier.Position.X, carrier.Position.Y] = 'F';
                carrier.Position = new Point(carrier.Position.X + carrier.Direction.X, carrier.Position.Y + carrier.Direction.Y);
                return 0;
            }
            else if (currentChar == '.')
            {
                carrier.Direction = Turn(carrier.Direction, false);
                Map[carrier.Position.X, carrier.Position.Y] = 'W';
                carrier.Position = new Point(carrier.Position.X + carrier.Direction.X, carrier.Position.Y + carrier.Direction.Y);
                return 0;
            }
            else if (currentChar == 'F')
            {
                carrier.Direction = Turn(Turn(carrier.Direction, true), true);
                Map[carrier.Position.X, carrier.Position.Y] = '.';
                carrier.Position = new Point(carrier.Position.X + carrier.Direction.X, carrier.Position.Y + carrier.Direction.Y);
                return 0;
            }
            else
            {
                Map[carrier.Position.X, carrier.Position.Y] = '#';
                carrier.Position = new Point(carrier.Position.X + carrier.Direction.X, carrier.Position.Y + carrier.Direction.Y);
                return 1;
            }
        }

        static Point Turn(Point direction, bool right)
        {
            int x = 0, y = 0;
            if (direction.X == 0)
            {
                y = 0;
                x = right ? direction.Y : -direction.Y;
            }
            else
            {
                x = 0;
                y = right ? -direction.X : direction.X;
            }
            return new Point(x, y);
        }

        #region Helpers

        private static string getMD5(string s)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(s);
            var hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }

        public class Point
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public override string ToString()
            {
                return string.Format("{0}:{1}", X, Y);
            }

            public override bool Equals(object obj)
            {
                var pointObj = obj as Point;
                return (X.Equals(pointObj.X) && Y.Equals(pointObj.Y));
            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }
        }

        #endregion

    }
}
