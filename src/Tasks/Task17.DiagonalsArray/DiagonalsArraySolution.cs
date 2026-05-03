using Tasks.Common;

namespace Task17.DiagonalsArray;

[Task(17, "Диагонали в массиве")]
public class DiagonalsArraySolution : IDiagonalsArraySolution
{
    public void Run()
    {
        int[,] array = GenerateDiagonalsArray(4, 6);
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                Console.Write(array[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    public int[,] GenerateDiagonalsArray(int n, int m)
    {
        int num = 1;
        int[,] array = new int[n, m];
        for(int d = 0; d < n + m - 1; d++)
        {
            int min = Math.Max(0, d - (m - 1));
            int max = Math.Min(n - 1, d);
            for (int i = min; i <= max; i++)
            {
                int j = d - i;
                array[i, j] = num++;
            }
        }
        return array;
    }
}
