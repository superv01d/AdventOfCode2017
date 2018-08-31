using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode15
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
            long previousA = 516;
            long previousB = 190;
            var counter = 0;
            var compareCount = 0;
            while (compareCount < 5000000)
            {
                do
                {
                    previousA = (previousA * 16807) % 2147483647;
                } while (previousA % 4 != 0);
                do
                {
                    previousB = (previousB * 48271) % 2147483647;
                } while (previousB % 8 != 0);
                if (LowerBitsEqual(previousA, previousB))
                {
                    counter++;
                }
                compareCount++;
            }
            Console.WriteLine(counter);

            Console.ReadLine();
        }

        static bool LowerBitsEqual(long a, long b)
        {
            if ((a & 65535) == (b & 65535))
            {
                return true;
            }
            else
            {
                return false;
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
