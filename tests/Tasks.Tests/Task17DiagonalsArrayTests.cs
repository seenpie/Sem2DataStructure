using Task17.DiagonalsArray;

namespace Tasks.Tests;

public class Task17DiagonalsArrayTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new DiagonalsArraySolution()];
    }

    // ---------- Граничные случаи ----------

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void OneByOne_SingleCell(IDiagonalsArraySolution s)
    {
        int[,] expected = { { 1 } };
        AssertEqual(expected, s.GenerateDiagonalsArray(1, 1));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void OneRow_FillsLeftToRight(IDiagonalsArraySolution s)
    {
        // 1×5 — все диагонали по 1 элементу, заполнение слева направо
        int[,] expected = { { 1, 2, 3, 4, 5 } };
        AssertEqual(expected, s.GenerateDiagonalsArray(1, 5));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void OneColumn_FillsTopToBottom(IDiagonalsArraySolution s)
    {
        // 5×1 — все диагонали по 1 элементу, заполнение сверху вниз
        int[,] expected = {
            { 1 },
            { 2 },
            { 3 },
            { 4 },
            { 5 },
        };
        AssertEqual(expected, s.GenerateDiagonalsArray(5, 1));
    }

    // ---------- Простые квадратные матрицы ----------

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TwoByTwo(IDiagonalsArraySolution s)
    {
        int[,] expected = {
            { 1, 2 },
            { 3, 4 },
        };
        AssertEqual(expected, s.GenerateDiagonalsArray(2, 2));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ThreeByThree(IDiagonalsArraySolution s)
    {
        // Диагонали:
        //  d=0: (0,0)=1
        //  d=1: (0,1)=2, (1,0)=3
        //  d=2: (0,2)=4, (1,1)=5, (2,0)=6
        //  d=3: (1,2)=7, (2,1)=8
        //  d=4: (2,2)=9
        int[,] expected = {
            { 1, 2, 4 },
            { 3, 5, 7 },
            { 6, 8, 9 },
        };
        AssertEqual(expected, s.GenerateDiagonalsArray(3, 3));
    }

    // ---------- Прямоугольные матрицы (широкие и высокие) ----------

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TwoByThree_Wide(IDiagonalsArraySolution s)
    {
        //  d=0: (0,0)=1
        //  d=1: (0,1)=2, (1,0)=3
        //  d=2: (0,2)=4, (1,1)=5
        //  d=3: (1,2)=6
        int[,] expected = {
            { 1, 2, 4 },
            { 3, 5, 6 },
        };
        AssertEqual(expected, s.GenerateDiagonalsArray(2, 3));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ThreeByTwo_Tall(IDiagonalsArraySolution s)
    {
        //  d=0: (0,0)=1
        //  d=1: (0,1)=2, (1,0)=3
        //  d=2: (1,1)=4, (2,0)=5
        //  d=3: (2,1)=6
        int[,] expected = {
            { 1, 2 },
            { 3, 4 },
            { 5, 6 },
        };
        AssertEqual(expected, s.GenerateDiagonalsArray(3, 2));
    }

    // ---------- Большой пример из условия (картинка) ----------

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FourBySix_TaskExampleFromPicture(IDiagonalsArraySolution s)
    {
        // Эталонная матрица из условия (arrdiag.jpg)
        int[,] expected = {
            { 1,  2,  4,  7, 11, 15 },
            { 3,  5,  8, 12, 16, 19 },
            { 6,  9, 13, 17, 20, 22 },
            { 10, 14, 18, 21, 23, 24 },
        };
        AssertEqual(expected, s.GenerateDiagonalsArray(4, 6));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SixByFour_TransposedSize(IDiagonalsArraySolution s)
    {
        // 6×4 — поменяли n и m местами относительно картинки
        int[,] expected = {
            { 1,  2,  4,  7 },
            { 3,  5,  8, 11 },
            { 6,  9, 12, 15 },
            { 10, 13, 16, 19 },
            { 14, 17, 20, 22 },
            { 18, 21, 23, 24 },
        };
        AssertEqual(expected, s.GenerateDiagonalsArray(6, 4));
    }

    // ---------- Инвариантные тесты (на любой матрице) ----------

    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 10)]
    [InlineData(10, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 5)]
    [InlineData(5, 3)]
    [InlineData(4, 6)]
    [InlineData(7, 7)]
    [InlineData(10, 15)]
    public void ResultHasCorrectDimensions(int n, int m)
    {
        DiagonalsArraySolution s = new DiagonalsArraySolution();
        int[,] result = s.GenerateDiagonalsArray(n, m);

        Assert.Equal(n, result.GetLength(0));
        Assert.Equal(m, result.GetLength(1));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 10)]
    [InlineData(10, 1)]
    [InlineData(3, 5)]
    [InlineData(5, 3)]
    [InlineData(4, 6)]
    [InlineData(7, 7)]
    public void EveryNumberFrom1ToNMAppearsExactlyOnce(int n, int m)
    {
        DiagonalsArraySolution s = new DiagonalsArraySolution();
        int[,] result = s.GenerateDiagonalsArray(n, m);

        bool[] seen = new bool[n * m + 1]; // индексы 1..n*m
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                int v = result[i, j];
                Assert.True(v >= 1 && v <= n * m,
                    $"Значение {v} в [{i},{j}] вне диапазона [1, {n * m}]");
                Assert.False(seen[v],
                    $"Значение {v} встречается повторно в [{i},{j}]");
                seen[v] = true;
            }
        }
    }

    [Theory]
    [InlineData(3, 5)]
    [InlineData(5, 3)]
    [InlineData(4, 6)]
    [InlineData(7, 7)]
    [InlineData(10, 15)]
    public void NumbersIncreaseAlongEachDiagonal(int n, int m)
    {
        // На одной диагонали (i+j = const) числа должны возрастать
        // при движении сверху вниз (с увеличением i).
        DiagonalsArraySolution s = new DiagonalsArraySolution();
        int[,] result = s.GenerateDiagonalsArray(n, m);

        for (int d = 0; d < n + m - 1; d++)
        {
            int iMin = Math.Max(0, d - (m - 1));
            int iMax = Math.Min(n - 1, d);

            for (int i = iMin; i < iMax; i++)
            {
                int j = d - i;
                int jNext = d - (i + 1);
                Assert.True(result[i, j] < result[i + 1, jNext],
                    $"Нарушение порядка на диагонали d={d}: " +
                    $"[{i},{j}]={result[i, j]} >= [{i + 1},{jNext}]={result[i + 1, jNext]}");
            }
        }
    }

    [Theory]
    [InlineData(3, 5)]
    [InlineData(5, 3)]
    [InlineData(4, 6)]
    public void DiagonalsAreFilledInOrder(int n, int m)
    {
        // Все числа на диагонали d должны быть строго меньше всех чисел на диагонали d+1.
        DiagonalsArraySolution s = new DiagonalsArraySolution();
        int[,] result = s.GenerateDiagonalsArray(n, m);

        for (int d = 0; d < n + m - 2; d++)
        {
            int maxOnD = MaxOnDiagonal(result, d, n, m);
            int minOnDNext = MinOnDiagonal(result, d + 1, n, m);
            Assert.True(maxOnD < minOnDNext,
                $"Диагональ d={d} содержит {maxOnD}, " +
                $"а d+1={d + 1} содержит {minOnDNext} — порядок нарушен");
        }
    }

    // ---------- Вспомогательные методы ----------

    private static void AssertEqual(int[,] expected, int[,] actual)
    {
        Assert.Equal(expected.GetLength(0), actual.GetLength(0));
        Assert.Equal(expected.GetLength(1), actual.GetLength(1));

        for (int i = 0; i < expected.GetLength(0); i++)
        {
            for (int j = 0; j < expected.GetLength(1); j++)
            {
                Assert.True(expected[i, j] == actual[i, j],
                    $"Несовпадение в [{i},{j}]: ожидалось {expected[i, j]}, получено {actual[i, j]}");
            }
        }
    }

    private static int MaxOnDiagonal(int[,] arr, int d, int n, int m)
    {
        int iMin = Math.Max(0, d - (m - 1));
        int iMax = Math.Min(n - 1, d);
        int max = arr[iMin, d - iMin];
        for (int i = iMin + 1; i <= iMax; i++)
        {
            int v = arr[i, d - i];
            if (v > max) max = v;
        }
        return max;
    }

    private static int MinOnDiagonal(int[,] arr, int d, int n, int m)
    {
        int iMin = Math.Max(0, d - (m - 1));
        int iMax = Math.Min(n - 1, d);
        int min = arr[iMin, d - iMin];
        for (int i = iMin + 1; i <= iMax; i++)
        {
            int v = arr[i, d - i];
            if (v < min) min = v;
        }
        return min;
    }
}
