namespace PIA_Zad5
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Graph
    {
        public int VerticesCount { get; set; }
        public List<Edge> Edges { get; set; } = new List<Edge>();

        public Graph(int verticesCount)
        {
            VerticesCount = verticesCount;
        }

        public void AddEdge(int src, int dest, int weight)
        {
            Edges.Add(new Edge(src, dest, weight));
        }

        public int[,] ToWeightMatrix()
        {
            int[,] matrix = new int[VerticesCount, VerticesCount];
            for (int i = 0; i < VerticesCount; i++)
                for (int j = 0; j < VerticesCount; j++)
                    matrix[i, j] = 0;

            foreach (var edge in Edges)
            {
                matrix[edge.Source, edge.Destination] = edge.Weight;
                matrix[edge.Destination, edge.Source] = edge.Weight;
            }
            return matrix;
        }

        public static Graph FromWeightMatrix(int[,] matrix)
        {
            int vertices = matrix.GetLength(0);
            Graph graph = new Graph(vertices);

            for (int i = 0; i < vertices; i++)
            {
                for (int j = i + 1; j < vertices; j++)
                {
                    if (matrix[i, j] > 0)
                    {
                        graph.AddEdge(i, j, matrix[i, j]);
                    }
                }
            }
            return graph;
        }

        public Graph KruskalMST()
        {
            Graph mst = new Graph(VerticesCount);
            Edges.Sort((e1, e2) => e1.Weight.CompareTo(e2.Weight));

            DisjointSet set = new DisjointSet(VerticesCount);

            foreach (var edge in Edges)
            {
                if (set.Find(edge.Source) != set.Find(edge.Destination))
                {
                    mst.AddEdge(edge.Source, edge.Destination, edge.Weight);
                    set.Union(edge.Source, edge.Destination);
                }
            }

            return mst;
        }

        public static Graph GenerateCase1(int n, int k)
        {
            Random random = new Random();
            Graph graph = new Graph(n);

            int centralNode = random.Next(0, n);
            for (int i = 0; i < n; i++)
            {
                if (i != centralNode)
                {
                    graph.AddEdge(centralNode, i, random.Next(1, 25));
                }
            }

            for (int i = 0; i < k; i++)
            {
                int src, dest;
                do
                {
                    src = random.Next(0, n);
                    dest = random.Next(0, n);
                } while (src == dest || graph.Edges.Any(e => (e.Source == src && e.Destination == dest) || (e.Source == dest && e.Destination == src)));

                graph.AddEdge(src, dest, random.Next(1, 25));
            }

            return graph;
        }

        public static Graph GenerateCase2(int n, int k)
        {
            Random random = new Random();
            Graph graph = new Graph(n);

            for (int i = 0; i < n - 1; i++)
            {
                graph.AddEdge(i, i + 1, random.Next(1, 25));
            }

            for (int i = 0; i < k; i++)
            {
                int src, dest;
                do
                {
                    src = random.Next(0, n);
                    dest = random.Next(0, n);
                } while (src == dest || graph.Edges.Any(e => (e.Source == src && e.Destination == dest) || (e.Source == dest && e.Destination == src)));

                graph.AddEdge(src, dest, random.Next(1, 25));
            }

            return graph;
        }
    }

    class Edge
    {
        public int Source { get; set; }
        public int Destination { get; set; }
        public int Weight { get; set; }

        public Edge(int source, int destination, int weight)
        {
            Source = source;
            Destination = destination;
            Weight = weight;
        }
    }

    class DisjointSet
    {
        private int[] Parent;
        private int[] Rank;

        public DisjointSet(int size)
        {
            Parent = new int[size];
            Rank = new int[size];
            for (int i = 0; i < size; i++)
                Parent[i] = i;
        }

        public int Find(int x)
        {
            if (Parent[x] != x)
                Parent[x] = Find(Parent[x]);
            return Parent[x];
        }

        public void Union(int x, int y)
        {
            int rootX = Find(x);
            int rootY = Find(y);

            if (rootX != rootY)
            {
                if (Rank[rootX] > Rank[rootY])
                    Parent[rootY] = rootX;
                else if (Rank[rootX] < Rank[rootY])
                    Parent[rootX] = rootY;
                else
                {
                    Parent[rootY] = rootX;
                    Rank[rootX]++;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Izaberite opciju:");
            Console.WriteLine("1 - Ucitavanje grafa iz fajla");
            Console.WriteLine("2 - Generisanje slucaja 1");
            Console.WriteLine("3 - Generisanje slucaja 2");
            int option = int.Parse(Console.ReadLine());

            Graph graph;

            if (option == 1)
            {
                string inputPath = "graph_input.txt";
                int[,] weightMatrix = LoadMatrixFromFile(inputPath);
                graph = Graph.FromWeightMatrix(weightMatrix);
            }
            else
            {
                int[] nValues = { 100, 1000, 10000, 100000 };
                int[] kFactors = { 2, 5, 10, 20, 33, 50 };

                foreach (int n in nValues)
                {
                    foreach (int kFactor in kFactors)
                    {
                        int k = kFactor * n;
                        Console.WriteLine($"Generisanje za N={n}, K={k}...");

                        if (option == 2)
                        {
                            graph = Graph.GenerateCase1(n, k);
                        }
                        else if (option == 3)
                        {
                            graph = Graph.GenerateCase2(n, k);
                        }
                        else
                        {
                            Console.WriteLine("Nepoznata opcija.");
                            return;
                        }

                        Graph mst = graph.KruskalMST();

                        string outputPath = $"mst_output_N{n}_K{k}.txt";
                        SaveMatrixToFile(mst.ToWeightMatrix(), outputPath);

                        Console.WriteLine($"Minimalno sprezno stablo za N={n}, K={k} je sacuvano u fajl: {outputPath}");
                    }
                }
                return;
            }

            Graph finalMST = graph.KruskalMST();
            string finalOutputPath = "mst_output.txt";
            SaveMatrixToFile(finalMST.ToWeightMatrix(), finalOutputPath);
            Console.WriteLine("Minimalno sprezno stablo je sacuvano u fajl: " + finalOutputPath);
        }

        static int[,] LoadMatrixFromFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            int rows = lines.Length;
            int cols = lines[0].Split(' ').Length;
            int[,] matrix = new int[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                var values = lines[i].Split(' ').Select(int.Parse).ToArray();
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = values[j];
                }
            }
            return matrix;
        }

        static void SaveMatrixToFile(int[,] matrix, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        writer.Write(matrix[i, j] + (j == matrix.GetLength(1) - 1 ? "" : " "));
                    }
                    writer.WriteLine();
                }
            }
        }
    }

}
