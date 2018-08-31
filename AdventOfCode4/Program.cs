using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode4
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
            List<string> inputList = readInput("input.txt");
            var sa = 0;
            var sb = 0;
            foreach (var item in inputList)
            {
                if (isValidPassphraseA(item))
                {
                    sa++;
                }
                if (isValidPassphraseB(item))
                {
                    sb++;
                }
            }
            Console.WriteLine(sa);
            Console.WriteLine(sb);
            Console.ReadLine();
        }

        static bool isValidPassphraseA(string input)
        {
            return !input
                .Split(new char[] { ' ' })
                .GroupBy(s => s)
                .Any(g => g.Count() > 1);
        }
        static bool isValidPassphraseB(string input)
        {
            return !input
                .Split(new char[] { ' ' })
                .Select(s => string.Join("", s.OrderByDescending(c => c)))
                .GroupBy(s => s)
                .Any(g => g.Count() > 1);
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
