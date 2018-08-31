using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode24
{
    class Program
    {
        class Node
        {
            public int[] Pins { get; set; }
            public int Id { get; set; }
            public Node(int a, int b)
            {
                Pins = new int[] { a, b };
            }            
        }

        class Bridge
        {
            public int Length { get; set; }
            public int Strength { get; set; }
            public Bridge(int l, int s)
            {
                Length = l;
                Strength = s;
            }
        }

        class State
        {
            public int Length { get; set; }
            public List<int> UsedNodes { get; set; }
            public int OuterPin { get; set; }
        }
        static List<State> CurrentStates = new List<State>();
        static List<Bridge> Bridges = new List<Bridge>();
        static List<Node> Nodes = new List<Node>();

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
            Nodes = readInput("input.txt").Select(s => s.Split(new char[] { '/' })).Select(sa => new Node(int.Parse(sa[0]), int.Parse(sa[1]))).ToList();
            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].Id = i;
            }
            var ZeroNodes = Nodes.Where(n => n.Pins.Contains(0));
            foreach (var zeroNode in ZeroNodes)
            {
                CurrentStates.Add(new State
                {
                    UsedNodes = new List<int> { zeroNode.Id },
                    Length = zeroNode.Pins.Sum(),
                    OuterPin = zeroNode.Pins.Sum()
                });
            }
            while (CurrentStates.Count > 0)
            {
                var currentState = CurrentStates.First();
                var nextStates = GetNextStates(currentState);
                if (nextStates.Count == 0)
                {
                    Bridges.Add(new Bridge(currentState.UsedNodes.Count, currentState.Length));
                }
                CurrentStates.AddRange(nextStates);
                CurrentStates.Remove(currentState);
            }
            Console.WriteLine(Bridges.Select(b => b.Strength).Max());
            var longestBridgeLength = Bridges.Select(br => br.Length).Max();
            Console.WriteLine(Bridges.Where(b => b.Length == longestBridgeLength).Select(lb => lb.Strength).Max());
            Console.ReadLine();
        }

        static List<State> GetNextStates(State state)
        {
            var result = new List<State>();
            var nextNodes = Nodes.Where(n => !state.UsedNodes.Contains(n.Id) && n.Pins.Contains(state.OuterPin));
            foreach (var node in nextNodes)
            {
                int[] arr = new int[state.UsedNodes.Count];
                state.UsedNodes.CopyTo(arr);
                var usedNodes = new List<int>(arr);
                usedNodes.Add(node.Id);
                result.Add(new State
                {
                    UsedNodes = usedNodes,
                    Length = state.Length + node.Pins.Sum(),
                    OuterPin = node.Pins[0] == state.OuterPin ? node.Pins[1] : node.Pins[0]
                });
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
