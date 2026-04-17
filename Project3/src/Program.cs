using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Diagnostics;

namespace project2
{
    // Node class representing each fish in the binary search tree
    public class FishNode
    {
        public EgeDeniziB Balik;   // Each node stores a fish object (data)
        public FishNode Left;      // Left child
        public FishNode Right;     // Right child

        public FishNode(EgeDeniziB balik)
        {
            Balik = balik;
            Left = null;
            Right = null;
        }
    }

    // Binary Search Tree (BST) class to store fish
    public class FishTree
    {
        public FishNode Root;

        public FishTree()
        {
            Root = null;
        }

        // Insert method to add fish into the tree (sorted by Balik_Adi)
        public void Insert(EgeDeniziB balik)
        {
            FishNode newNode = new FishNode(balik);
            if (Root == null)
            {
                Root = newNode; // If the tree is empty, insert as root
            }
            else
            {
                InsertRec(Root, newNode); // Otherwise, insert recursively
            }
        }

        // Recursive helper method to insert a node into the tree
        private void InsertRec(FishNode root, FishNode newNode)
        {
            // With string.Compare(...), comparing Balik_Adi according to CurrentCulture
            int comparison = string.Compare(
                root.Balik.Balik_Adi,
                newNode.Balik.Balik_Adi,
                StringComparison.CurrentCulture);

            if (comparison > 0)
            {
                if (root.Left == null)
                {
                    root.Left = newNode;
                }
                else
                {
                    InsertRec(root.Left, newNode);
                }
            }
            else
            {
                if (root.Right == null)
                {
                    root.Right = newNode;
                }
                else
                {
                    InsertRec(root.Right, newNode);
                }
            }
        }
    }

    // Class to hold fish information and its word tree
    public class EgeDeniziB
    {
        public string Balik_Adi { get; set; }
        public string Bilgi { get; set; }
        public WordsTree KelimelerAgaci { get; set; }

        public EgeDeniziB()
        {
            Bilgi = "";
            KelimelerAgaci = new WordsTree();
        }
    }

    // Words Tree for each fish's information
    public class WordsTree
    {
        public WordNode Root;

        public WordsTree()
        {
            Root = null;
        }

        public void Insert(string word)
        {
            WordNode newNode = new WordNode(word);
            if (Root == null)
            {
                Root = newNode;
            }
            else
            {
                InsertRec(Root, newNode);
            }
        }

        private void InsertRec(WordNode root, WordNode newNode)
        {
            int comparison = string.Compare(root.Word, newNode.Word, StringComparison.CurrentCulture);
            if (comparison > 0)
            {
                if (root.Left == null)
                {
                    root.Left = newNode;
                }
                else
                {
                    InsertRec(root.Left, newNode);
                }
            }
            else
            {
                if (root.Right == null)
                {
                    root.Right = newNode;
                }
                else
                {
                    InsertRec(root.Right, newNode);
                }
            }
        }

        public int GetDepth()
        {
            return GetDepthRec(Root);
        }
        private int GetDepthRec(WordNode node)
        {
            if (node == null)
                return 0;
            int leftDepth = GetDepthRec(node.Left);
            int rightDepth = GetDepthRec(node.Right);
            return 1 + Math.Max(leftDepth, rightDepth);
        }

        public int GetNodeCount()
        {
            return GetNodeCountRec(Root);
        }
        private int GetNodeCountRec(WordNode node)
        {
            if (node == null)
                return 0;
            return 1 + GetNodeCountRec(node.Left) + GetNodeCountRec(node.Right);
        }

        public double GetBalancedDepth()
        {
            int totalNodes = GetNodeCount();
            if (totalNodes == 0)
                return 0;
            return Math.Ceiling(Math.Log(totalNodes + 1, 2));
        }
    }

    // Node class representing each word in the words tree
    public class WordNode
    {
        public string Word;
        public WordNode Left;
        public WordNode Right;

