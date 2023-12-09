using System;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    
    static int numberOfElementsA = 20000;
    static int numberOfElementsB = 9000;
    static int[] A = new int[numberOfElementsA];
    static int[] B = new int[numberOfElementsB];
    static void Main()
    {
        int[] Result = new int[A.Length + B.Length]; // Added Result array
        Random rnd = new Random();

        // Sequential initialization
        for (int i = 0; i < numberOfElementsA; i++)
        {
            A[i] = rnd.Next(3, 8);
        }
        for(int i = 0; i < numberOfElementsB; i++)
        {
            B[i] = rnd.Next(7, 9);
        }

        Array.Sort(A);
        Array.Sort(B);

        ParallelOptions options = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount };

        Stopwatch watchParallel = Stopwatch.StartNew();

        // Increase task granularity

        var arrayATask = Task.Factory.StartNew(() =>
        {
            int ChunkSizeA = numberOfElementsA / 4;
            Parallel.For(0, numberOfElementsA / ChunkSizeA, options, counter =>
            {
                int startIndex = counter * ChunkSizeA;
                int endIndex = Math.Max(startIndex + ChunkSizeA, numberOfElementsA);

                for (int i = startIndex; i < endIndex; i++)
                {
                    int j = BinarySearchB(A[i]);
                    Result[i + j] = A[i];
                    //int k = BinarySearchA(B[i]);
                    //Result[i + k] = B[i];
                }
            });
        });
        var arrayBTask = Task.Factory.StartNew(() => {

            int ChunkSizeB = numberOfElementsB / 4;
            Parallel.For(0, numberOfElementsB / ChunkSizeB, options, counter =>
            {
                int startIndex = counter * ChunkSizeB;
                int endIndex = Math.Max(startIndex + ChunkSizeB, numberOfElementsB);

                for (int i = startIndex; i < endIndex; i++)
                {
                    int j = BinarySearchA(B[i]);
                    Result[i + j] = B[i];
                    //int k = BinarySearchA(B[i]);
                    //Result[i + k] = B[i];
                }
            });
          });
        Task.WaitAll(arrayATask, arrayBTask);
        watchParallel.Stop();
        Console.WriteLine("Parallel Result:");

        // Printing the Parallel Arrays
        //Console.WriteLine("Sorted A:");
        //for (int i = 0; i < A.Length; i++)
        //{
        //    Console.Write(A[i] + " ");
        //}
        //Console.WriteLine("Sorted B");
        //for (int i = 0; i < B.Length; i++)
        //{
        //    Console.Write(B[i] + " ");
        //}
        //Console.WriteLine();
        //Console.WriteLine("Parallel Sorted Array:");
        //for (int i = 0; i < Result.Length; i++)
        //{
        //    Console.Write(Result[i] + " ");
        //}
        Console.WriteLine("Parallel: {0}", watchParallel.ElapsedMilliseconds);

        // Sequential Result
        Console.WriteLine();
        Console.WriteLine("Sequential Result:");
        int[] sequentialResult = new int[A.Length + B.Length];
        SequentialMergeArraysSlow(A, B,sequentialResult);
        //for(int i = 0; i < sequentialResult.Length; i++)
        //{
        //    Console.Write(sequentialResult[i] + " ");
        //}

        Console.ReadLine();
    }

   static int BinarySearchB(int key)
    {
        int low = 0;
        int high = B.Length - 1;

        while (low <= high)
        {
            int mid = low + (high - low) / 2;

            if (key > B[mid])
            {
                low = mid + 1;
            }
            else
            {
                high = mid - 1;
            }
        }

        return low;
    }
    static int BinarySearchA(int key)
    {
        int low = 0;
        int high = A.Length - 1;

        while (low <= high)
        {
            int mid = low + (high - low) / 2;

            if (key >= A[mid])
            {
                low = mid + 1;
            }
            else
            {
                high = mid - 1;
            }
        }

        return low;
    }
    public static void SequentialMergeArraysSlow(int[] arr1, int[] arr2, int[] arr3)
    {
        int i = 0;
        int j = 0;
        int k = 0;

        var watchSequential = Stopwatch.StartNew();
        // traverse the arr1 and insert its element in arr3
        while (i < arr1.Length)
        {
            arr3[k++] = arr1[i++];
        }

        // now traverse arr2 and insert in arr3
        while (j < arr2.Length)
        {
            arr3[k++] = arr2[j++];
        }
        // sort the whole array arr3
        Array.Sort(arr3);
        watchSequential.Stop();
        Console.WriteLine("Sequential time: {0}:",watchSequential.ElapsedMilliseconds);
    }
    static int[] SequentialMergeArrays(int[] array1, int[] array2)
    {
        int[] result = new int[array1.Length + array2.Length];

        int i = 0, j = 0, k = 0;
        
        Stopwatch sequentialWatch = Stopwatch.StartNew();

        while (i < array1.Length && j < array2.Length)
        {
            if (array1[i] < array2[j])
            {
                result[k] = array1[i];
                i++;
            }
            else
            {
                result[k] = array2[j];
                j++;
            }
            k++;
        }

        while (i < array1.Length)
        {
            result[k] = array1[i];
            i++;
            k++;
        }

        while (j < array2.Length)
        {
            result[k] = array2[j];
            j++;
            k++;
        }

        sequentialWatch.Stop();

        Console.WriteLine("Sequential time: {0}",sequentialWatch.ElapsedMilliseconds);

        return result;
    }
}
