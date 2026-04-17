using System;
using System.Collections.Generic;

public class Graph
{
    public int V;
    public List<int>[] adjList;

    public Graph(int vertices)
    {
        V = vertices;
        adjList = new List<int>[V];
        for (int i = 0; i < V; i++)
        {
            adjList[i] = new List<int>();
        }
    }

    public void AddEdge(int u, int v)
    {
        adjList[u].Add(v);
        adjList[v].Add(u);
    }

    public void BFT(int startVertex)
    {
        bool[] visited = new bool[V];

        Queue<int> queue = new Queue<int>();

        visited[startVertex] = true;
        queue.Enqueue(startVertex);

        while (queue.Count > 0)
        {
            int currentVertex = queue.Dequeue();
            Console.Write(currentVertex + " ");

            foreach (int neighbor in adjList[currentVertex])
            {
                if (!visited[neighbor])
                {
                    visited[neighbor] = true;
                    queue.Enqueue(neighbor);
                }
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.White;
        Console.Clear();

        Graph g = new Graph(6);

        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(1, 3);
        g.AddEdge(1, 4);
        g.AddEdge(2, 5);

        Console.WriteLine("Breadth-First Traversal (BFT) starting from vertex 0:");
        g.BFT(0);
        Console.ReadLine();
    }
}