        public WordNode(string word)
        {
            Word = word;
            Left = null;
            Right = null;
        }
    }

    // MaxHeap class
    public class MaxHeap
    {
        private EgeDeniziB[] heapArray;  // EgeDeniziB nesnelerini tutacak heap array
        private int size;
        private int maxSize;

        // Constructor to initialize the max-heap
        public MaxHeap(int maxSize)
        {
            this.maxSize = maxSize;
            this.size = 0;
            this.heapArray = new EgeDeniziB[maxSize];
        }

        // Method to insert a new EgeDeniziB object into the heap
        public void Insert(EgeDeniziB newBalik)
        {
            if (size >= maxSize)
            {
                Console.WriteLine("Heap is full");
                return;
            }

            heapArray[size] = newBalik;  // Insert the new fish at the end
            trickleUp(size);  // Restore heap property by moving the node up
            size++;  // Increment size
        }

        // Method to remove the maximum element (root node) from the heap
        public EgeDeniziB remove()
        {
            if (size == 0)
            {
                Console.WriteLine("Heap is empty");
                return null;
            }

            EgeDeniziB root = heapArray[0];  // The root node is the maximum
            heapArray[0] = heapArray[size - 1];  // Replace the root with the last element
            size--;  // Decrease the size of the heap
            trickleDown(0);  // Restore heap property by moving the new root down
            return root;
        }

        // Method to move a node up to restore heap property
        private void trickleUp(int index)
        {
            int parent = (index - 1) / 2;  // Calculate the index of the parent node
            EgeDeniziB bottom = heapArray[index];  // The node to move up

            while (index > 0 && string.Compare(heapArray[parent].Balik_Adi, bottom.Balik_Adi) < 0)
            {
                heapArray[index] = heapArray[parent];  // Move the parent down
                index = parent;  // Move index up
                parent = (parent - 1) / 2;  // Calculate the new parent index
            }

            heapArray[index] = bottom;  // Place the node in its correct position
        }

        // Method to move a node down to restore heap property
        private void trickleDown(int index)
        {
            int leftChild = 2 * index + 1;  // Left child index
            int rightChild = 2 * index + 2; // Right child index
            int largest = index;  // Assume the current node is the largest

            // Compare with left child
            if (leftChild < size && string.Compare(heapArray[leftChild].Balik_Adi, heapArray[largest].Balik_Adi) > 0)
            {
                largest = leftChild;
            }

            // Compare with right child
            if (rightChild < size && string.Compare(heapArray[rightChild].Balik_Adi, heapArray[largest].Balik_Adi) > 0)
            {
                largest = rightChild;
            }

            // If the largest is not the current node, swap and trickle down
            if (largest != index)
            {
                change(index, largest);  // Swap the nodes
                trickleDown(largest);  // Continue heapifying down
            }
        }

        // Method to swap two nodes
        private void change(int i, int j)
        {
            EgeDeniziB temp = heapArray[i];
            heapArray[i] = heapArray[j];
            heapArray[j] = temp;
        }

        // Method to display the heap
        public void displayHeap()
        {
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine(heapArray[i].Balik_Adi);  // Print fish name
            }
        }
        public int Size
        {
            get { return size; }
        }

