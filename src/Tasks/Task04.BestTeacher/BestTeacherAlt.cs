using Tasks.Common;

namespace Task04.BestTeacher;

[Task(4, "Лучший преподаватель", "alt")]
public class BestTeacherAlt: IBestTeacherSolution
{
    static double[,] arrMarks = {{3.6, 3.1, 2.8, 1, 4, 3.3, 3.2, 3 },
        {3.5, 3.6, 4.1, 3.9, 3.5, 5, 4, 5 },
        {2.2, 2.7, 3.1, 3, 4.5, 2.2, 3.1, 3.7},
        {4.2, 3.4, 3, 4.3, 4.1, 4.6, 4.4, 4.5},
        {4.7, 4.1, 3.6, 2.1, 2.7, 2, 2.5, 2.7},
        {1, 1.9, 3, 5, 5, 5, 5, 5}
    };

    public void Run()
    {
        int index = FindIndexWithMaxAverage(arrMarks, out double maxAverage);
        Console.WriteLine($"{maxAverage:F} {index}");
    }

    public bool FindBestTeacher(double[,] marks, out int index, out double average)
    {
        index = FindIndexWithMaxAverage(marks, out average);
        return index != -1 && marks.GetLength(1) > 2;
    }

    private static int FindIndexWithMaxAverage(double[,] data, out double maxAverage)
    {
        int index = -1;
        maxAverage = -1;

        int rows = data.GetLength(0);
        int cols = data.GetLength(1);

        if (cols == 0) return index;

        for (int i = 0; i < rows; i++)
        {
            double max = data[i, 0];
            double min = data[i, 0];
            double sum = data[i, 0];

            for (int j = 1; j < cols; j++)
            {
                if (max < data[i, j]) max = data[i, j];
                if (min > data[i, j]) min = data[i, j];
                sum += data[i, j];
            }

            double currAverage = CalculateAverage(sum, max, min, cols);

            if (maxAverage < currAverage)
            {
                maxAverage = currAverage;
                index = i;
            }
        }

        maxAverage = Math.Round(maxAverage, 2);
        return index;
    }

    private static double CalculateAverage(double sum, double max, double min, int length)
    {
        if (length > 2) return (sum - max - min) / (length - 2);
        return sum / length;
    }
}
