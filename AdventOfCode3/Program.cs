using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode3
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

        const int gridSize = 12;
        static int[,] Grid = new int[gridSize, gridSize];
        const int input = 277678;
        static void Main(string[] args)
        {
            var start = gridSize / 2;
            var curPoint = new Point(start, start);
            Grid[curPoint.X, curPoint.Y] = 1;
            var size = 3;
            while (true)
            {
                curPoint = fillGrid(curPoint, size);
                size += 2;
            }

            //var size = getCircleSize(input);
            //var centers = getCenters(size);
            //var pathToCenter = size;
            //for (int i = 0; i < centers.Count(); i++)
            //{
            //    if (pathToCenter > Math.Abs(input - centers[i]))
            //    {
            //        pathToCenter = Math.Abs(input - centers[i]);
            //    }
            //}


           // Console.WriteLine(fullPath);
           // Console.ReadLine();
        }

        public static Point fillGrid(Point start, int size)
        {
            var x = start.X + 1;
            var y = start.Y;
            Grid[x, y] = getNeighboursSum(new Point(x, y));
            for (int i = 0; i < size - 2; i++)
            {
                Grid[x, --y] = getNeighboursSum(new Point(x, y));
            }
            for (int i = 0; i < size - 1; i++)
            {
                Grid[--x, y] = getNeighboursSum(new Point(x, y));
            }
            for (int i = 0; i < size - 1; i++)
            {
                Grid[x, ++y] = getNeighboursSum(new Point(x, y));
            }
            for (int i = 0; i < size - 1; i++)
            {
                Grid[++x, y] = getNeighboursSum(new Point(x, y));
            }
            return new Point(x, y);

        }

        public static int getNeighboursSum(Point p)
        {
            var sum = Grid[p.X + 1, p.Y] +
                Grid[p.X + 1, p.Y + 1] +
                Grid[p.X + 1, p.Y - 1] +
                Grid[p.X - 1, p.Y + 1] +
                Grid[p.X - 1, p.Y] +
                Grid[p.X - 1, p.Y - 1] +
                Grid[p.X, p.Y + 1] +
                Grid[p.X, p.Y - 1];
            if (sum > input)
            {
                Console.WriteLine($"{p.X - gridSize / 2}:{p.Y - gridSize / 2} {sum}");
                Console.ReadLine();
            }
            return sum;
        }

        public static int getCircleSize(int num)
        {
            var n = 1;
            while (num >= n * n)
            {
                n += 2;
            }
            return n;
        }

        public static int[] getCenters(int size)
        {
            var result = new int[4];
            var corner = size * size;
            result[0] = corner - size / 2;
            for (int i = 1; i < 4; i++)
            {
                result[i] = result[i - 1] - (size - 1);
            }
            return result;
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
