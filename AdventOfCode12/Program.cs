using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode12
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

        static Dictionary<int, List<int>> Relations = new Dictionary<int, List<int>>();
        static void Main(string[] args)
        {
            List<string> inputList = readInput("input.txt");
            var pattern = new Regex(@"(?<src>\d+) <-> (?<trg>[\d, ]+)");
            foreach (var item in inputList)
            {
                var groups = pattern.Match(item).Groups;
                var source = int.Parse(groups["src"].Value);
                var targets = groups["trg"].Value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(t => int.Parse(t)).ToList();
                Relations.Add(source, targets);
            }
            var groupCount = 0;
            while (Relations.Count > 0)
            {
                RemoveGroup(Relations.Keys.First());
                groupCount++;
            }
            Console.WriteLine(groupCount);            
            Console.ReadLine();
        }

        static int RemoveGroup(int start)
        {
            var searchList = new List<int> { start };
            var RelsZero = new List<int> { start };
            while (searchList.Count > 0)
            {
                var current = searchList.First();
                foreach (var prog in Relations[current])
                {
                    if (!RelsZero.Contains(prog))
                    {
                        RelsZero.Add(prog);
                        searchList.Add(prog);
                    }
                    Relations.Remove(current);
                }
                searchList.Remove(current);
            }
            if (start == 0)
            {
                Console.WriteLine(RelsZero.Count);
            }
            return RelsZero.Count;
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