        // Method to get fish at a specific index
        public EgeDeniziB GetFishAt(int index)
        {
            if (index < 0 || index >= size)
            {
                Console.WriteLine("Index out of range");
                return null;
            }
            return heapArray[index];
        }

    }

    class Program
    {
        // (2.c) Turkish alphabet added
        const string TurkishAlphabet = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

        // Simplification of Turkish characters function
        static string RemoveTurkishDiacritics(string text)
        {
            text = text.Replace("ğ", "g").Replace("Ğ", "G");
            text = text.Replace("ı", "i").Replace("İ", "I");
            text = text.Replace("ş", "s").Replace("Ş", "S");
            text = text.Replace("ö", "o").Replace("Ö", "O");
            text = text.Replace("ü", "u").Replace("Ü", "U");
            text = text.Replace("ç", "c").Replace("Ç", "C");

            return text.ToUpper(new CultureInfo("tr-TR"));
        }

        static void Main(string[] args)
        {
            // Reading the Balik_Text.txt file
            string[] baliklar = File.ReadAllLines("Balik_Text.txt");

            // (1.a) Creating BST
            FishTree balikAgaci = new FishTree();

            // (2.a) Creating Dictionary (Hash Table) 
            Dictionary<string, EgeDeniziB> fishDictionary = new Dictionary<string, EgeDeniziB>();

            // (3.a) Creating Heap
            MaxHeap balikHeap = new MaxHeap(baliklar.Length);

            foreach (var tekBilgi in baliklar)
            {
                EgeDeniziB currentBalik = new EgeDeniziB();
                string[] currentBilgi = tekBilgi.Split(';');

                for (int i = 0; i < currentBilgi.Length; i++)
                {
                    if (i == 0)
                    {
                        if (currentBilgi[i].Contains("("))
                        {
                            int parantezIndex = currentBilgi[i].IndexOf('(');
                            currentBalik.Balik_Adi = currentBilgi[i].Substring(0, parantezIndex).Trim();
                        }
                        else
                        {
                            currentBalik.Balik_Adi = currentBilgi[i].Trim();
                        }
                    }
                    else
                    {
                        currentBalik.Bilgi += currentBilgi[i];
                    }
                }

                // Adding to BST
                balikAgaci.Insert(currentBalik);

                // Adding to dictionary (normalised key)
                string dictKey = RemoveTurkishDiacritics(currentBalik.Balik_Adi);
                fishDictionary[dictKey] = currentBalik;

                // Filling the words tree
                var kelimeler = currentBalik.Bilgi.Split(
                    new[] { ' ', ',', '.', ';' },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (var kelime in kelimeler)
                {
                    currentBalik.KelimelerAgaci.Insert(kelime.Trim());
                }

                // (3.b) Adding to Heap
                balikHeap.Insert(currentBalik);

            }

            // (1.a 1.b 1.c 1.d) BST information, listing between the letters, balanced tree vb.

            // (1.a) Printing current fish tree (Balik_Adi and words)
            PrintFishTree(balikAgaci.Root);

            // (1.b) Calculating average depth while collecting all fish
            List<FishNode> allFishNodes = new List<FishNode>();
            CollectAllFishNodes(balikAgaci.Root, allFishNodes);

            double sumDepth = 0;
            foreach (var fishNode in allFishNodes)
            {
                sumDepth += fishNode.Balik.KelimelerAgaci.GetDepth();
            }
            double averageDepth = (allFishNodes.Count > 0) ? sumDepth / allFishNodes.Count : 0;

            Console.WriteLine("\n-----------------------------------------");
            Console.WriteLine($"Tüm kelimeler ağaçlarının ORTALAMA DERİNLİĞİ = {averageDepth}");
            Console.WriteLine("-----------------------------------------");

            foreach (var fishNode in allFishNodes)
            {
                var treeDepth = fishNode.Balik.KelimelerAgaci.GetDepth();
                var nodeCount = fishNode.Balik.KelimelerAgaci.GetNodeCount();
                var balancedDepth = fishNode.Balik.KelimelerAgaci.GetBalancedDepth();

                Console.WriteLine($"Balık: {fishNode.Balik.Balik_Adi}");
                Console.WriteLine($"\tKelime ağacı derinliği: {treeDepth}");
                Console.WriteLine($"\tKelime ağacı düğüm sayısı: {nodeCount}");
                Console.WriteLine($"\tDengeli ağaç olsaydı derinlik (yaklaşık): {balancedDepth}");
            }

            // (1.c) List the fish between two letters
            Console.WriteLine("\n[(1.c) ŞIKKI]");
            Console.Write("Lütfen ilk harfi giriniz: ");
            string letter1Input = Console.ReadLine();

            Console.Write("Lütfen ikinci harfi giriniz: ");
            string letter2Input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(letter1Input) && !string.IsNullOrWhiteSpace(letter2Input))
            {
                char startLetter = ToTurkishUpper(letter1Input[0]);
                char endLetter = ToTurkishUpper(letter2Input[0]);

                int startIndex = IndexInTurkishAlphabet(startLetter);
                int endIndex = IndexInTurkishAlphabet(endLetter);

                if (startIndex > endIndex)
                {
                    int temp = startIndex;
                    startIndex = endIndex;
                    endIndex = temp;
                }

                Console.WriteLine($"\n'{TurkishAlphabet[startIndex]}' ile '{TurkishAlphabet[endIndex]}' arasındaki balık isimleri:");
                ListFishBetweenLetters(balikAgaci.Root, startIndex, endIndex);
            }
            else
            {
                Console.WriteLine("Lütfen geçerli iki harf giriniz (boş olmamalı).");
            }

            // (1.d) Creating balanced BST
            Console.WriteLine("\n[(1.d) ŞIKKI] Dengeli ağaç oluşturma:");

            // Collecting fish and sorting
            List<EgeDeniziB> allFishBalikAdlari = new List<EgeDeniziB>();
            foreach (var fNode in allFishNodes)
            {
                allFishBalikAdlari.Add(fNode.Balik);
            }
            allFishBalikAdlari.Sort((x, y) =>
                string.Compare(x.Balik_Adi, y.Balik_Adi, true, new CultureInfo("tr-TR"))
            );

            // New balanced tree
            FishTree balancedTree = new FishTree();
            balancedTree.Root = BuildBalancedBST(allFishBalikAdlari, 0, allFishBalikAdlari.Count - 1);

            Console.WriteLine("Dengeli ağaç (in-order sırayla):");
            PrintFishTreeInOrder(balancedTree.Root);

            // (2.a 2.b) Creating and updating dictionary

            // (2.a) Listing fish in the dictionary
            Console.WriteLine("\n[(2.a) ŞIKKI] Dictionary'deki balıkları listeleyelim:");
            foreach (var kvp in fishDictionary)
            {
                Console.WriteLine($" - Anahtar: {kvp.Key} | Değer (Balık_Adi): {kvp.Value.Balik_Adi}");
                //Information version if want to print information of the fish in order to name
                //Console.WriteLine($" - Anahtar: {kvp.Key} | Değer (Balık_Adi): {kvp.Value.Bilgi}");
            }

            // (2.b) Updating fish
            Console.WriteLine("\n[(2.b) ŞIKKI] Balık güncelleme");
            Console.Write("Güncellenecek balığın adını giriniz: ");
            string fishToUpdate = Console.ReadLine().Trim();

            string dictSearchKey = RemoveTurkishDiacritics(fishToUpdate);

            if (!string.IsNullOrWhiteSpace(dictSearchKey) && fishDictionary.ContainsKey(dictSearchKey))
            {
                EgeDeniziB selectedFish = fishDictionary[dictSearchKey];

                Console.WriteLine($"Lütfen '{selectedFish.Balik_Adi}' için yeni bilgi (paragraf) giriniz:");
                string newParagraph = Console.ReadLine();

                // Update the fish info and word tree
                selectedFish.Bilgi = newParagraph;
                selectedFish.KelimelerAgaci = new WordsTree();

                var newWords = newParagraph.Split(
                    new[] { ' ', ',', '.', ';' },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (var w in newWords)
                {
                    selectedFish.KelimelerAgaci.Insert(w.Trim());
                }

                fishDictionary[dictSearchKey] = selectedFish;

                Console.WriteLine($"\nBalık '{selectedFish.Balik_Adi}' başarıyla güncellendi!");

                // [Extra] Printing the updated version of the fish
                Console.WriteLine("[(2.b) ŞIKKI] Güncellenen Balığın Yeni Bilgileri:");
                Console.WriteLine($"Balık Adı: {selectedFish.Balik_Adi}");
                Console.WriteLine("Yeni Paragraf (Bilgi):");
                Console.WriteLine(selectedFish.Bilgi);
                Console.WriteLine("Kelime Ağacındaki Kelimeler:");
                PrintWordsTree(selectedFish.KelimelerAgaci.Root);
            }
            else
            {
                Console.WriteLine("Girdiğiniz balık sözlükte bulunamadı veya geçersiz isim girdiniz.");
            }

            // (3.a 3.b 3.c) MaxHeap class

            // (3.a) Priting the fish in the heap
            Console.WriteLine("\nHeap yapısındaki balıklar:\n------------------------------------------------");

            balikHeap.displayHeap();

            // (3.b) already done at line 389

            // (3.c) Listing the first 3 fish in the heap

            Console.WriteLine("\n Heapteki ilk 3 balık ve bilgileri\n------------------------------------------------");

            for (int i = 0; i < 3; i++)
            {
                var balik = balikHeap.GetFishAt(i);
                Console.WriteLine($"{balik.Balik_Adi}: {balik.Bilgi}\n");
            }


            // (4.c) Measuring the time taken by both programs to sort a 100-element unsorted array 10,000,000 times

            int arraySize = 100;
            int iterations = 10000000;
            long[] insertionArray = new long[arraySize];
            long[] radixArray = new long[arraySize];

            Random rand = new Random();
            for (int i = 0; i < arraySize; i++)
            {
                long value = rand.Next(1, 10000000);
                insertionArray[i] = value;
                radixArray[i] = value;
            }

            Console.WriteLine("\n[(4.c) ŞIKKI]");
            Console.WriteLine("Sorting array of size: " + arraySize + " for " + iterations + " iterations");

            // Measure Insertion Sort
            long insertionSortTime = MeasureSortingTime(InsertionSort, insertionArray, iterations);
            Console.WriteLine("Insertion Sort Time: " + insertionSortTime + " ms");

            // Measure Radix Sort
            long radixSortTime = MeasureSortingTime(RadixSort, radixArray, iterations);
            Console.WriteLine("Radix Sort Time: " + radixSortTime + " ms");

            // Comparison
            Console.WriteLine(insertionSortTime < radixSortTime
                ? "Insertion Sort is faster."
                : "Radix Sort is faster.");

            // Analysis of results
            Console.WriteLine("Analysis:");
            Console.WriteLine("Insertion Sort: Suitable for small or nearly sorted datasets. Inefficient for large datasets.");
            Console.WriteLine("Radix Sort: Efficient for large datasets with uniform digit lengths. May consume more memory.");
            Console.ReadLine();
        }

        // Helper methods

        // Printing current BST in pre-order traversal
        static void PrintFishTree(FishNode node)
        {
            if (node != null)
            {
                PrintFishTree(node.Left);
                Console.WriteLine(node.Balik.Balik_Adi);
                PrintWordsTree(node.Balik.KelimelerAgaci.Root);
                PrintFishTree(node.Right);
            }
        }

        // Printing WordsTree in in-order traversal
        static void PrintWordsTree(WordNode node)
        {
            if (node != null)
            {
                PrintWordsTree(node.Left);
                Console.WriteLine("  -> " + node.Word);
                PrintWordsTree(node.Right);
            }
        }

        // Function that collects all nodes
        static void CollectAllFishNodes(FishNode node, List<FishNode> list)
        {
            if (node == null)
                return;
            list.Add(node);
            CollectAllFishNodes(node.Left, list);
            CollectAllFishNodes(node.Right, list);
        }

        // (1.c) Listing the fish between two letters
        static void ListFishBetweenLetters(FishNode root, int startIndex, int endIndex)
        {
            if (root == null)
                return;

            ListFishBetweenLetters(root.Left, startIndex, endIndex);

            char firstChar = ToTurkishUpper(root.Balik.Balik_Adi[0]);
            int fishIndex = IndexInTurkishAlphabet(firstChar);

            if (fishIndex >= startIndex && fishIndex <= endIndex)
            {
                Console.WriteLine("  -> " + root.Balik.Balik_Adi);
            }

            ListFishBetweenLetters(root.Right, startIndex, endIndex);
        }

        // (1.d) Creating balanced BST
        static FishNode BuildBalancedBST(List<EgeDeniziB> fishList, int start, int end)
        {
            if (start > end)
                return null;

            int mid = (start + end) / 2;
            FishNode root = new FishNode(fishList[mid]);

            root.Left = BuildBalancedBST(fishList, start, mid - 1);
            root.Right = BuildBalancedBST(fishList, mid + 1, end);

            return root;
        }

        // Printing the balanced tree in in-order traversal
        static void PrintFishTreeInOrder(FishNode node)
        {
            if (node == null)
                return;
            PrintFishTreeInOrder(node.Left);
            Console.WriteLine("  -> " + node.Balik.Balik_Adi);
            PrintFishTreeInOrder(node.Right);
        }

        // Finding index at Turkish letters
        static int IndexInTurkishAlphabet(char c)
        {
            return TurkishAlphabet.IndexOf(c);
        }

        // Convert a single character to uppercase
        static char ToTurkishUpper(char c)
        {
            return char.ToUpper(c, new CultureInfo("tr-TR"));
        }


        // 4.a and 4.b


        // Insertion Sort implementation
        public static void InsertionSort(long[] arr)
        {
            int inIdx, outIdx;
            for (outIdx = 1; outIdx < arr.Length; outIdx++)
            {
                long temp = arr[outIdx];
                inIdx = outIdx;

                while (inIdx > 0 && arr[inIdx - 1] >= temp)
                {
                    arr[inIdx] = arr[inIdx - 1];
                    inIdx--;
                }

                arr[inIdx] = temp;
            }
            // Time Complexity: O(n^2) in the worst and average cases, O(n) in the best case (nearly sorted arrays).
            // Space Complexity: O(1) as it is an in-place sorting algorithm.
            // Advantages: Performs well on small or nearly sorted datasets.
            // Disadvantages: Inefficient for large datasets due to quadratic time complexity.
        }

        // Radix Sort implementation
        public static void RadixSort(long[] arr)
        {
            long max = GetMax(arr);
            for (long exp = 1; max / exp > 0; exp *= 10)
            {
                CountingSort(arr, exp);
            }
            // Time Complexity: O(d * (n + b)), where d is the number of digits, n is the number of elements, 
            // and b is the base (10 in this case).
            // Space Complexity: O(n + b) due to auxiliary arrays.
            // Advantages: Performs well on large datasets with uniform digit lengths.
            // Disadvantages: Requires additional memory and can be slower on small datasets.
        }

        // Helper function for Radix Sort
        private static void CountingSort(long[] arr, long exp)
        {
            long[] output = new long[arr.Length];
            int[] count = new int[10];

            for (int i = 0; i < arr.Length; i++)
            {
                count[(int)((arr[i] / exp) % 10)]++;
            }

            for (int i = 1; i < 10; i++)
            {
                count[i] += count[i - 1];
            }

            for (int i = arr.Length - 1; i >= 0; i--)
            {
                output[count[(int)((arr[i] / exp) % 10)] - 1] = arr[i];
                count[(int)((arr[i] / exp) % 10)]--;
            }

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = output[i];
            }
        }

        // Helper function to find the maximum value in an array
        private static long GetMax(long[] arr)
        {
            long max = arr[0];
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] > max)
                {
                    max = arr[i];
                }
            }
            return max;
        }

        // Function to measure sorting time
        public static long MeasureSortingTime(Action<long[]> sortMethod, long[] arr, int iterations)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < iterations; i++)
            {
                sortMethod((long[])arr.Clone());
            }

            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }
}