﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode17
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
        const int input = 376;
        static List<int> Vals = new List<int> { 0 };
        static void Main(string[] args)
        {
            var current = 0;
            for (int i = 1; i < 2018; i++)
            {
                current = (current + input) % Vals.Count;
                Vals.Insert(++current, i);
            }
            Console.WriteLine(Vals[current + 1]);
            var valLength = 1;
            current = 0;
            var valueAfterZero = 0;
            for (int i = 1; i <= 50000000; i++)
            {
                current = (current + input) % valLength++;
                if (current++ == 0)
                {
                    valueAfterZero = i;
                }
            }
            Console.WriteLine(valueAfterZero);
            Console.ReadLine();
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
