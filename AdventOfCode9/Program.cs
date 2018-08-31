using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode9
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
            var regex = new Regex(@"(<>|<.*?[^!]>)");
            var inputList = readInput("input.txt").First();
            var input = Regex.Replace(inputList, "!!", "");
            var clearedInput = regex.Replace(input, "");

            var score = 0;
            var level = 0;
            foreach (var c in clearedInput)
            {
                if (c == '{')
                {
                    level++;
                    score += level;
                }
                else if (c == '}')
                {
                    level--;
                }
            }
            Console.WriteLine(score);

            var inputCount = inputList.Length;            
            var doubleExclams = Regex.Matches(inputList, "!!").Count;
            var cancelledChars = Regex.Matches(input, "!").Count;
            var garbageStarts = regex.Matches(input).Count;
            var garbageLength = inputCount - clearedInput.Length - doubleExclams * 2 - cancelledChars * 2 - garbageStarts * 2;
            Console.WriteLine(garbageLength);
            Console.ReadLine();
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
