using Tasks.Common;

namespace Task05.CombArray;

[Task(5, "Комбинированный массив", "alt")]
public class CombArraySolutionAlt: ICombArraySolution
{
    public void Run()
    {
        int[] arr = ReadArray();
        int[] sortedArr = SortV2(arr);
        Print(sortedArr);
    }

    public int[] FilterNonDecreasing(int[] array)
    {
        return SortV2(array);
    }

    private static int[] ReadArray()
    {
        int size = int.Parse(Console.ReadLine());
        int[] arr = new int[size];

        for (int i = 0; i < size; i++)
        {
            arr[i] = int.Parse(Console.ReadLine());
        }

        return arr;
    }

    private static int[] Sort(int[] arr)
    {
        if (arr.Length <= 1) return arr;

        int[] sortedArr = new int[arr.Length];
        sortedArr[0] = arr[0];
        int idx = 0;

        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i] >= sortedArr[idx])
            {
                idx += 1;
                sortedArr[idx] = arr[i];
            }
        }

        if (idx + 1 != arr.Length)
        {
            Array.Resize(ref sortedArr, idx + 1);
        }

        return sortedArr;
    }

    private static int[] SortV2(int[] arr)
    {
        if (arr.Length <= 1) return arr;

        int idx = 1;

        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i] >= arr[idx - 1])
            {
                arr[idx] = arr[i];
                idx += 1;
            }
        }

        Array.Resize(ref arr, idx);

        return arr;
    }

    private static void Print(int[] arr)
    {
        foreach (int el in arr)
        {
            Console.WriteLine(el);
        }
    }
}
