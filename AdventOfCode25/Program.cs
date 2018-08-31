using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;
using System.Text.RegularExpressions;

namespace AdventOfCode25
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

        class Actions
        {
            public int Value { get; set; }
            public int Direction { get; set; }
            public char NextState { get; set; }
        }

        class FullState
        {
            public char State { get; set; }
            public int Index { get; set; }
        }

        const int TapeSize = 20000;
        static int[] Tape = new int[TapeSize];

        static void Main(string[] args)
        {
            var stateRules = new Dictionary<string, Actions>();
            var fullState = new FullState
            {
                State = 'A',
                Index = TapeSize / 2
            };
            var inputList = readInput("input.txt");
            var stateRe = new Regex("In state (.)");
            var valRe = new Regex(@"If the current value is (\d)");
            var nextValRe = new Regex(@"Write the value (\d)");
            var moveRe = new Regex(@"Move one slot to the (\w+)");
            var nextStateRe = new Regex(@"Continue with state (.)");
            var curState = 'A';
            var curVal = 0;
            var act = new Actions();
            foreach (var item in inputList)
            {                
                if (stateRe.IsMatch(item))
                {                    
                    curState = stateRe.Match(item).Groups[1].Value[0];
                }
                else if (valRe.IsMatch(item))
                {
                    curVal = int.Parse(valRe.Match(item).Groups[1].Value);
                }
                else if (nextValRe.IsMatch(item))
                {
                    act.Value = int.Parse(nextValRe.Match(item).Groups[1].Value);
                }
                else if (moveRe.IsMatch(item))
                {
                    act.Direction = moveRe.Match(item).Groups[1].Value == "right" ? 1 : -1;
                }
                else if (nextStateRe.IsMatch(item))
                {
                    act.NextState = nextStateRe.Match(item).Groups[1].Value[0];
                    stateRules[curState + curVal.ToString()] = act;
                    act = new Actions();
                }
            }

            for (int i = 0; i < 12994925; i++)
            {
                Step(fullState, stateRules[fullState.State + Tape[fullState.Index].ToString()]);
            }
            Console.WriteLine(Tape.Sum());
            Console.ReadLine();
        }

        static void Step(FullState state, Actions act)
        {
            Tape[state.Index] = act.Value;
            state.Index += act.Direction;
            state.State = act.NextState;
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
