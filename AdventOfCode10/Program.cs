using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode10
{
    public class Program
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
        static List<int> Nums = new List<int>();
        static void Main(string[] args)
        {
            var input = readInput("input.txt").First();
            
            Console.WriteLine(GetHash(input));

            Console.ReadLine();
        }

        public static string GetHash(string input)
        {
            Nums = new List<int>();
            var inputList = input.Trim().Select(c => (int)c).ToList();
            inputList.AddRange(new List<int> { 17, 31, 73, 47, 23 });
            for (int i = 0; i < 256; i++)
            {
                Nums.Add(i);
            }
            var curPos = 0;
            var skip = 0;
            for (int i = 0; i < 64; i++)
            {
                foreach (var length in inputList)
                {
                    Transform(curPos, length);
                    curPos = (curPos + length + skip++) % Nums.Count;
                }
            }
            var denseHash = new List<int>();
            for (int i = 0; i < 16; i++)
            {
                denseHash.Add(GetDenseHashElement(Nums.GetRange(i * 16, 16)));
            }
            var hash = string.Empty;
            foreach (var item in denseHash)
            {
                hash += item.ToString("X2");
            }
            return hash.ToLower();

        }
        static int GetDenseHashElement(List<int> nums)
        {
            var result = nums[0];
            for (int i = 1; i < nums.Count; i++)
            {
                result ^= nums[i];
            }
            return result;
        }

        static void Transform(int curPos, int length)
        {
            var temp = new List<int>();
            for (int i = 0; i < length; i++)
            {
                temp.Add(Nums[(i + curPos) % Nums.Count]);
            }
            temp.Reverse();
            for (int i = 0; i < length; i++)
            {
                Nums[(i + curPos) % Nums.Count] = temp[i];
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
