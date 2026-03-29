using Tasks.Common;

namespace Task10.SortJaggedArray;

[Task(10, "Сортировка ступенчатого массива", "alt")]
public class SortJaggedArraySolutionAlt : ISortJaggedArraySolution
{
    public void Run()
    {
        int[][] arrTaxi1 = new int[10][];
        arrTaxi1[0] = new int[] { 100, 289, 200, 101, 90, 230 };
        arrTaxi1[1] = new int[] { 290, 300, 303, 120, 150 };
        arrTaxi1[2] = new int[] { 80 };
        arrTaxi1[3] = new int[] { 300, 60, 120, 400, 410 };
        arrTaxi1[4] = new int[] { 60, 100, 40 };
        arrTaxi1[5] = new int[] { 60, 160, 165, 120, 110, 230, 200, 30 };
        arrTaxi1[6] = new int[] { 230, 200, 250, 100 };
        arrTaxi1[7] = new int[] { 100, 209, 175, 100 };
        arrTaxi1[8] = new int[] { 70, 120, 290 };
        arrTaxi1[9] = new int[] { 90, 80, 105, 140, 120 };
        RunTest(arrTaxi1, "тест 1");

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
        RunTest(arrTaxi2, "тест 2");

        int[][] arrTaxi3 = new int[10][];
        arrTaxi3[9] = new int[] { 80 };
        arrTaxi3[8] = new int[] { 60, 100, 40 };
        arrTaxi3[7] = new int[] { 70, 120, 290 };
        arrTaxi3[6] = new int[] { 100, 209, 175, 100 };
        arrTaxi3[5] = new int[] { 230, 200, 250, 100 };
        arrTaxi3[4] = new int[] { 90, 80, 105, 140, 120 };
        arrTaxi3[3] = new int[] { 290, 300, 303, 120, 150 };
        arrTaxi3[2] = new int[] { 300, 60, 120, 400, 410 };
        arrTaxi3[1] = new int[] { 100, 289, 200, 101, 90, 230 };
        arrTaxi3[0] = new int[] { 60, 160, 165, 120, 110, 230, 200, 30 };
        RunTest(arrTaxi3, "тест 3");
    }
    private void RunTest(int[][] arrTaxi, string label)
    {
        Console.WriteLine(label);
        int iters = 0;
        Sort(arrTaxi, ref iters);
        Console.WriteLine($"число итераций - {iters}");
        foreach (int[] arr in arrTaxi)
        {
            Console.Write($"длина - {arr.Length}; ");
            Console.Write($"сумма - {arr.Sum()}; ");
            Console.Write($"массив - [{string.Join(' ', arr)}]");
            Console.WriteLine();
        }
    }

    private void Sort(int[][] array, ref int iters)
    {
        ShellSort(array, ref iters);
    }

    private void ShellSort(int[][] array, ref int iters)
    {
        int h = 1;
        while (h < array.Length / 3)
            h = 3 * h + 1;

        while (h > 0)
        {
            for (int i = h; i < array.Length; i++)
            {
                int[] temp = array[i];

                int j = i;
                while (j >= h && ShouldSwap(array[j - h], temp, ref iters))
                {
                    array[j] = array[j - h];
                    j -= h;
                }

                array[j] = temp;
            }

            h /= 3;
        }
    }

    private bool ShouldSwap(int[] arr1, int[] arr2, ref int iters)
    {
        iters++;
        if (arr1.Length < arr2.Length) return true;
        if (arr1.Length > arr2.Length) return false;
        return arr1.Sum() < arr2.Sum();
    }

    public void SortTaxiArray(int[][] arrTaxi, ref int iterationCounter)
    {
        Sort(arrTaxi, ref iterationCounter);
    }
}
