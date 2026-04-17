using System;
using System.Collections.Generic;

class Graph
{
    public int V;
    public List<Tuple<int, int>>[] adjacenyList;

    public Graph(int vertices)
    {
        V = vertices;
        adjacenyList = new List<Tuple<int, int>>[V];
        for (int i = 0; i < V; i++)
            adjacenyList[i] = new List<Tuple<int, int>>();
    }

    public void AddEdge(int u, int v, int weight)
    {
        adjacenyList[u].Add(new Tuple<int, int>(v, weight));
        adjacenyList[v].Add(new Tuple<int, int>(u, weight));
    }

    public void Dijkstra(int source)
    {
        int[] distance = new int[V];
        bool[] shortestPathTree = new bool[V];

        for (int i = 0; i < V; i++)
        {
            distance[i] = int.MaxValue;
            shortestPathTree[i] = false;
        }

        // Distance from source to itself is always 0
        distance[source] = 0;

        // Priority Queue for selecting the vertex with the minimum distance
        SortedSet<Tuple<int, int>> PQ = new SortedSet<Tuple<int, int>>();
        PQ.Add(new Tuple<int, int>(0, source));

        while (PQ.Count > 0)
        {
            var node = PQ.Min;
            PQ.Remove(node);

            int u = node.Item2;
            shortestPathTree[u] = true;

            // Update distances for adjacent vertices
            foreach (var neighbor in adjacenyList[u])
            {
                int v = neighbor.Item1;
                int weight = neighbor.Item2;

                if (!shortestPathTree[v] && distance[u] != int.MaxValue && distance[u] + weight < distance[v])
                {
                    // Remove the old distance for v and add the new one
                    PQ.RemoveWhere(n => n.Item2 == v);
                    distance[v] = distance[u] + weight;
                    PQ.Add(new Tuple<int, int>(distance[v], v));
                }
            }
        }
        Print(distance);
    }

    public void Print(int[] dist)
    {
        Console.WriteLine("Vertex \t Distance from Source");
        for (int i = 0; i < V; i++)
            Console.WriteLine($"{i} \t {dist[i]}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Clear();

        Graph g = new Graph(9);

        g.AddEdge(0, 1, 4);
        g.AddEdge(0, 7, 8);
        g.AddEdge(1, 2, 8);
        g.AddEdge(1, 7, 11);
        g.AddEdge(2, 3, 7);
        g.AddEdge(2, 8, 2);
        g.AddEdge(2, 5, 4);
        g.AddEdge(3, 4, 9);
        g.AddEdge(3, 5, 14);
        g.AddEdge(4, 5, 10);
        g.AddEdge(5, 6, 2);
        g.AddEdge(6, 7, 1);
        g.AddEdge(6, 8, 6);
        g.AddEdge(7, 8, 7);

        int source = 0;
        Console.WriteLine("Dijkstra's Shortest Path Algorithm:");
        g.Dijkstra(source);
        Console.ReadLine();
    }
}