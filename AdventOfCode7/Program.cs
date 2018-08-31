using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Priority_Queue;

namespace AdventOfCode7
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

        class SubProgram
        {
            public string Name { get; set; }
            public int Weight { get; set; }
            public List<string> Children { get; set; }
            public int SumWeight { get; set; }
            public List<SubProgram> ChildNodes { get; set; }
            public SubProgram Parent { get; set; }
            public SubProgram(string init)
            {
                var regexp = new Regex(@"(?<name>\w+) \((?<weight>\d+)\)(?: -> (?<subs>[\w, ]+))?");
                var groups = regexp.Match(init).Groups;
                Name = groups["name"].Value;
                Weight = int.Parse(groups["weight"].Value);
                var subs = groups["subs"]?.Value;                
                if (!string.IsNullOrEmpty(subs))
                {
                    Children = new List<string>(subs.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries));
                    SumWeight = -1;
                }
                else
                {
                    Children = new List<string>();
                    SumWeight = Weight;
                }
            }
        }
        static List<SubProgram> SubPrograms;

        static void Main(string[] args)
        {
            SubPrograms = new List<SubProgram>();
            var inputList = readInput("input.txt");
            foreach (var item in inputList)
            {
                SubPrograms.Add(new SubProgram(item));
            }
            var allChildren = new List<string>();
            foreach (var sp in SubPrograms.Where(sp => sp.Children.Count > 0))
            {
                allChildren.AddRange(sp.Children);
            }
            var bottom = SubPrograms.Single(sp => !allChildren.Contains(sp.Name));
            Console.WriteLine($"Bottom program is {bottom.Name}");
            BuildNodeTree(bottom);
            CalculateSums(bottom);
            var current = bottom;
            var previous = bottom;
            while (current != null)
            {
                previous = current;
              //  Console.WriteLine(current.Name);
                current = GetWrongNode(current);
            }
            var diff = previous.SumWeight - GetCorrectWeight(previous.Parent);

            Console.WriteLine($"{previous.Name} is off by {diff}. Should weight {previous.Weight - diff}");
            Console.WriteLine();
            Console.ReadLine();
        }

        static void BuildNodeTree(SubProgram start)
        {
            if (start.Children.Count > 0)
            {
                var childs = SubPrograms.Where(sp => start.Children.Contains(sp.Name));
                start.ChildNodes = childs.ToList();
                foreach (var childNode in childs)
                {
                    childNode.Parent = start;
                    BuildNodeTree(childNode);
                }
            }
            else
            {
                return;
            }
        }

        static int GetCorrectWeight(SubProgram start)
        {
            var childs = start.ChildNodes;
            var correct = childs[0].SumWeight;
            if (childs[1].SumWeight != correct)
            {
                if (childs[2].SumWeight != correct)
                {
                    correct = childs[1].SumWeight;
                }
            }
            return correct;
        }

        static SubProgram GetWrongNode(SubProgram start)
        {
            var correct = GetCorrectWeight(start);
            return start.ChildNodes.SingleOrDefault(n => n.SumWeight != correct);
        }

        static void CalculateSums(SubProgram start)
        {

            while (start.ChildNodes.Exists(n => n.SumWeight == -1))
            {
                CalculateSums(start.ChildNodes.First(n => n.SumWeight == -1));
            }
            start.SumWeight = start.ChildNodes.Sum(n => n.SumWeight) + start.Weight;
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
