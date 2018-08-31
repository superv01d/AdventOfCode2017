using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode13
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

        static Dictionary<int, int> Firewall = new Dictionary<int, int>();

        static void Main(string[] args)
        {
            List<string> inputList = readInput("input.txt");
            foreach (var item in inputList)
            {
                var itemArr = item.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Firewall.Add(int.Parse(itemArr[0]), int.Parse(itemArr[1]));
            }
            var severity = 0;
            foreach (var depth in Firewall.Keys)
            {
                severity += Severity(depth);
            }
            Console.WriteLine(severity);

            var delay = 0;
            while (Caught(delay++))
            {

            }
            Console.WriteLine(--delay);
            Console.ReadLine();
        }

        static bool Caught(int delay)
        {
            foreach (var depth in Firewall.Keys)
            {
                var ms = depth + delay;
                if (Firewall.ContainsKey(depth))
                {
                    var range = Firewall[depth];
                    var cycleSize = (range - 1) * 2;
                    var effStep = ms % cycleSize;
                    if (effStep == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static int Severity(int depth)
        {
            if (Firewall.ContainsKey(depth))
            {
                var range = Firewall[depth];
                var cycleSize = (range - 1) * 2;
                var effStep = depth % cycleSize;
                if (effStep == 0)
                {
                    return depth * range;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
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
