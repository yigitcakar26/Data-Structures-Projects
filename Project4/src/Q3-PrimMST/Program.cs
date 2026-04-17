using System;

public class Graph
{
    public int V;
    public int[,] adjMatrix;

    public Graph(int vertices)
    {
        V = vertices;
        adjMatrix = new int[V, V];

        for (int i = 0; i < V; i++)
        {
            for (int j = 0; j < V; j++)
            {
                adjMatrix[i, j] = -1; // No edge between vertices.
            }
        }
    }

    public void AddEdge(int u, int v, int weight)
    {
        adjMatrix[u, v] = weight;
        adjMatrix[v, u] = weight;
    }

    public void Prim()
    {
        int[] key = new int[V];

        bool[] visited = new bool[V];

        for (int i = 0; i < V; i++)
        {
            key[i] = int.MaxValue;
            visited[i] = false;
        }

        key[0] = 0;

        int[] parent = new int[V];
        parent[0] = -1;

        for (int count = 0; count < V - 1; count++)
        {
            int u = findMinKey(key, visited);
            visited[u] = true;

            for (int v = 0; v < V; v++)
            {
                if (adjMatrix[u, v] != -1 && !visited[v] && adjMatrix[u, v] < key[v])
                {
                    key[v] = adjMatrix[u, v];
                    parent[v] = u;
                }
            }
        }
        PrintMST(parent);
    }

    private int findMinKey(int[] key, bool[] inMST)
    {
        int min = int.MaxValue;
        int minIndex = -1;

        for (int v = 0; v < V; v++)
        {
            if (!inMST[v] && key[v] < min)
            {
                min = key[v];
                minIndex = v;
            }
        }
        return minIndex;
    }

    private void PrintMST(int[] parent)
    {
        Console.WriteLine("Edge \tWeight");
        for (int i = 1; i < V; i++)
        {
            Console.WriteLine(parent[i] + " - " + i + "\t" + adjMatrix[i, parent[i]]);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Clear();

        Graph g = new Graph(5);

        // Add edges (u, v, weight)
        g.AddEdge(0, 1, 2);
        g.AddEdge(0, 3, 6);
        g.AddEdge(1, 2, 3);
        g.AddEdge(1, 3, 8);
        g.AddEdge(1, 4, 5);
        g.AddEdge(2, 4, 7);
        g.AddEdge(3, 4, 9);

        Console.WriteLine("Prim's Minimum Spanning Tree (without priority queue):");
        g.Prim();
        Console.ReadLine(); 
    }
}
