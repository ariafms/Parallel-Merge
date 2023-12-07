using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Threading.Tasks;
Random rnd = new Random();
int numberOfElements = 100;
int[] A = new int[numberOfElements];
int[] B = new int[numberOfElements];
int FindInA(int key)
{
    int count = 0;
    for (int i = 0; i < A.Length; i++)
    {
        if (key >= A[i])
        {
            count++;
        }
    }
    return count;

}
int FindInB(int key)
{
    int count = 0;
    for (int i = 0; i < B.Length; i++)
    {
        if (key > B[i])
        {
            count++;
        }
    }
    return count;

}
//Make Sure the  Array is Sorted

Stopwatch watchSequential, watchParallel, watchInit;
ParallelOptions options = new ParallelOptions() { MaxDegreeOfParallelism = 2 };
watchInit = Stopwatch.StartNew();
var firstArray = Task.Factory.StartNew(() =>
{
    Parallel.For(0, numberOfElements, options, i =>
    {
        A[i] = rnd.Next(5, 28);
    }
    );
});
firstArray.Wait();
Array.Sort(A);
var secondArray = Task.Factory.StartNew(() =>
{
    Parallel.For(0, numberOfElements, options, i =>
   {
       B[i] = rnd.Next(3, 15);
   }
   );
});
secondArray.Wait();
Array.Sort(B);
watchInit.Stop();
Console.WriteLine("Init Elapsed Time: {0}", watchInit.Elapsed);
int[] Result = new int[A.Length + B.Length];

#region
//int Find(int key, int[] array)
//{
//    int count = 0;
//    int low = 0 , high = array.Length - 1 , mid = (low + high) / 2;
//        while (low <= high)
//        {
//            if (key < array[mid])
//            {
//                high = mid - 1;
//                mid = (low + high) / 2;
//            }
//        }
//    return count;
//}
#endregion
watchParallel = Stopwatch.StartNew();
//loop through an array of tasks
Parallel.For(0, 10, counter =>
  {
      for (int i = 10 * counter; i < 10 * (counter + 1); i++)
      {
          int j = FindInB(A[i]);
          Result[i + j] = A[i];
          int k = FindInA(B[i]);
          Result[i + k] = B[i];
      }
  });

watchParallel.Stop();
Console.WriteLine("Parallel:");
Console.WriteLine("Elapsed Time: {0}", watchParallel.Elapsed);


//Sorted A and B


#region
//Console.WriteLine("Sorted A:");
//foreach (int i in A)
//{
//    Console.Write(i + " ");
//}
//Console.WriteLine();
//Console.WriteLine("Sorted B:");
//foreach (int i in B)
//{
//    Console.Write(i + " ");
//}
#endregion

//Parallel Result

#region
Console.WriteLine();
//Console.WriteLine("Sorted Array:");
//for (int i = 0; i < Result.Length; i++)
//{
//    Console.Write(Result[i] + " ");
//}
#endregion

static int[] SequentialMergeArrays(int[] array1, int[] array2)
{
    int[] result = new int[array1.Length + array2.Length];

    int i = 0, j = 0, k = 0;

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

    return result;
}
Console.WriteLine();
Console.WriteLine("Sequnetial:");
#region
//Console.WriteLine("Sorted A:");
//foreach (int i in A)
//{
//    Console.Write(i + " ");
//}
//Console.WriteLine();
//Console.WriteLine("Sorted B:");
//foreach (int i in B)
//{
//    Console.Write(i + " ");
//}
watchSequential = Stopwatch.StartNew();
var sequentialResult = SequentialMergeArrays(A, B);
watchSequential.Stop();
//Console.WriteLine();
//Console.WriteLine("Sorted Array:");
//for (int i = 0; i < Result.Length; i++)
//{
//    Console.Write(sequentialResult[i] + " ");
//}
#endregion
Console.WriteLine("Elapsed Time: {0}", watchSequential.Elapsed);
