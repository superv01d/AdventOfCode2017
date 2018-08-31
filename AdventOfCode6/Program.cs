using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode6
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

        class BlockSet
        {
            public List<int> Blocks { get; set; }

            public BlockSet(IEnumerable<int> ints)
            {
                Blocks = ints.ToList();
            }
            public override bool Equals(object obj)
            {
                var blocksObj = obj as BlockSet;
                for (int i = 0; i < Blocks.Count; i++)
                {
                    if (Blocks[i] != blocksObj.Blocks[i])
                    {
                        return false;
                    }
                }
                return true;
            }

            public override string ToString()
            {
                var s = string.Empty;
                foreach (var block in Blocks)
                {
                    s += $"{block}:";
                }
                return s;
            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }
        }
        static void Main(string[] args)
        {
            var inputList = readInput("input.txt").First().Split(new char[] { '\t' }).Select(s => int.Parse(s));
            var blocks = new BlockSet(inputList);
            var history = new List<string> { blocks.ToString() };
            var counter = 0;
            var cycleStartState = string.Empty;
            while (true)
            {
                counter++;
                Redistribute(blocks.Blocks);
                var state = blocks.ToString();
                if (history.Contains(state))
                {
                    cycleStartState = state;
                    break;
                }
                else
                {
                    history.Add(state);
                }
            }
            Console.WriteLine($"Part 1: {counter}");

            counter = 0;
            do
            {
                counter++;
                Redistribute(blocks.Blocks);
            } while (cycleStartState != blocks.ToString());

            Console.WriteLine($"Part 2: {counter}");
            Console.ReadLine();
        }

        static void Redistribute(List<int> memory)
        {
            var max = memory.Max();
            var index = memory.IndexOf(max);
            memory[index] = 0;
            for (int i = 0; i < max; i++)
            {
                memory[(index + i + 1) % memory.Count]++;
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
