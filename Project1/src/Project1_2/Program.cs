using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Project1._1
{
    internal class Program
    {


        public class Neuron
        {
            private double[][] inputs;
            private double[] weights;
            private static Random rand = new Random();

            // constructor
            public Neuron(int numInputs)
            {
                inputs = new double[numInputs][];
                weights = new double[numInputs];

                // Random weights between 0 and 1
                for (int i = 0; i < numInputs; i++)
                {
                    weights[i] = rand.NextDouble();
                }
            }

            // Computing outputs using inputs and weights related to that input
            public double ComputeOutput(double[][] inputs)
            {
                this.inputs = inputs;

                double weightedSum = 0;

                // Selecting all matrices and their variables to calculate weight
                for (int i = 0; i < inputs.Length; i++)
                {
                    for (int j = 0; j < inputs[0].Length; j++)
                    {
                        weightedSum += this.inputs[i][j] * weights[i];
                    }
                }


                return weightedSum;
            }

            // Weights getter method
            public double[] GetWeights()
            {
                return this.weights;
            }

            // Weights setter method
            public void SetWeights(double[] newWeights)
            {
                if (newWeights.Length == weights.Length)
                {
                    this.weights = newWeights;
                }
                else
                {
                    Console.WriteLine("Error: Weights array size mismatch.");
                }
            }

            // Pulling the weights to chech the calcualtions' accuracy
            public void DisplayWeights()
            {
                Console.WriteLine("Weights: ");
                foreach (var weight in weights)
                {
                    Console.WriteLine(weight);
                }
            }
        }

        public class NeuralNetwork
        {
            private Neuron n1;
            private Neuron n2;
            private double learningRate;
            private int[] output = new int[20];


            // Constructor: Creating neurons in the network
            public NeuralNetwork(int numInputs, double learningRate)
            {
                this.learningRate = learningRate;
                n1 = new Neuron(numInputs);  // create the n1 neuron
                n2 = new Neuron(numInputs);  // create the n2 neuron
            }

            // Training the neurons to get more suitable weigthts
            public void Train(double[][][] inputs, int[] targets, int epochs)
            {
                for (int epoch = 0; epoch < epochs; epoch++) // epoch loop
                {
                    Console.WriteLine($"Epoch {epoch + 1}/{epochs}");  // Display the eopch's number

                    for (int m = 0; m < inputs.Length; m++) // Loop on every input
                    {
                        for (int i = 0; i < inputs[m].GetLength(0); i++)
                        {

                            double[][] input = inputs[m];

                            int target = targets[m];      // Pull the target

                            // Calculating the outputs with the realted input
                            double n1Output = n1.ComputeOutput(input);
                            double n2Output = n2.ComputeOutput(input);

                            // Determining the neuron that has the bigger output
                            int maxOutputNeuron = (n1Output > n2Output) ? 1 : 2;

                            // Comparing the bigger output with the target
                            if ((target == 1 && maxOutputNeuron != 1) || (target == 2 && maxOutputNeuron != 2))
                            {
                                // If they are not same update neurons
                                if (maxOutputNeuron == 1)
                                {
                                    // n1's output is wrong and n2's true
                                    UpdateWeights(n1, -1, input);  // decrease the n1's weights
                                    UpdateWeights(n2, 1, input);   // increase the n2's weights
                                }
                                else
                                {
                                    // n2's output is wrong and n1's true
                                    UpdateWeights(n2, -1, input);  // decrease the n2's weights
                                    UpdateWeights(n1, 1, input);   // increase the n1's weights
                                }
                            }

                        }
                    }
                }
            }

            // Weight update function
            private void UpdateWeights(Neuron neuron, int direction, double[][] inputs)
            {
                double[] weights = neuron.GetWeights();  // Get the weights of the neuron

                for (int m = 0; m < inputs.Length; m++) // Loop on every input
                {
                    for (int i = 0; i < inputs[m].GetLength(0); i++)
                    {
                        weights[m] += direction * learningRate * inputs[m][i];  // Update the weights
                    }
                }
                neuron.SetWeights(weights);  // Set the updated weights
            }

            // Displaying weights in the neurons
            public void DisplayWeights()
            {
                Console.WriteLine("N1 Weights:");
                n1.DisplayWeights();  // Show the n1's weights
                Console.WriteLine("N2 Weights:");
                n2.DisplayWeights();  // Show the n2's weights
            }

            // Test method, it is similar to train but not updating weights at this method
            public void Test(double[][][] inputs)
            {
                Console.WriteLine("\nTest Results:");

                for (int a = 0; a < inputs.Length; a++) // Loop on all of the matrices
                {
                    double[][] input = inputs[a];


                    // Calculating outputs with the inputs
                    double n1Output = n1.ComputeOutput(input);
                    double n2Output = n2.ComputeOutput(input);

                    // Print the outputs
                    Console.WriteLine("Input:");
                    for (int i = 0; i < input.Length; i++) // Loop on all of the matrices
                    {
                        for (int b = 0; b < input[i].Length; b++)
                        {
                            Console.Write($"{input[i][b],1} ");
                        }
                        Console.WriteLine();

                    }
                    Console.WriteLine($"=> N1 Output: {n1Output:F2}, N2 Output: {n2Output:F2}");
                    Console.WriteLine();

                    // Decide the output
                    if (n1Output > n2Output)
                    {

                        Console.WriteLine("Output: 1 (N1 is greater)\n");
                        output[a] = 1;
                    }
                    else
                    {
                        Console.WriteLine("Output: 2 (N2 is greater)\n");
                        output[a] = 2;
                    }

                }
            }

            public void DisplayMatrices(double[][][] inputs)
            {
                Console.WriteLine("\nMatrices: ");

                for (int a = 0; a < inputs.Length; a++) // Loop on all of the matrices
                {
                    double[][] input = inputs[a];

                    // Print the outputs
                    Console.WriteLine($"Input{a + 1}:");
                    for (int i = 0; i < input.Length; i++) // Loop on all of the matrices
                    {
                        for (int b = 0; b < input[i].Length; b++)
                        {
                            Console.Write($"{input[i][b],1} ");
                        }
                        Console.WriteLine();

                    }

                }
            }

            // Table to check accuracy of inputs to targets
            public void TargetOutputTable(double[][][] inputs, int[] targets)
            {
                double[][] table = new double[targets.Length][];
                Console.WriteLine("\nTarget - Output and Accuracy table:");
                int trueOutputs = 0;


                for (int a = 0; a < inputs.Length; a++) // Loop on all of the matrices
                {
                    double[][] input = inputs[a];


                    // Calculating outputs with the inputs
                    double n1Output = n1.ComputeOutput(input);
                    double n2Output = n2.ComputeOutput(input);

                    // Determining if the output is correct to find accuracy
                    if (output[a] == targets[a])
                    {
                        trueOutputs += 1;
                    }

                    //adding output-target pair to the table
                    table[a] = new double[2];
                    table[a][0] = targets[a];
                    table[a][1] = output[a];
                }

                // Printing the table
                Console.WriteLine("\nTarget - Output Table:");
                Console.WriteLine("{");
                for (int i = 0; i < table.Length; i++)
                {
                    Console.Write($"  {{ {table[i][0]}, {table[i][1]} }}");
                    if (i < table.Length - 1)
                        Console.Write(", ");
                    Console.Write("");
                    if ((i + 1) % 5 == 0)
                    {
                        Console.WriteLine();
                    }
                }
                Console.WriteLine("}");

                // Printing the accuracy
                Console.WriteLine($"Accuracy: {trueOutputs}/{targets.Length} = %{trueOutputs / (double)targets.Length * 100}");
            }
        }

        static void Main(string[] args)
        {
            // 1 Matrices
            double[][] ornek_matris1 = new double[][]
            {
                new double[] { 1, 1, 0, 1, 1 },
                new double[] { 1, 0, 0, 1, 1 },
                new double[] { 1, 1, 0, 1, 1 },
                new double[] { 1, 1, 0, 1, 1 },
                new double[] { 1, 1, 0, 1, 1 }
            };

            double[][][] matrices_1 = new double[10][][];

            for (int m = 0; m < matrices_1.Length; m++) // Selecting all of the matrices
            {
                matrices_1[m] = (double[][])ornek_matris1.Clone();
                for (int i = 0; i < ornek_matris1.Length; i++)
                {
                    matrices_1[m][i] = (double[])ornek_matris1[i].Clone(); // Copy every line 
                }

                for (int i = 0; i < ornek_matris1.Length; i++)
                {
                    for (int j = 0; j < ornek_matris1[0].Length; j++)
                    {
                        matrices_1[m][i][j] = ornek_matris1[i][j]; // Copy the data

                        if (i == 4 && (m == 9 || m == 8 || m == 6 || m == 5))
                        {
                            matrices_1[m][i][m - 5] = 0;
                        }

                        else if (i == 4 && (m == 4 || m == 3))
                        {
                            matrices_1[m][i][m] = 0;
                            matrices_1[m][i][4 - m] = 0;
                        }

                        else if (i == 4 && (m == 2 || m == 1))
                        {
                            matrices_1[m][i][5 - m] = 0;
                            matrices_1[m][i][4 - m] = 0;
                            matrices_1[m][i][3 - m] = 0;
                            matrices_1[m][i][2 - m] = 0;
                        }
                        else if (i == 4 && m == 0)
                        {
                            matrices_1[m][i][m] = 0;
                            matrices_1[m][i][m + 1] = 0;
                            matrices_1[m][i][m + 2] = 0;
                            matrices_1[m][i][m + 3] = 0;
                            matrices_1[m][i][m + 4] = 0;
                        }
                    }
                }
            }

            // 2 Matrices
            double[][] ornek_matris2 = new double[][]
            {
                new double[] { 1, 0, 0, 0, 1 },
                new double[] { 0, 1, 1, 1, 0 },
                new double[] { 1, 1, 1, 0, 1 },
                new double[] { 1, 1, 0, 1, 1 },
                new double[] { 0, 0, 0, 0, 0 }
            };

            double[][][] matrices_2 = new double[10][][];

            for (int m = 0; m < matrices_2.Length; m++) // Selecting all of the matrices
            {
                matrices_2[m] = (double[][])ornek_matris2.Clone();
                for (int i = 0; i < ornek_matris2.Length; i++)
                {
                    matrices_2[m][i] = (double[])ornek_matris2[i].Clone(); // Copy every line 
                }

                for (int i = 0; i < ornek_matris2.Length; i++)
                {
                    for (int j = 0; j < ornek_matris2[0].Length; j++)
                    {
                        matrices_2[m][i][j] = ornek_matris2[i][j]; // Copy the data

                        if (i == 0 && (m == 0 || m == 4))
                        {
                            matrices_2[m][i][m] = 0;
                        }

                        else if (i == 1 && (m == 1 || m == 3))
                        {
                            matrices_2[m][i][m] = 0;
                        }

                        else if (i == 2 && (m == 5 || m == 7))
                        {
                            matrices_2[m][i][m - 3] = 0;
                        }
                        else if (i == 3 && (m == 6 || m == 8))
                        {
                            matrices_2[m][i][m - 5] = 0;
                        }
                        else if (i == 4 && m == 9)
                        {
                            matrices_2[m][i][m - 9] = 1;
                            matrices_2[m][i][m - 5] = 1;
                        }
                    }
                }
            }



            // Independent examples
            double[][] example3_1 = new double[][]
            {
                new double[] { 1, 1, 0, 1, 1 },
                new double[] { 1, 1, 0, 1, 1 },
                new double[] { 1, 1, 0, 1, 1 },
                new double[] { 1, 1, 0, 1, 1 },
                new double[] { 1, 1, 0, 1, 1 }
            };

            double[][] example3_2 = new double[][]
            {
                new double[] { 1, 1, 0, 1, 1 },
                new double[] { 0, 0, 0, 0, 1 },
                new double[] { 1, 1, 0, 1, 1 },
                new double[] { 1, 1, 0, 1, 1 },
                new double[] { 1, 0, 0, 0, 0 }
            };

            double[][] example3_3 = new double[][]
            {
                new double[] { 1, 0, 1, 0, 1 },
                new double[] { 0, 1, 0, 1, 0 },
                new double[] { 1, 1, 1, 0, 1 },
                new double[] { 1, 1, 0, 1, 1 },
                new double[] { 1, 0, 0, 0, 1 }
            };

            // A single neuron example
            // Creating a neuron with 4 examples
            Neuron neuron = new Neuron(4);

            // 4 input example
            double[][] neuron_inputs = new double[][]
                {
                    new double[] {0.9},
                    new double[] {0.2},
                    new double[] {0.5},
                    new double[] {0.7}
                };

            // Calculating outputs
            double output = neuron.ComputeOutput(neuron_inputs);

            // Writing output in the console
            Console.WriteLine("Output: " + output);

            // Displaying weights used to calculate these 4 inputs
            neuron.DisplayWeights();

            // inputs matrice to store 1 matrices (10 matrices) and 2 matrices (10 matrices) (total 20 matrices)
            double[][][] inputs = new double[20][][];

            for (int m = 0; m < (inputs.Length) / 2; m++)
            {
                inputs[m] = matrices_1[m];
            }

            for (int m = 0; m < (inputs.Length) / 2; m++)
            {
                inputs[m + 10] = matrices_2[m];
            }

            // targets of that 1 and 2 matrices
            int[] targets =
            {
                1, 1, 1, 1, 1,
                1, 1, 1, 1, 1,
                2, 2, 2, 2, 2,
                2, 2, 2, 2, 2,
            };

            // inputs matrice to store 3 independent matrice
            double[][][] testDifferentInputs = new double[3][][]
                        {
                            example3_1, example3_2, example3_3
                        };

            // Neural Network that gets 20 matrices to train and has 0.03 λ value 
            NeuralNetwork nn = new NeuralNetwork(inputs.Length, 0.000005);

            // Displaying all of the 20 matrices
            nn.DisplayMatrices(inputs);

            // Training the network with these 20 matrices
            nn.Train(inputs, targets, 40);

            // Testing the network with the same inputs
            nn.Test(inputs);

            // Writing the output-target and accuracy rate
            nn.TargetOutputTable(inputs, targets);

            // Testing with 3 different matrices
            nn.Test(testDifferentInputs);

            Console.ReadKey();

        }
    }
}