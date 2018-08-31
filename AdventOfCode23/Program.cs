using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode23
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

        static int Index = 0;
        static int MulCounter = 0;
        static Dictionary<char, long> Registers = new Dictionary<char, long>();
        static void Main(string[] args)
        {
            var instructions = new List<string>();
            var inputList = readInput("input.txt");
            foreach (var item in inputList)
            {
                instructions.Add(item);
            }
         //   Registers['a'] = 1;
            while (Index >= 0 && Index < instructions.Count)
            {
                    ExecuteInstruction(instructions[Index]);
                  //  PringRegisters();
            }
            var b = 109300;
            var c = 126300;
            var h = 0;
            while (b <= c)
            {
                if (!IsPrime(b))
                {
                    h++;
                }
                b += 17;
            }
            Console.WriteLine(MulCounter);
            Console.WriteLine(h);
            Console.ReadLine();
        }
        static bool IsPrime(int num)
        {
            if (num == 1)
            {
                return false;
            }
            if (num == 2)
            {
                return true;
            }
            if (num % 2 == 0)
            {
                return false;
            }
            var boundary = (int)Math.Floor(Math.Sqrt(num));
            for (int i = 3; i < boundary; i += 2)
            {
                if (num % i == 0) return false;
            }
            return true;
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
                    if (!Registers.ContainsKey(instArr[2][0]))
                    {
                        Registers[instArr[2][0]] = 0;
                    }
                    amount = Registers[instArr[2][0]];
                }
            }
            if (command.Equals("jnz"))
            {
                if (register - '0' < 10)
                {
                    Jump(amount);
                }
                else
                {
                    if (!Registers.ContainsKey(register))
                    {
                        Registers[register] = 0;
                    }
                    if (Registers[register] != 0)
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
                    if (!Registers.ContainsKey(register))
                    {
                        Registers[register] = 0;
                    }
                }
                Command(command, register, amount);
            }
        }
        static void PringRegisters()
        {
            Console.Write($"Step: {Index} ");
            foreach (var key in Registers.Keys.OrderBy(k=> k))
            {                
                Console.Write($"{key}: {Registers[key]} ");                
            }
            Console.WriteLine();
        }
        static void Jump(long value)
        {            
            Index += (int)value;
        }

        static void Command(string command, char reg, long amount)
        {
            switch (command)
            {
                case "set":
                    Registers[reg] = amount;
                    break;
                case "sub":
                    Registers[reg] -= amount;
                    break;
                case "mul":
                    MulCounter++;
                    Registers[reg] *= amount;
                    break;
                //case "mod":
                //    Registers[reg] %= amount;
                //    break;
                //case "snd":
                //    if (reg - '0' < 10)
                //    {
                //        amount = reg - '0';
                //    }
                //    else
                //    {
                //        amount = Registers[reg];
                //    }
                //    if (CurrentProgram == 1)
                //    {
                //        Program1SendCounter++;
                //    }
                //    Queues[(CurrentProgram + 1) % Queues.Length].Enqueue(amount);
                //    Locks[(CurrentProgram + 1) % Locks.Length] = false;
                //    break;
                //case "rcv":
                //    if (Queues.Count > 0)
                //    {
                //        Registers[reg] = Queues.Dequeue();
                //    }
                //    else
                //    {
                //        Locks = true;
                //        CurrentProgram = (CurrentProgram + 1) % 2;
                //        return;
                //    }
                //    break;
            }
            Index++;
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
