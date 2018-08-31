using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode11
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
        static void Main(string[] args)
        {
            int x = 0, y = 0, z = 0;
            var inputList = readInput("input.txt").First().Split(new char[] { ',' });
            var maxDist = 0;
            foreach (var item in inputList)
            {
                switch (item)
                {
                    case "n":
                        y++;
                        break;
                    case "nw":
                        x++;
                        break;
                    case "ne":
                        z++;
                        break;
                    case "sw":
                        z--;
                        break;
                    case "s":
                        y--;
                        break;
                    case "se":
                        x--;
                        break;
                }
                var dist = Distance(x, y, z);
                maxDist = maxDist < dist ? dist : maxDist;
            }
            Console.WriteLine(Distance(x, y, z));
            Console.WriteLine(maxDist);


            Console.ReadLine();
        }

        static int Distance(int x, int y, int z)
        {
            if (x >= 0)
            {
                if (y >= 0)
                {
                    if (z >= 0)
                    {
                        return Math.Max(x + y, y + z);
                    }
                    else
                    {
                        return Math.Max(x + y, x - z);
                    }
                }
                else
                {
                    if (z >= 0)
                    {
                        return x - y + z - Math.Min(Math.Min(x, -y), z) * 3;
                    }
                    else
                    {
                        return Math.Max(x - z, -y - z);
                    }
                }
            }
            else
            {
                if (y >= 0)
                {
                    if (z >= 0)
                    {
                        return Math.Max(z + y, z - x);
                    }
                    else
                    {
                        return -x + y - z - Math.Min(Math.Min(-x, y), -z) * 3;
                    }
                }
                else
                {
                    if (z >= 0)
                    {
                        return Math.Max(z - x, -y - x);
                    }
                    else
                    {
                        return Math.Max(-x - y, -y - z);
                    }
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
