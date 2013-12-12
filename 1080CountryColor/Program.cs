using System;
using System.Collections.Generic;
using System.Linq;

namespace _1080CountryColor
{
    internal enum Colors
    {
        Red,
        Blue
    }

    internal class Program
    {
        private const int MaxN = 100;
        private static int[][] matrix;

        private static int count;

        private static readonly Colors[] colores = new Colors[MaxN];
        private static readonly HashSet<int> colored = new HashSet<int>();
        private static readonly Queue<int> ToDFS = new Queue<int>();
        private static readonly HashSet<int> Visited = new HashSet<int>();

        private static Colors Revert(Colors color)
        {
            switch (color)
            {
                case Colors.Red:
                    return Colors.Blue;
                case Colors.Blue:
                    return Colors.Red;
                default:
                    return color;
            }
        }

        private static bool dfs(int startPoint)
        {
            Visited.Add(startPoint);
            var neiboColor = Revert(colores[startPoint]);

            for (int i = 1; i <= count; i++)
            {
                if (matrix[startPoint][i] == 1)
                {
                    ToDFS.Enqueue(i);
                    if (colored.Contains(i) && neiboColor != colores[i])
                    {
                        return false;
                    }
                    colores[i] = neiboColor;
                    colored.Add(i);
                }
            }

            int nextPoint;
            do
            {
                if (ToDFS.Count == 0)
                    return true;
                nextPoint = ToDFS.Dequeue();
            } while (Visited.Contains(nextPoint));

            return dfs(nextPoint);
        }

        private static void Read()
        {
            count = int.Parse(Console.ReadLine());
            for (int i = 1; i <= count; i++)
            {
                var vertexes = Console.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
                foreach (var vertex in vertexes)
                {
                    if (vertex == 0)
                        continue;

                    matrix[i][vertex] = 1;
                    matrix[vertex][i] = 1;
                }
            }
        }

        private static void Main(string[] args)
        {
            matrix = new int[MaxN + 1][];
            for (int i = 1; i <= MaxN; i++)
            {
                matrix[i] = new int[MaxN];
            }

            Read();

            colores[1] = Colors.Red;

            string result=string.Empty;

            if (dfs(1))
                for (int i = 1; i <= count; i++)
                {
                    result+=((int) colores[i]);
                }
            else
                result += (-1);

            Console.Write(result);

//            PrintState();
//
//            Console.ReadLine(); 
        }
    }
}