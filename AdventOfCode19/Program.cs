using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode19
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

        static char[,] Map = new char[202,200];
        static void Main(string[] args)
        {
            var r = 0;
            var inputList = readInput("input.txt");
            foreach (var item in inputList)
            {
                for (int c = 0; c < item.Length; c++)
                {
                    Map[c, r] = item[c];
                }
                r++;
            }
            var Direction = new Point(0, 1);
            var y = 0;
            var x = 0;
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                if (Map[i, 0] == '|')
                {
                    x = i;
                    break;
                }
            }
            var steps = 0;
            char curChar;
            string result = string.Empty;
            do
            {
                x += Direction.X;
                y += Direction.Y;
                curChar = Map[x, y];
                if (curChar == '+')
                {
                    Direction = ChangeDirection(x, y, Direction);
                }
                else if (curChar >= 'A' && curChar <= 'Z')
                {
                    result += curChar;
                }
                steps++;
            } while (curChar != ' ');

            Console.WriteLine(result);
            Console.WriteLine(steps);
            Console.ReadLine();
        }

        static Point ChangeDirection(int x, int y, Point curDirection)
        {
            if (curDirection.X == 0)
            {
                if (x == Map.GetLength(0) - 1 || Map[x + 1, y] == ' ')
                {
                    return new Point(-1, 0);
                }
                else
                {
                    return new Point(1, 0);
                }
            }
            else
            {
                if (y == Map.GetLength(1) - 1 || Map[x, y + 1] == ' ')
                {
                    return new Point(0, -1);
                }
                else
                {
                    return new Point(0, 1);
                }
            }
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
