using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Priority_Queue;

namespace AdventOfCode8
{
    class Program
    {
        static Dictionary<string, int> Registers = new Dictionary<string, int>();
        static int Max = int.MinValue;
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
            var inputList = readInput("input.txt");
            foreach (var item in inputList)
            {
                ProcessInstruction(item);
            }
            Console.WriteLine(Registers.Select(r => r.Value).Max());
            Console.WriteLine(Max);
            Console.ReadLine();
        }

        static void ProcessInstruction(string instruction)
        {
            var regexp = new Regex(@"(?<register>\w+) (?<operator>dec|inc) (?<amount>[-\d]+) if (?<conditionalReg>\w+) (?<condition>[<=>!]+) (?<conditionAmount>[-\d]+)");
            var groups = regexp.Match(instruction).Groups;
            var register = groups["register"].Value;
            var op = groups["operator"].Value;
            var amount = int.Parse(groups["amount"].Value);
            var conditionalReg = groups["conditionalReg"].Value;
            var condition = groups["condition"].Value;
            var conditionAmount = int.Parse(groups["conditionAmount"].Value);
            ExecuteInstruction(register, op, amount, conditionalReg, condition, conditionAmount);
        }

        static void ExecuteInstruction(string reg, string op, int amount, string conReg, string condition, int condAmount)
        {
            if (!Registers.ContainsKey(reg))
            {
                Registers[reg] = 0;
            }
            if (!Registers.ContainsKey(conReg))
            {
                Registers[conReg] = 0;
            }
            var conditionTrue = false;
            switch (condition)
            {
                case ">":
                    if (Registers[conReg] > condAmount)
                    {
                        conditionTrue = true;
                    }
                    break;
                case "<":
                    if (Registers[conReg] < condAmount)
                    {
                        conditionTrue = true;
                    }
                    break;
                case ">=":
                    if (Registers[conReg] >= condAmount)
                    {
                        conditionTrue = true;
                    }
                    break;
                case "<=":
                    if (Registers[conReg] <= condAmount)
                    {
                        conditionTrue = true;
                    }
                    break;
                case "==":
                    if (Registers[conReg] == condAmount)
                    {
                        conditionTrue = true;
                    }
                    break;
                case "!=":
                    if (Registers[conReg] != condAmount)
                    {
                        conditionTrue = true;
                    }
                    break;
            }

            if (conditionTrue)
            {
                switch (op)
                {
                    case "inc":
                        Registers[reg] += amount;
                        break;
                    case "dec":
                        Registers[reg] -= amount;
                        break;
                }
                if (Registers[reg] > Max)
                {
                    Max = Registers[reg];
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
