using Tasks.Common;

namespace Task07.CalculateOddIndicesAverage;

[Task(7, "Вычислить в 2D массиве сред. ариф. элем. с нечёт. индексами")]
public class CalculateOddIndicesAverageSolution : ICalculateOddIndicesAverageSolution
{
    public void Run()
    {
        double[,] test1 = {{1, 2, 3, 4}, {1, 2, 3, 4}};
        double[,] test2 = new double[1, 5];
        double[,] test3 = new double[5, 1];
        double[,] test4 = new double[0, 0];
        double[,] test5 =
        {
            {10, 20, 30, 40, 50},
            { 1,  2,  3,  4,  5},
            {10, 20, 30, 40, 50},
            { 5, 10, 15, 20, 25}
        };

        Console.WriteLine($"Test 1: {CalculateOddIndicesAverage(test1)} (3)");
        Console.WriteLine($"Test 2: {CalculateOddIndicesAverage(test2)} (0)");
        Console.WriteLine($"Test 3: {CalculateOddIndicesAverage(test3)} (0)");
        Console.WriteLine($"Test 4: {CalculateOddIndicesAverage(test4)} (0)");
        Console.WriteLine($"Test 5: {CalculateOddIndicesAverage(test5)} (9)");
    }

    public double CalculateOddIndicesAverage(double[,] arr)
    {
        int count = 0;
        double sum = 0;
        int rows = arr.GetLength(0);
        int cols = arr.GetLength(1);

        if (rows <= 1 || cols <= 1) return sum;

        for (int i = 1; i < rows; i += 2)
        {
            for (int j = 1; j < cols; j += 2)
            {
                count++;
                sum += arr[i, j];
            }
        }

        return sum / count;
    }
}
