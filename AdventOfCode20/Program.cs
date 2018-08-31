using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Priority_Queue;

namespace AdventOfCode20
{
    class Program
    {
        class Particle
        {
            public Point Position { get; set; }
            public Point Velocity { get; set; }
            public Point Acceleration { get; set; }
            public Particle(Point p, Point v, Point a)
            {
                Position = p;
                Velocity = v;
                Acceleration = a;
            }
        }
        static List<Particle> Particles = new List<Particle>();
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
                Particles.Add(ParseParticle(item));
            }
            //var slowestParticle = Particles.First(p => p.Acceleration.Distance() == Particles.Select(p2 => p2.Acceleration.Distance()).Min());
            //Console.WriteLine(Particles.IndexOf(slowestParticle));
            Console.WriteLine(FindClosestToZero(1000000));
            for (int i = 0; i < 100; i++)
            {
                RenderNextFrame();
                Collapse();
            }
            Console.WriteLine(Particles.Count);
            Console.ReadLine();
        }

        static Particle ParseParticle(string input)
        {
            var regexp = new Regex(@"p=<(?<position>-?\d+,-?\d+,-?\d+)>, v=<(?<velocity>-?\d+,-?\d+,-?\d+)>, a=<(?<acceleration>-?\d+,-?\d+,-?\d+)>");
            var groups = regexp.Match(input).Groups;
            var position = groups["position"].Value.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();
            var velocity = groups["velocity"].Value.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();
            var acceleration = groups["acceleration"].Value.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();
            var p = new Point(position[0], position[1], position[2]);
            var v = new Point(velocity[0], velocity[1], velocity[2]);
            var a = new Point(acceleration[0], acceleration[1], acceleration[2]);
            return new Particle(p, v, a);
        }

        static void RenderNextFrame()
        {
            foreach (var particle in Particles)
            {
                var v = new Point(particle.Velocity.X + particle.Acceleration.X,
                    particle.Velocity.Y + particle.Acceleration.Y,
                    particle.Velocity.Z + particle.Acceleration.Z);
                var p = new Point(particle.Position.X + v.X,
                    particle.Position.Y + v.Y,
                    particle.Position.Z + v.Z);
                particle.Position = p;
                particle.Velocity = v;
            }
        }

        static void Collapse()
        {
            var collided = Particles.GroupBy(p => p.Position).Where(g => g.Count() > 1).SelectMany(gr => gr);
            foreach (var particle in collided)
            {
                Particles.Remove(particle);
            }
        }

        static int FindClosestToZero(int time)
        {
            var min = int.MaxValue;
            var minIndex = -1;
            for (int i = 0; i < Particles.Count; i++)
            {
                var particle = Particles[i];
                var x = particle.Position.X + MeanVelocity(particle.Velocity.X, particle.Acceleration.X, time);
                var y = particle.Position.Y + MeanVelocity(particle.Velocity.Y, particle.Acceleration.Y, time);
                var z = particle.Position.Z + MeanVelocity(particle.Velocity.Z, particle.Acceleration.Z, time);
                var distance = (new Point(x, y, z)).Distance();
                if (distance < min)
                {
                    min = distance;
                    minIndex = i;
                }
            }
            return minIndex;
        }

        static int MeanVelocity(int v, int a, int time)
        {
            return v + a * (time + 1) / 2;
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
            public int Z { get; set; }

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public Point(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public override string ToString()
            {
                return string.Format("{0}:{1}:{2}", X, Y, Z);
            }

            public override bool Equals(object obj)
            {
                var pointObj = obj as Point;
                return (X.Equals(pointObj.X) && Y.Equals(pointObj.Y) && Z.Equals(pointObj.Z));
            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }

            public int Distance()
            {
                return Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
            }
        }

        #endregion

    }
}
