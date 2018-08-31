using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode14
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

        static List<List<int>> Grid = new List<List<int>>();
        static void Main(string[] args)
        {
            var input = "nbysizxe";
            for (int i = 0; i < 128; i++)
            {
                Grid.Add(Row($"{input}-{i}"));
            }
            var usedCount = Grid.Sum(r => r.Sum());
            Console.WriteLine(usedCount);
            var groupCount = 0;
            int row;
            while ((row = Grid.FindIndex(r => r.Any(c => c == 1))) != -1)
            {
                var col = Grid[row].FindIndex(c => c == 1);
                RemoveGroup(new Point(row, col));
                groupCount++;
            }
            Console.WriteLine(groupCount);
            Console.ReadLine();
        }

        static List<int> Row(string input)
        {
            return string.Join(string.Empty, AdventOfCode10.Program.GetHash(input).Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'))).Select(c => c - '0').ToList();
        }

        static void RemoveGroup(Point p)
        {
            var searchList = new List<Point> { p };
            while (searchList.Count > 0)
            {
                var current = searchList.First();
                searchList.AddRange(FindAdjacent(current));
                Grid[current.X][current.Y] = 0;
                searchList.Remove(current);
            }
        }

        static List<Point> FindAdjacent(Point p)
        {
            var result = new List<Point>();
            if (p.X - 1 >= 0)
            {
                if (Grid[p.X - 1][p.Y] == 1)
                {
                    result.Add(new Point(p.X - 1, p.Y));
                }
            }

            if (p.Y - 1 >= 0)
            {
                if (Grid[p.X][p.Y - 1] == 1)
                {
                    result.Add(new Point(p.X, p.Y - 1));
                }
            }

            if (p.X + 1 < Grid.Count)
            {
                if (Grid[p.X + 1][p.Y] == 1)
                {
                    result.Add(new Point(p.X + 1, p.Y));
                }
            }

            if (p.Y + 1 < Grid[p.X].Count)
            {
                if (Grid[p.X][p.Y + 1] == 1)
                {
                    result.Add(new Point(p.X, p.Y + 1));
                }
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
