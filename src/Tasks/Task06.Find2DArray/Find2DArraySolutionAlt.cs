using Tasks.Common;

namespace Task06.Find2DArray;

[Task(6, "Поиск в 2D-массиве", "alt")]
public class Find2DArraySolutionAlt : IFind2DArraySolution
{
    public void Run()
    {
        int[,] test1 = { { 2, 6, 7, 9, 9, 14}, {18, 20, 26, 26, 29, 40 }, { 44, 47, 50, 51, 55, 62} };
        int[,] test2 = { { 2, 6, 7, 9, 9, 14}, {18, 20, 26, 26, 40, 40 }, { 44, 47, 50, 51, 55, 62} };
        int[,] test3 = { {1, 2, 3}, {4, 5, 6}, {7, 8, 9} };
        (int[,], int)[] tests = [(test1, 18), (test2, 40), (test3, 1)];

        foreach (var (test, value) in tests)
        {
            Console.WriteLine(FindNumber(test, value, out int[] indexes)
                ? $"индексы - [{indexes[0]}, {indexes[1]}]"
                : "В массиве нет указанного значения");
        }
    }

    public bool FindNumber(int[,] array, int value, out int[] indexes)
    {
        indexes = [-1, -1];
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        if (rows < 1 || cols < 1) return false;

        int leftRow = 0;
        int rightRow = rows - 1;

        while (leftRow <= rightRow)
        {
            int midRow = (rightRow + leftRow) / 2;

            // проверка: есть ли искомое значение в диапазоне конкретной строки
            if (value < array[midRow, 0])
            {
                rightRow = midRow - 1;
                continue;
            }

            if (value > array[midRow, cols - 1])
            {
                leftRow = midRow + 1;
                continue;
            }

            int leftCol = 0;
            int rightCol = cols - 1;

            while (leftCol <= rightCol)
            {
                int midCol = (leftCol + rightCol) / 2;

                if (array[midRow, midCol] > value) rightCol = midCol - 1;
                else if (array[midRow, midCol] < value) leftCol = midCol + 1;
                else
                {
                    indexes = [midRow, midCol];
                    return true;
                }
            }

            // если мы тут, значит значения вообще нет в представленном массиве
            break;
        }

        return false;
    }
}
