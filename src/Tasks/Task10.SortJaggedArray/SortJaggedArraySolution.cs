using System;

using Tasks.Common;

namespace Task10.SortJaggedArray;

[Task(10, "Сортировка ступенчатого массива")]
public class SortJaggedArraySolution : ISortJaggedArraySolution
{

    public void Run()
    {

        int[][] arrTaxi = new int[10][];
 
        arrTaxi[0] = new int[] {100, 289, 200, 101, 90, 230};
        arrTaxi[1] = new int[] {290, 300, 303, 120, 150 };
        arrTaxi[2] = new int[] { 80 };
        arrTaxi[3] = new int[] { 300, 60, 120, 400, 410 };
        arrTaxi[4] = new int[] { 60, 100, 40 };
        arrTaxi[5] = new int[] { 60, 160, 165, 120, 110, 230, 200, 30 };
        arrTaxi[6] = new int[] { 230, 200, 250, 100 };
        arrTaxi[7] = new int[] { 100, 209, 175, 100 };
        arrTaxi[8] = new int[] { 70, 120, 290 };
        arrTaxi[9] = new int[] { 90, 80, 105, 140, 120 };

        int iterationCount = 0;
        SortTaxiArray(arrTaxi, ref iterationCount);

        foreach(int[] arr in arrTaxi)
        {
            foreach(int t in arr)
                Console.Write(t + " ");
            Console.Write("Sum: " + arr.Sum());
            Console.WriteLine();
        }

        Console.WriteLine("Iteration: " + iterationCount);
        iterationCount = 0;

        // ПРИМЕР 2
        int[][] arrTaxi2 = new int[10][];
 
        arrTaxi2[0] = new int[] { 80 };
        arrTaxi2[1] = new int[] { 60, 100, 40 };
        arrTaxi2[2] = new int[] { 70, 120, 290 };
        arrTaxi2[3] = new int[] { 100, 209, 175, 100 };
        arrTaxi2[4] = new int[] { 230, 200, 250, 100 };
        arrTaxi2[5] = new int[] { 90, 80, 105, 140, 120 };
        arrTaxi2[6] = new int[] { 290, 300, 303, 120, 150 };
        arrTaxi2[7] = new int[] { 300, 60, 120, 400, 410 };
        arrTaxi2[8] = new int[] { 100, 289, 200, 101, 90, 230 };
        arrTaxi2[9] = new int[] { 60, 160, 165, 120, 110, 230, 200, 30 };

        // ИСПРАВЛЕНО: Передаем arrTaxi2
        SortTaxiArray(arrTaxi2, ref iterationCount);

        // ИСПРАВЛЕНО: Выводим arrTaxi2
        foreach(int[] arr in arrTaxi2)
        {
            foreach(int t in arr)
                Console.Write(t + " ");
            Console.Write("Sum: " + arr.Sum());
            Console.WriteLine();
        }

        Console.WriteLine("Iteration: " + iterationCount);
    }

    public void SortTaxiArray(int[][] arrTaxi, ref int iterationCounter)
    {
        QuickSort(arrTaxi, 0, arrTaxi.Length - 1, ref iterationCounter);
    }

    private static void QuickSort(int[][] arr, int low, int high, ref int iterationCounter)
    {
        if (low >= high)
            return;

        int pivot = Partition(arr, low, high, ref iterationCounter);

        QuickSort(arr, low, pivot - 1, ref iterationCounter); 
        QuickSort(arr, pivot + 1, high, ref iterationCounter); 
    }

    private static int Partition(int[][] arr, int low, int high, ref int iterationCounter)
    {
        int[] pivot = arr[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            iterationCounter++;
            if (TaxiArrCompare(arr[j], pivot))
                Swap(++i, j, arr); 
        }

        Swap(i+1, high, arr); 

        return i + 1;
    }

    private static bool TaxiArrCompare(int[] current, int[] pivot)
    {
        if (current.Length > pivot.Length) return true;
        if (current.Length < pivot.Length) return false;
        return current.Sum() > pivot.Sum();
    }

    private static void Swap(int i, int j, int[][] arr)
    {
        int[] temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }
}
