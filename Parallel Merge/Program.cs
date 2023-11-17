using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Threading.Tasks;

//Make Sure the  Array is Sorted
Random rnd = new Random();
int numberOfElements = 10000;
int[] A = new int[numberOfElements];
int[] B = new int[numberOfElements];
var firstArray = Task.Factory.StartNew(() =>
{
    Parallel.For(0, numberOfElements, i =>
    {
        A[i] = rnd.Next(5, 28);
    }
    );
});
firstArray.Wait();
Array.Sort(A);
var secondArray = Task.Factory.StartNew(() =>
{
    Parallel.For(0, numberOfElements, i =>
   {
       B[i] = rnd.Next(3, 15);
   }
   );
});
secondArray.Wait();
Array.Sort(B);
int[] Result = new int[A.Length + B.Length];
int FindInA(int key, int[] Array)
{
    int count = 0;
    for (int i = 0; i < Array.Length; i++)
    {
        if (key >= Array[i])
        {
            count++;
        }
    }
    return count;
   
}
int FindInB(int key, int[] Array)
{
    int count = 0;
    for (int i = 0; i < Array.Length; i++)
    {
        if (key > Array[i])
        {
            count++;
        }
    }
    return count;

}
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

var mergeTask = new Task(() => {

        var innerTask1 = new Task(() =>
        {
            for (int i = 0; i < A.Length; i++)
            {
                int j = FindInB(A[i],B);
                Result[i + j] = A[i];
            }

        },TaskCreationOptions.AttachedToParent);
        var innerTask2 = new Task(() =>
        {
            for (int i = 0; i < B.Length; i++)
            {
                int j = FindInA(B[i],A);
                Result[i + j] = B[i];
            }
        },TaskCreationOptions.AttachedToParent);
        innerTask1.Start();
        innerTask2.Start();
    
});

mergeTask.Start();
mergeTask.Wait();
Console.WriteLine("Sorted A:");
foreach(int i in A)
{
    Console.Write(i + " ");
}
Console.WriteLine();
Console.WriteLine("Sorted B:");
foreach(int i in B)
{
    Console.Write(i + " ");
}
Console.WriteLine();
Console.WriteLine("Sorted Array:");
for (int i = 0; i < Result.Length; i++) {
    Console.Write(Result[i] + " ");
}