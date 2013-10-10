using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_SecondEdition
{
    class lab1
    {
        struct Edge
        {
            public int from;
            public int to;
            public int weight;
        }

        static readonly Queue<Edge> edges = new Queue<Edge>();
        static readonly HashSet<Edge> newEdges = new HashSet<Edge>(); 
        static readonly Dictionary<int, int> familyOfVertex = new Dictionary<int, int>();         //Vertex2Family
        static readonly Dictionary<int, HashSet<int>> vertexesFamily = new Dictionary<int, HashSet<int>>();//Family2Vertexes

        static void Read()
        {
            var input = new StreamReader(new FileStream("in.txt", FileMode.Open, FileAccess.Read));

            var number = int.Parse(input.ReadLine());

            var notSortedEdges = new List<Edge>();

            var tempList = new List<int>();

            while (!input.EndOfStream)
            {
                var line = input.ReadLine();
                tempList.AddRange(line.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
            }

            var arrayStorage = tempList.ToArray();

            var CPointer = 0;
            var fPointer = arrayStorage[CPointer++];
            var numOfVertexes = fPointer;
            var sPointer = arrayStorage[CPointer++];
            var currentVertex = 1;

            while (fPointer<number)
            {
                familyOfVertex.Add(currentVertex, currentVertex);
                vertexesFamily.Add(currentVertex, new HashSet<int> { currentVertex });

                var range = Enumerable.Range(fPointer-1, sPointer - fPointer);
                
                var last = new Edge();
                foreach (var i in range)
                {
                    if ((i+numOfVertexes)%2 == 1)
                    {
                        last = (new Edge {from = currentVertex, to = arrayStorage[i]});
                    }
                    else
                    {
                        last.weight = arrayStorage[i];
                        notSortedEdges.Add(last);
                    }
                }
                fPointer = sPointer;
                sPointer = arrayStorage[CPointer++];
                currentVertex++;
            }

            notSortedEdges.Sort((edge, edge1) => edge.weight-edge1.weight);
            notSortedEdges.ForEach(edges.Enqueue);

//            counter++;
//            familyOfVertex.Add(counter, counter);
//            vertexesFamily.Add(counter, new HashSet<int> { counter });

        }

        static void Main(string[] args)
        {
            var count_AddedEdges = 0;

            Read();

            while (edges.Count != 0 && familyOfVertex.Count != count_AddedEdges)
            {
                var currentEdge = edges.Dequeue();
                    if (familyOfVertex[currentEdge.from] != familyOfVertex[currentEdge.to])
                    {
                        count_AddedEdges++;
                        newEdges.Add(currentEdge);

                        HashSet<int> familyToMove;
                        int newFamily;
                        int oldFamily;

                        if ((vertexesFamily[familyOfVertex[currentEdge.from]].Count > vertexesFamily[familyOfVertex[currentEdge.to]].Count))
                        {
                            familyToMove = vertexesFamily[familyOfVertex[currentEdge.to]];
                            oldFamily = familyOfVertex[currentEdge.to];
                            newFamily = familyOfVertex[currentEdge.from];
                        }
                        else
                        {
                            familyToMove = vertexesFamily[familyOfVertex[currentEdge.from]];
                            oldFamily = familyOfVertex[currentEdge.from];
                            newFamily = familyOfVertex[currentEdge.to];
                        }

                        foreach (var movingVertex in familyToMove)
                        {
                            familyOfVertex[movingVertex] = newFamily;
                            vertexesFamily[newFamily].Add(movingVertex);
                        }

                        vertexesFamily.Remove(oldFamily);
                    }
                }
            
            Console.WriteLine(newEdges.Aggregate(0,(acc, elem)=>acc+=elem.weight));
            Writer(vertexesFamily.First().Value, newEdges);
        }

        private static void Writer(IEnumerable<int> vertexes, HashSet<Edge> edges)
        {
            var output = new StreamWriter(new FileStream("out.txt", FileMode.Create));

            var toAns = new Dictionary<int, string>();

            foreach (var vertex in vertexes)
            {
                var neibos = edges.Where(edge => edge.from.Equals(vertex)).Select(edge => edge.to).
                                   Concat(
                                       edges.Where(edge => edge.to.Equals(vertex)).Select(edge => edge.from)
                    ).ToList();
                neibos.Sort();
                toAns.Add(vertex, String.Join(" ", neibos)+ " 0");
               
            }

            for (int i = 1; i < vertexes.Count()+1; i++)
            {
                output.WriteLine(toAns[i]);
            }

            output.Write(newEdges.Aggregate(0, (acc, elem) => acc += elem.weight));
            output.Flush();
        }
    }
}
