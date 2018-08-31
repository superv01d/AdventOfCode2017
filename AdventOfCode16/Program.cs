using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode16
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
            var inputList = readInput("input.txt").First().Split(new char[] { ',' });
            var result = "abcdefghijklmnop";
            var step = 0;
            while (true)
            {
                if (result == "abcdefghijklmnop" && step > 0)
                {
                    break;
                }
                result = Dance(result, inputList);
                if (step == 0)
                {
                    Console.WriteLine(result);
                }
                step++;
            }
            var steps = 1000000000 % step;
            for (int i = 0; i < steps; i++)
            {
                result = Dance(result, inputList);
            }
            Console.WriteLine(result);
            Console.ReadLine();
        }

        static string Dance(string input, string[] moves)
        {
            foreach (var item in moves)
            {                
                switch (item[0])
                {
                    case 's':
                        var count = int.Parse(item.Substring(1));
                        input = Spin(input, count);
                        break;
                    case 'x':
                        var ab = item.Substring(1).Split(new char[] { '/' });
                        var a = int.Parse(ab[0]);
                        var b = int.Parse(ab[1]);
                        input = Exchange(input, a, b);
                        break;
                    case 'p':
                        var xy = item.Substring(1).Split(new char[] { '/' });
                        var x = xy[0][0];
                        var y = xy[1][0];
                        input = Partner(input, x, y);
                        break;
                }
            }
            return input;
        }

        static string Spin(string input, int count)
        {
            return input.Substring(input.Length - count) + input.Substring(0, input.Length - count);
        }

        static string Exchange(string input, int a, int b)
        {
            var charArr = input.ToCharArray();
            var temp = charArr[a];
            charArr[a] = charArr[b];
            charArr[b] = temp;
            return new string(charArr);
        }

        static string Partner(string input, char a, char b)
        {
            var x = input.IndexOf(a);
            var y = input.IndexOf(b);
            return Exchange(input, x, y);
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
