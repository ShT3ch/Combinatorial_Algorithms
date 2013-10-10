using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace lab2
{
    internal struct Edge
    {
        public int from;
        public int to;
        public int weight;
    }

    internal class lab2
    {
        public static void Read(string filename = "in.txt")
        {
            Edges.Clear();
            inPare.Clear();
            X.Clear();
            Y.Clear();

            var file = new StreamReader(File.Open(filename, FileMode.Open));
            var head = file.ReadLine().Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
            _i = head.First();
            var l = head.Last();
            for (int i = 1; i <= _i; i++)
            {
                var line = file.ReadLine().Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                line.RemoveAt(line.Count-1);
                X.Add(i);
                line.ForEach(i1 =>
                    {
                        Edges.Add(new Edge {from = i, to = _i+i1});
                        Edges.Add(new Edge {from =_i+ i1, to = i});
                        Y.Add(_i+i1);
                    });
            }
        }

        public static bool BeginKuhn(int beginVertex)
        {
            visited.Clear();
            return kuhn(beginVertex);
        }

        private static bool kuhn(int beginVertex)
        {
            if (visited.Contains(beginVertex)) return false;
            visited.Add(beginVertex);
            var edgesToTest = Edges.Where(edge => edge.from == beginVertex).ToList();
            foreach (var edge in edgesToTest)
            {
                var to = edge.to; 
                if (!inPare.ContainsKey(to) || kuhn(inPare[to]))
                {
                    inPare[to] = beginVertex;
                    return true;
                }
            }
            return false;
        }

        private static void Main(string[] args)
        {
            Read();
            foreach (var i in X)
            {
                if (!BeginKuhn(i))
                {
                    ErrorWrite(i);
                    return;
                }
            }
            Write();
        }

        public static string GetStateString()
        {
            var ans = "";
            foreach (var x in X)
            {
                ans += x;
                ans += " : ";
                ans += inPare.ContainsKey(x) ? 
                    inPare[x] 
                    :
                    inPare.ContainsValue(x) ? 
                        inPare.First(pair => pair.Value == x).Key 
                        :
                        -1;
                ans += "\r\n";
            }
            return ans;
        }

        public static void ErrorWrite(int i, string name = "out.txt")
        {
            var file = new StreamWriter(File.OpenWrite(name));

            file.WriteLine("N");

            file.WriteLine(i);
            file.Flush();
        }

        public static void Write(string name = "out.txt")
        {//
            var file = new StreamWriter(File.Create(name));

            file.WriteLine("Y");

            var ans = "";
            for (int i = 1; i <= X.Count; i++)
            {
                ans += (inPare.First(i1 => i1.Value.Equals(i)).Key-_i) + " ";
            }

            file.Write(ans);
            file.Flush();
        }

        public static HashSet<Edge> Edges = new HashSet<Edge>();

        public static HashSet<int> X = new HashSet<int>();
        public static HashSet<int> Y = new HashSet<int>();


        public static Dictionary<int, int> inPare = new Dictionary<int, int>();
        public static HashSet<int> visited = new HashSet<int>();
        private static int _i;
    }
}