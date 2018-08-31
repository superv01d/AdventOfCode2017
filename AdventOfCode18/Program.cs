using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode18
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
        static Dictionary<char, long>[] Registers = new Dictionary<char, long>[] { new Dictionary<char, long> { { 'p', 0 } }, new Dictionary<char, long> { { 'p', 1 } } };
        static long[] Index = new long[] { 0, 0 };
        //static long LastSoundFrequency = 0;
        static int CurrentProgram = 0;
        static Queue<long>[] Queues = new Queue<long>[] { new Queue<long>(), new Queue<long>() };
        static bool[] Locks = new bool[] { false, false };
        static int Program1SendCounter = 0;
        static void Main(string[] args)
        {
            var instructions = new List<string>();
            var inputList = readInput("input.txt");
            foreach (var item in inputList)
            {
                instructions.Add(item);
            }
            while (!Locks[0] || !Locks[1])
            {
                if (Index[CurrentProgram] >= 0 && Index[CurrentProgram] < instructions.Count)
                {
                    ExecuteInstruction(instructions[(int)Index[CurrentProgram]]);
                }
                else
                {
                    Locks[CurrentProgram] = true;
                    CurrentProgram = (CurrentProgram + 1) % 2;
                }                
            }
            Console.WriteLine(Program1SendCounter);
            Console.ReadLine();
        }

        static void ExecuteInstruction(string instruction)
        {
            var instArr = instruction.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var command = instArr[0];
            var register = instArr[1][0];
            long amount = 0;
            if (instArr.Length > 2)
            {
                if (!long.TryParse(instArr[2], out amount))
                {
                    if (!Registers[CurrentProgram].ContainsKey(instArr[2][0]))
                    {
                        Registers[CurrentProgram][instArr[2][0]] = 0;
                    }
                    amount = Registers[CurrentProgram][instArr[2][0]];
                }
            }            
            if (command.Equals("jgz"))
            {
                if (register - '0' < 10)
                {
                    Jump(amount);
                }
                else
                {
                    if (!Registers[CurrentProgram].ContainsKey(register))
                    {
                        Registers[CurrentProgram][register] = 0;
                    }
                    if (Registers[CurrentProgram][register] > 0)
                    {
                        Jump(amount);
                    }
                    else
                    {
                        Jump(1);
                    }
                }
            }
            else
            {
                if (register - '0' >= 10)
                {
                    if (!Registers[CurrentProgram].ContainsKey(register))
                    {
                        Registers[CurrentProgram][register] = 0;
                    }
                }
                Command(command, register, amount);
            }            
        }

        static void Jump(long value)
        {
            Index[CurrentProgram] += value;
        }

        static void Command(string command, char reg, long amount)
        {
            switch (command)
            {
                case "set":
                    Registers[CurrentProgram][reg] = amount;
                    break;
                case "add":
                    Registers[CurrentProgram][reg] += amount;
                    break;
                case "mul":
                    Registers[CurrentProgram][reg] *= amount;
                    break;
                case "mod":
                    Registers[CurrentProgram][reg] %= amount;
                    break;
                case "snd":
                    if (reg - '0' < 10)
                    {
                        amount = reg - '0';
                    }
                    else
                    {
                        amount = Registers[CurrentProgram][reg];
                    }
                    if (CurrentProgram == 1)
                    {
                        Program1SendCounter++;
                    }
                    Queues[(CurrentProgram + 1) % Queues.Length].Enqueue(amount);
                    Locks[(CurrentProgram + 1) % Locks.Length] = false;
                    break;
                case "rcv":
                    if (Queues[CurrentProgram].Count > 0)
                    {
                        Registers[CurrentProgram][reg] = Queues[CurrentProgram].Dequeue();
                    }
                    else
                    {
                        Locks[CurrentProgram] = true;

                        CurrentProgram = (CurrentProgram + 1) % 2;                        
                        return;                        
                    }
                    break;
            }
            Index[CurrentProgram]++;
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
