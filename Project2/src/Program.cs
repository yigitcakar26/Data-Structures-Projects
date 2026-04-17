using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

class EgeDeniziB
{
    public string FıshName { get; set; }
    public string OtherName { get; set; }
    public string Size { get; set; }
    public string Info { get; set; }
    public List<string> Medium { get; set; }
}

class Stack
{
    private int maxSize; 
    private EgeDeniziB[] stackArray; 
    private int top; 

    public Stack(int size)
    {
        maxSize = size;
        stackArray = new EgeDeniziB[maxSize];
        top = -1; 
    }

    public void Push(EgeDeniziB item) 
    {
        if (IsFull())
        {
            Console.WriteLine("No more elements can be added.");
            return;
        }
        stackArray[++top] = item;
    }

    public EgeDeniziB Pop() 
    {
        if (IsEmpty())
        {
            Console.WriteLine("The element cannot be removed.");
            return null;
        }
        return stackArray[top--];
    }

    public bool IsEmpty() 
    {
        return top == -1;
    }

    public bool IsFull() 
    {
        return top == maxSize - 1;
    }
}

class Queue<T>
{
    private int maxSize; 
    private T[] queueArray; 
    private int front; 
    private int rear; 
    private int nItems; 

    public Queue(int size)
    {
        maxSize = size;
        queueArray = new T[maxSize];
        front = 0; 
        rear = -1; 
        nItems = 0; 
    }

    public void Insert(T item) 
    {
        if (IsFull())
        {
            Console.WriteLine("No more elements can be added.");
            return;
        }
        if (rear == maxSize - 1) 
            rear = -1;
        queueArray[++rear] = item;
        nItems++;
    }

    public T Remove() 
    {
        if (IsEmpty())
        {
            Console.WriteLine("The element cannot be removed.");
            return default;
        }
        T temp = queueArray[front++];
        if (front == maxSize) 
            front = 0;
        nItems--;
        return temp;
    }

    public T PeekFront() 
    {
        if (IsEmpty())
        {
            Console.WriteLine("The Queue is Empty.");
            return default;
        }
        return queueArray[front];
    }

    public bool IsEmpty() 
    {
        return nItems == 0;
    }

    public bool IsFull() 
    {
        return nItems == maxSize;
    }
}

class PQ<T>
{
    private List<T> pq;
    private IComparer<T> comparer;

    public PQ()
    {
        pq = new List<T>();
        comparer = Comparer<T>.Default;
    }

    public PQ(IComparer<T> customComparer)
    {
        pq = new List<T>();
        comparer = customComparer;
    }

    public void Enque(T item)
    {
        pq.Add(item);
    }

    public T Deque()
    {
        if (IsEmpty())
        {
            throw new InvalidOperationException("The element cannot be removed.");
        }

        // Finding the smallest element
        T smallest = pq[0];
        foreach (var item in pq)
        {
            if (comparer.Compare(item, smallest) < 0)
            {
                smallest = item;
            }
        }

        pq.Remove(smallest); // Removing the smallest element from the list
        return smallest;
    }

