using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode21
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

        class Rule
        {
            public char[,] Source { get; set; }
            public char[,] Target { get; set; }
            public int SrcDimension { get; set; }
            public Rule(int dim)
            {
                SrcDimension = dim;
                Source = new char[dim, dim];
                Target = new char[dim + 1, dim + 1];
            }
        }

        static List<Rule> Rules = new List<Rule>();
        static List<char[,]> GridHistory = new List<char[,]>();

        static void Main(string[] args)
        {
            var currentGrid = new char[,] { { '.', '#', '.' }, { '.', '.', '#' }, { '#', '#', '#' } };
            GridHistory.Add(currentGrid);
            List<string> inputList = readInput("input.txt");
            foreach (var item in inputList)
            {
                Rules.Add(ParseRule(item));
            }
            for (int i = 0; i < 5; i++)
            {
                var dim = currentGrid.GetLength(0);
                int tiles;
                int newDim;
                int partDim;
                if (dim % 2 == 0)
                {
                    newDim = dim / 2 * 3;
                    tiles = dim / 2;
                    partDim = 2;
                }
                else
                {
                    newDim = dim / 3 * 4;
                    tiles = dim / 3;
                    partDim = 3;
                }
                var nextGrid = new char[newDim, newDim];
                for (int j = 0; j < tiles; j++)
                {
                    for (int k = 0; k < tiles; k++)
                    {
                        var gridPart = new char[partDim, partDim];
                        var a = j * partDim;
                        var b = k * partDim;
                        for (int x = 0; x < partDim; x++)
                        {
                            for (int y = 0; y < partDim; y++)
                            {
                                gridPart[x, y] = currentGrid[a + x, b + y];
                            }
                        }
                        var newGridPart = Transform(gridPart);
                        for (int x = 0; x < partDim + 1; x++)
                        {
                            for (int y = 0; y < partDim + 1; y++)
                            {
                                nextGrid[a + j + x, b + k + y] = newGridPart[x, y];
                            }
                        }
                    }
                }
                GridHistory.Add(nextGrid);
                currentGrid = nextGrid;
            }
            var sum = 0;
            var curDim = currentGrid.GetLength(0);
            for (int i = 0; i < curDim; i++)
            {
                for (int j = 0; j < curDim; j++)
                {
                    if (currentGrid[i, j] == '#')
                    {
                        sum++;
                    }
                }
            }
            Console.WriteLine(sum);
            Console.ReadLine();
        }

        static Rule ParseRule(string input)
        {
            var ruleArr = input.Split(new string[] { " => " }, StringSplitOptions.RemoveEmptyEntries);
            var srcArr = ruleArr[0].Split(new char[] { '/' });
            var tgtArr = ruleArr[1].Split(new char[] { '/' });
            var rule = new Rule(srcArr.Length);
            for (int i = 0; i < rule.SrcDimension; i++)
            {
                for (int j = 0; j < rule.SrcDimension; j++)
                {
                    rule.Source[i, j] = srcArr[i][j];
                }
            }
            for (int i = 0; i <= rule.SrcDimension; i++)
            {
                for (int j = 0; j <= rule.SrcDimension; j++)
                {
                    rule.Target[i, j] = tgtArr[i][j];
                }
            }
            return rule;
        }

        static char[,] Transform(char[,] source, int rotateCount = 0)
        {
            var srcDim = source.GetLength(0);
            foreach (var rule in Rules.Where(r => r.SrcDimension == srcDim))
            {
                if (GridsEquals(source, rule.Source))
                {
                    return rule.Target;
                }
            }
            if (rotateCount < 3)
            {
                return Transform(Rotate(source), rotateCount + 1);
            }
            else
            {
                return Transform(Flip(Rotate(source)), 0);
            }
        }

        static char[,] Rotate(char[,] grid)
        {
            var dim = grid.GetLength(0);
            var result = new char[dim, dim];
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    result[i, j] = grid[dim - 1 - j, i];
                }
            }
            return result;
        }

        static char[,] Flip(char[,] grid)
        {
            var dim = grid.GetLength(0);
            var result = new char[dim, dim];
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    result[i, j] = grid[i, dim - 1 - j];
                }
            }
            return result;
        }

        static bool GridsEquals(char[,] grid1, char[,] grid2)
        {
            var dim = grid1.GetLength(0);
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    if (grid1[i, j] != grid2[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
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
