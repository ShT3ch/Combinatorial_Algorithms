using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _1022GenealogicTree
{
    class Program
    {
        private const int maxN = 101;

        private static int count;
        private static readonly Dictionary<int, HashSet<int>> childsOf = new Dictionary<int, HashSet<int>>(); 
        private static readonly Queue<int> forSortQueue = new Queue<int>();
        private static readonly int[] WeightOfPapa = new int[maxN];
        private static void Read()
        {
            count = int.Parse(Console.ReadLine());
            for (int i = 1; i <= count; i++)
            {
                var vertexes = Console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
                foreach (var vertex in vertexes)
                {
                    if (!childsOf.ContainsKey(i))
                        childsOf.Add(i, new HashSet<int>());
                    if (vertex == 0)
                        continue;
                    childsOf[i].Add(vertex);
                }
            }
        }

        private static IEnumerable<int> ZeroInput()
        {
            return Enumerable.Range(1, count).Where(dude => childsOf.Keys.All(papas => !childsOf[papas].Contains(dude)));
        }

        static void Main(string[] args)
        {
            Read();
            foreach (var papas in ZeroInput())
            {
                WeightOfPapa[papas] = 1;
                forSortQueue.Enqueue(papas);
            }

            while (forSortQueue.Count!=0)
            {
                var currentPapa = forSortQueue.Dequeue();
                foreach (var child in childsOf[currentPapa])
                {
                    WeightOfPapa[child] = Math.Max(WeightOfPapa[currentPapa] + 1, WeightOfPapa[child]);
                    forSortQueue.Enqueue(child);
                }
            }
            var ans = childsOf.Keys.ToList();
            ans.Sort((i, i1) => WeightOfPapa[i] - WeightOfPapa[i1]);

            foreach (var an in ans)
            {
                Console.Write("{0} ", an);
            }
//            Console.ReadLine();
        }

    }
}
