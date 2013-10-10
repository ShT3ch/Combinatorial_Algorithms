using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1273_После_Вчерашнего
{
    class Program
    {

        static void Read()
        {
            var line = Console.ReadLine();
            var count = int.Parse(line);

            if (count == 0)
                return;

            for (int i = 0; i < count; i++)
            {
                var items = Console.ReadLine().Split(new []{" "},StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
                from2.Add(items.First(),items.Last());
                
                sequenceLeft.Add(items.First());
            }
            
            sequenceLeft.Sort((i, i1) => i-i1);
            sequenceRight = sequenceLeft.Select(litem => from2[litem]).ToList();

        }

        static void Main(string[] args)
        {
            Read();

            if (sequenceLeft.Count == 0)
            {
                Console.WriteLine(0);
                return;
            }

            d[0] = 1;

            for (int i = 0; i < sequenceRight.Count; i++)
            {
                var newD = 1;
                for (int j = 0; j < i; j++)
                {
                    if (sequenceRight[j] < sequenceRight[i])
                    {
                        newD = Math.Max(d[j] + 1, newD);
                    }
                }
                d[i] = newD;
            }
            Console.WriteLine(sequenceLeft.Count-d.Max());
        }

        public static Dictionary<int,int> from2 = new Dictionary<int, int>();
        public static List<int> sequenceRight = new List<int>(101);
        public static List<int> sequenceLeft = new List<int>(101);
        public static int[] d = new int[101];
    }
}
