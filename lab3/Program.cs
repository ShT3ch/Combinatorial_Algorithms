using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace lab3
{

    public struct Edge
    {
        public int from;
        public int to;
        public int weight;
    }

    internal struct microWay
    {
        public Point start;
        public Point end;

        public override string ToString()
        {
            return String.Format("begin: {0}, end: {1}", start, end);
        }
    }

    internal struct Point
    {
        public double X;
        public double Y;

        public override string ToString()
        {
            return String.Format("X:{0,0000}; Y:{1,0000}", X, Y);
        }
    }

    internal class Program
    {
        private static readonly List<Point> mastaWay = new List<Point>();
        private static readonly List<Point> interestingPlaces = new List<Point>();

        private static int wayCount = 0;
        private static int interestCount = 0;

        private static readonly Dictionary<microWay, HashSet<Point>> reachebleInterestFrom = new Dictionary<microWay, HashSet<Point>>();

        private static bool IsReachebleBetween(Point place, Point begin, Point end, int maxSpeed)
        {
            return Distance(begin, place) + Distance(place, end) < Distance(begin, end)*maxSpeed;
        }

        private static double Distance(Point from, Point to)
        {
            return Math.Sqrt(Math.Pow(from.X - to.X, 2) + Math.Pow(from.Y - to.Y, 2));
        }

        private static void Read()
        {
            var counters = Console.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
            wayCount = counters.First();
            interestCount = counters.Last();

            var mastaways = Console.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            for (int i = 0; i < wayCount*2; i += 2)
            {
                mastaWay.Add(new Point {X = mastaways[i], Y = mastaways[i + 1]});
            }

            var interests = Console.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            for (int i = 0; i < interestCount*2; i += 2)
            {
                interestingPlaces.Add(new Point {X = interests[i], Y = interests[i + 1]});
            }
        }

        private static void Main(string[] args)
        {
            Read();
            PrintState();

            FillReacheble();
            PrintReacheble();
            BuildGraph();
            ShowGraph();
            StartSearch();

            Console.ReadLine();
        }

        private static void ShowGraph()
        {
            foreach (var i in X)
            {
                Console.WriteLine("way №{0}: {1}", i, vertex2way[i]);
            }

            foreach (var i in Y)
            {
                Console.WriteLine("interestPoint №{0}: {1}", i, vertex2interest[i]);
            }

            foreach (var edge in Edges)
            {
                if (vertex2interest.ContainsKey(edge.to) && vertex2way.ContainsKey(edge.from))
                    Console.WriteLine("Connected: {0}:{1} <=> {2}:{3}", edge.from, vertex2way[edge.from], edge.to, vertex2interest[edge.to]);
            }

        }

        private static void PrintState()
        {
            Console.WriteLine("Way: {0}", mastaWay.Aggregate("", (acc, elem) => acc += elem + ", "));
            Console.WriteLine("interests: {0}", interestingPlaces.Aggregate("", (acc, elem) => acc += elem + ", "));
        }

        private static void FillReacheble()
        {
            for (int i = 0; i < wayCount - 1; i++)
            {
                var beginP = mastaWay[i];
                var endP = mastaWay[i + 1];
                var way = new microWay {end = endP, start = beginP};

                if (!reachebleInterestFrom.ContainsKey(way))
                    reachebleInterestFrom.Add(way, new HashSet<Point>());
                (interestingPlaces.Where(interestPlace => IsReachebleBetween(interestPlace, beginP, endP, 2)))
                    .ToList()
                    .ForEach(
                        interestPlace => reachebleInterestFrom[way].Add(interestPlace)
                    );
            }
        }

        private static void PrintReacheble()
        {
            foreach (var way in reachebleInterestFrom.Keys)
            {
                Console.WriteLine(" Way: {0}", way);
                foreach (var point in reachebleInterestFrom[way])
                {
                    Console.WriteLine("intPlace: {0}", point);
                }
            }
        }

        public static void BuildGraph()
        {
            Edges.Clear();
            inPare.Clear();
            X.Clear();
            Y.Clear();

            _i = reachebleInterestFrom.Keys.Count;
            var counterFromX = 1;
            var CounterToY = 1;
            foreach (var way in reachebleInterestFrom.Keys)
            {
                X.Add(counterFromX);
                vertex2way.Add(counterFromX, way);

                reachebleInterestFrom[way].ToList().ForEach(reachebleInteresting =>
                {
                    var interestPlaceNumber = vertex2interest.Values.Any(place => place.Equals(reachebleInteresting)) ?
                        vertex2interest.Keys.First(key => vertex2interest[key].Equals(reachebleInteresting)) 
                        :
                        _i + CounterToY;
                    Edges.Add(new Edge { from = counterFromX, to = interestPlaceNumber });
                    Edges.Add(new Edge { from = interestPlaceNumber, to = counterFromX });
                    Y.Add(interestPlaceNumber);
                    if (!vertex2interest.ContainsKey(interestPlaceNumber))
                        vertex2interest.Add(interestPlaceNumber, reachebleInteresting);
                    CounterToY++;
                });
                counterFromX++;
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

        private static void StartSearch()
        {
            foreach (var i in X)
            {
                BeginKuhn(i);
            }
            DogsWayCount();
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
                    : inPare.ContainsValue(x) ?
                        inPare.First(pair => pair.Value == x).Key
                        : -1;
                ans += "\r\n";
            }
            return ans;
        }

        public static void DogsWayCount(string name = "out.txt")
        {
            Console.WriteLine(inPare.Count*3);
//            file.Flush();
        }

        public static Dictionary<int, microWay> vertex2way = new Dictionary<int, microWay>();
        public static Dictionary<int, Point> vertex2interest = new Dictionary<int, Point>();

        public static HashSet<Edge> Edges = new HashSet<Edge>();

        public static HashSet<int> X = new HashSet<int>();
        public static HashSet<int> Y = new HashSet<int>();


        public static Dictionary<int, int> inPare = new Dictionary<int, int>();
        public static HashSet<int> visited = new HashSet<int>();
        private static int _i;
    }


}