    public bool IsEmpty()
    {
        return pq.Count == 0;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // File path
        string filePath = "FıshText.txt";

        // The seas
        string[] denizler = { "Ege", "Akdeniz", "Karadeniz", "Marmara", "Hint-Pasifik", "Kızıldeniz", "Endonezya" };

        // Create a Generic List 
        List<EgeDeniziB> AegeanFishes = new List<EgeDeniziB>();

        // Reading the file
        string[] lines = File.ReadAllLines(filePath);

        foreach (string line in lines)
        {
            string[] parts = line.Split(';');

            // If the number of parts is missing, complete by adding blank values.
            if (parts.Length < 4)
            {
                Array.Resize(ref parts, 4); 
            }
           
            string fish_name = parts[0].Split('(')[0].Trim();
            string other_name = parts[0].Contains("(") ? parts[0].Split('(')[1].Replace(")", "").Trim() : null;
            string size = parts.FirstOrDefault(p => p.Contains("cm") || p.Contains("metre") || p.Contains("kg"))?.Trim();

            // Skip the first line in the slug area and merge the remaining paragraphs.
            string info = string.Join(" ", parts.Skip(1).Where(p => !string.IsNullOrEmpty(p)).Select(p => p.Trim()));

            var medium = denizler.Where(d => line.IndexOf(d, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            // Create new EgeDeniziB object and add it to the list
            AegeanFishes.Add(new EgeDeniziB
            {
                FıshName = fish_name,
                OtherName = other_name,
                Size = size,
                Info = info,
                Medium = medium
            });
        }

        // Separating fish into groups of 10
        int group_size = 10;
        int total_group_size = (int)Math.Ceiling((double)AegeanFishes.Count / group_size);
        List<EgeDeniziB>[] group_lists = new List<EgeDeniziB>[total_group_size];

        for (int i = 0; i < total_group_size; i++)
        {
            group_lists[i] = AegeanFishes.Skip(i * group_size).Take(group_size).ToList();
        }


        // Menu
        bool cont = true;
        while (cont)
        {
            Console.WriteLine("A: EgeDeniziB class and objects definition");
            Console.WriteLine("B: 38 Element Generic List");
            Console.WriteLine("C: Separation into groups of 10");
            Console.WriteLine("D: Printing groups and number of fish with 'Other Name'");
            Console.WriteLine("2A: Processing EgeDeniziB Objects with Stack Structure");
            Console.WriteLine("2B: Processing EgeDeniziB Objects with Queue Structure");
            Console.WriteLine("3: Processing EgeDeniziB Objects with PQ Structure");
            Console.WriteLine("4A: Calculating customer processing times with FIFO Queue.");
            Console.WriteLine("4B: Calculating customer processing times with PQ.");
            Console.WriteLine("4C: Comparison of processing times between FIFO Queue and PQ.");
            Console.WriteLine("Press 'Q' to exit.");
            Console.Write("Your choice: ");
            string secim = Console.ReadLine().ToUpper();

            switch (secim)
            {
                case "A":
                    PrintingAll(AegeanFishes);
                    break;
                case "B":
                    Console.WriteLine($"Total Number of Fish: {AegeanFishes.Count}");
                    PrintingAll(AegeanFishes);
                    break;
                case "C":
                    PrintingC(group_lists);
                    break;
                case "D":
                    PrintingD(group_lists);
                    break;
                case "2A":
                    PrintingStack(AegeanFishes);
                    break;
                case "2B":
                    PrintingQueue(AegeanFishes);
                    break;
                case "3":
                    PrintingPQ(AegeanFishes);
                    break;
                case "4A":
                    process_time_queue();
                    break;
                case "4B":
                    process_time_PQ();
                    break;
                case "4C":
                    comparing_operations();
                    break;
                case "Q":
                    cont = false;
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
        
    }

    static void PrintingStack(List<EgeDeniziB> AegeanFishes)
    {
        // Create stack
        Stack stack_fish = new Stack(AegeanFishes.Count);

        // Adding element
        foreach (var fish in AegeanFishes)
        {
            stack_fish.Push(fish);
        }

        // Removing an element and printing it
        Console.WriteLine("Elements of Stack:");
        while (!stack_fish.IsEmpty())
        {
            var fish = stack_fish.Pop();
            if (fish != null)
            {
                Console.WriteLine($"Fish Name: {fish.FıshName}" + (fish.OtherName != null ? $" ({fish.OtherName})" : ""));
                Console.WriteLine($"Size: {fish.Size}");
                Console.WriteLine($"İnfo: {fish.Info}");
                Console.WriteLine($"Medium: {string.Join(", ", fish.Medium)}");
                Console.WriteLine(new string('-', 50));
            }
        }
    }

    static void PrintingQueue(List<EgeDeniziB> AegeanFishes)
    {
        // Create Generic Queue
        var queue_fish = new Queue<EgeDeniziB>(AegeanFishes.Count);

        // Adding element
        foreach (var fish in AegeanFishes)
        {
            queue_fish.Insert(fish);
        }

        // Removing an element and printing it
        Console.WriteLine("Elements of Queue:");
        while (!queue_fish.IsEmpty())
        {
            var fish = queue_fish.Remove();
            if (fish != null)
            {
                Console.WriteLine($"Fish Name: {fish.FıshName}" + (fish.OtherName != null ? $" ({fish.OtherName})" : ""));
                Console.WriteLine($"Size: {fish.Size}");
                Console.WriteLine($"İnfo: {fish.Info}");
                Console.WriteLine($"Medium: {string.Join(", ", fish.Medium)}");
                Console.WriteLine(new string('-', 50));
            }
        }
    }

    static void PrintingPQ(List<EgeDeniziB> AegeanFishes)
    {
        // Create Comparer
        var comparer = Comparer<EgeDeniziB>.Create((x, y) =>
            string.Compare(x.FıshName, y.FıshName, true, new System.Globalization.CultureInfo("tr-TR"))
        );

        // Create PQ
        PQ<EgeDeniziB> pq_fish = new PQ<EgeDeniziB>(comparer);

        // Adding element
        foreach (var fish in AegeanFishes)
        {
            pq_fish.Enque(fish);
        }

        // Delete and print elements in the queue in alphabetical order
        Console.WriteLine("Elemnts of PQ:");
        while (!pq_fish.IsEmpty())
        {
            var fish = pq_fish.Deque();
            if (fish != null)
            {
                Console.WriteLine($"Fish Name: {fish.FıshName}" + (fish.OtherName != null ? $" ({fish.OtherName})" : ""));
                Console.WriteLine($"Size: {fish.Size}");
                Console.WriteLine($"İnfo: {fish.Info}");
                Console.WriteLine($"Medium: {string.Join(", ", fish.Medium)}");
                Console.WriteLine(new string('-', 50));
            }
        }
    }

    static (double, double) process_time_queue()
    {
        // Costumer Product numbers
        int[] products = { 15, 1, 12, 8, 7, 4, 21, 3, 2, 6, 5, 9, 11 };
        double scanning_time = 3.3; //(second)

        // Creating Queue
        var queue = new Queue<int>(products.Length);

        // Adding the number of products to the queue
        foreach (var product in products)
        {
            queue.Insert(product);
        }

        double total_time = 0;

        int number_of_consumer = 1;

        while (!queue.IsEmpty())
        {
            int number_of_product = queue.Remove();
            double process_time = number_of_product * scanning_time;
            total_time += process_time;

            Console.WriteLine($"Consumer {number_of_consumer}: {number_of_product} product -> {process_time:F2} second");
            number_of_consumer++;
        }

        double avarage_time = total_time / products.Length;

        Console.WriteLine(new string('-', 50));
        Console.WriteLine($"Total Process Time: {total_time:F2} second");
        Console.WriteLine($"Avarage Process Time: {avarage_time:F2} second");
        Console.WriteLine(new string('-', 50));

        return (total_time, avarage_time); 
    }

    static (double, double) process_time_PQ()
    {
        // Costumer Product numbers
        int[] products = { 15, 1, 12, 8, 7, 4, 21, 3, 2, 6, 5, 9, 11 };
        double scanning_time = 3.3; // (second)

        // Creating PQ
        var comparer = Comparer<int>.Default; 
        var pq = new PQ<int>(comparer);

        // Adding the number of products to the pq
        foreach (var product in products)
        {
            pq.Enque(product);
        }

        double total_time = 0;

        int number_of_consumer = 1;

        while (!pq.IsEmpty())
        {
            int number_of_product = pq.Deque();
            double process_time = number_of_product * scanning_time;
            total_time += process_time;

            Console.WriteLine($"Consumer {number_of_consumer}: {number_of_product} Product -> {process_time:F2} second");
            number_of_consumer++;
        }

        double avarage_time = total_time / products.Length;

        Console.WriteLine(new string('-', 50));
        Console.WriteLine($"Total Process Time: {total_time:F2} second");
        Console.WriteLine($"Avarage Process Time: {avarage_time:F2} second");
        Console.WriteLine(new string('-', 50));

        return (total_time, avarage_time); 
    }

    static void comparing_operations()
    {        
        Console.WriteLine("FIFO Queue Processing Times:");
        var result_of_queue = process_time_queue();  

        Console.WriteLine();
        
        Console.WriteLine("PQ Processing Times:");
        var result_of_pq = process_time_PQ();

        // Comparison of results
        Console.WriteLine(new string('=', 50));
        Console.WriteLine("Comparison of FIFO and PQ Queues Results:");
        Console.WriteLine($"FIFO Total Time: {result_of_queue.Item1:F2} second, Avarge Time: {result_of_queue.Item2:F2} second");
        Console.WriteLine($"PQ   Total Time: {result_of_pq.Item1:F2} second, Avarge Time: {result_of_pq.Item2:F2} second");
        Console.WriteLine();
        Console.WriteLine("Process completion times are generally faster when using PQ because customers with low product counts are processed first.");
        Console.WriteLine("When FIFO Queue is used, it is a first-in, first-out fair queue system, but disadvantageous situations may arise in terms of time.");
        Console.WriteLine("However, in this case there is no difference because in both cases the required work and time conditions are the same, only their order is different.");
        Console.WriteLine(new string('=', 50));
    }

    static void PrintingAll(List<EgeDeniziB> fishes)
    {
        foreach (var fish in fishes)
        {
            Console.WriteLine($"Fish Name: {fish.FıshName}" + (fish.OtherName != null ? $" ({fish.OtherName})" : ""));
            Console.WriteLine($"Size: {fish.Size}");
            Console.WriteLine($"Info: {fish.Info}");
            Console.WriteLine($"Medium: {string.Join(", ", fish.Medium)}");
            Console.WriteLine(new string('-', 50));
        }
    }

    static void PrintingC(List<EgeDeniziB>[] group_lists)
    {
        for (int i = 0; i < group_lists.Length; i++)
        {
            Console.WriteLine($"\n*** Group {i + 1} ***\n");
            PrintingAll(group_lists[i]);
        }
    }

    static void PrintingD(List<EgeDeniziB>[] group_lists)
    {
        Console.WriteLine(new string('=', 50));
        Console.WriteLine("Number and Group Information of Fish with 'Other Name'");
        Console.WriteLine(new string('=', 50));

        // Printing all groups using PrintingC
        PrintingC(group_lists);

        // Additional information: Count of fishes with 'Other Name'
        for (int i = 0; i < group_lists.Length; i++)
        {
            int otherNamedFish = group_lists[i].Count(fish => !string.IsNullOrEmpty(fish.OtherName));
            Console.WriteLine($"Group {i + 1} Total Number of Fishes: {group_lists[i].Count}");
            Console.WriteLine($"Group {i + 1} Number of OtherNamedFishes: {otherNamedFish}");
            Console.WriteLine(new string('-', 50));
        }

        Console.WriteLine(new string('=', 50));
    }
}