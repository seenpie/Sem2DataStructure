using Task06.Find2DArray;

namespace Tasks.Tests;

public class Task06Find2DArrayTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new Find2DArraySolution()];
        yield return [new Find2DArraySolutionAlt()];
    }

    public static IEnumerable<object[]> SolutionsWithValues()
    {
        int[,] arr = { {2,6,7,9,9,14}, {18,20,26,26,29,40}, {44,47,50,51,55,62} };
        foreach (var s in new IFind2DArraySolution[] { new Find2DArraySolution() })
            foreach (int num in new[] { 2, 18, 29, 62 })
                yield return [s, arr, num];
    }

    private static bool Find(IFind2DArraySolution s, int[,] arr, int num, out int[] idx)
        => s.FindNumber(arr, num, out idx);

    // ─── Примеры из условия ───────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TaskExample1_Num18(IFind2DArraySolution s)
    {
        int[,] arr = { {2,6,7,9,9,14}, {18,20,26,26,29,40}, {44,47,50,51,55,62} };
        Assert.True(Find(s, arr, 18, out int[] idx));
        Assert.Equal([1, 0], idx);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TaskExample2_Num40(IFind2DArraySolution s)
    {
        int[,] arr = { {2,6,7,9,9,14}, {18,20,26,26,40,40}, {44,47,50,51,55,62} };
        Assert.True(Find(s, arr, 40, out int[] idx));
        Assert.Equal([1, 4], idx);
    }

    // ─── Found / NotFound ─────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NotFound_ReturnsFalseAndMinusOne(IFind2DArraySolution s)
    {
        int[,] arr = { {1, 3, 5}, {7, 9, 11} };
        Assert.False(Find(s, arr, 6, out int[] idx));
        Assert.Equal([-1, -1], idx);
    }

    // ─── Позиции ──────────────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FirstElement(IFind2DArraySolution s)
    {
        int[,] arr = { {1,2,3}, {4,5,6}, {7,8,9} };
        Assert.True(Find(s, arr, 1, out int[] idx));
        Assert.Equal([0, 0], idx);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void LastElement(IFind2DArraySolution s)
    {
        int[,] arr = { {1,2,3}, {4,5,6}, {7,8,9} };
        Assert.True(Find(s, arr, 9, out int[] idx));
        Assert.Equal([2, 2], idx);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MiddleElement(IFind2DArraySolution s)
    {
        int[,] arr = { {1,2,3}, {4,5,6}, {7,8,9} };
        Assert.True(Find(s, arr, 5, out int[] idx));
        Assert.Equal([1, 1], idx);
    }

    // ─── Граничные массивы ────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SingleElement_Found(IFind2DArraySolution s)
    {
        int[,] arr = { {42} };
        Assert.True(Find(s, arr, 42, out int[] idx));
        Assert.Equal([0, 0], idx);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SingleElement_NotFound(IFind2DArraySolution s)
    {
        int[,] arr = { {42} };
        Assert.False(Find(s, arr, 0, out int[] idx));
        Assert.Equal([-1, -1], idx);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SingleRow_Found(IFind2DArraySolution s)
    {
        int[,] arr = { {1,3,5,7,9} };
        Assert.True(Find(s, arr, 5, out int[] idx));
        Assert.Equal([0, 2], idx);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SingleColumn_Found(IFind2DArraySolution s)
    {
        int[,] arr = { {1}, {3}, {5}, {7} };
        Assert.True(Find(s, arr, 7, out int[] idx));
        Assert.Equal([3, 0], idx);
    }

    // ─── Значения вне диапазона ───────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ValueSmallerThanMin_NotFound(IFind2DArraySolution s)
    {
        int[,] arr = { {5,10,15}, {20,25,30} };
        Assert.False(Find(s, arr, 1, out _));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ValueLargerThanMax_NotFound(IFind2DArraySolution s)
    {
        int[,] arr = { {5,10,15}, {20,25,30} };
        Assert.False(Find(s, arr, 100, out _));
    }

    // ─── Отрицательные числа ──────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NegativeNumbers_Found(IFind2DArraySolution s)
    {
        int[,] arr = { {-10,-5,-3}, {-1,0,4}, {7,12,20} };
        Assert.True(Find(s, arr, -5, out int[] idx));
        Assert.Equal([0, 1], idx);
    }

    // ─── Найденный индекс указывает на правильное значение ───────────────────────

    [Theory]
    [MemberData(nameof(SolutionsWithValues))]
    public void FoundIndex_PointsToCorrectValue(IFind2DArraySolution s, int[,] arr, int num)
    {
        Assert.True(Find(s, arr, num, out int[] idx));
        Assert.Equal(num, arr[idx[0], idx[1]]);
    }

    // ─── Большой массив ───────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void LargeArray_CorrectResult(IFind2DArraySolution s)
    {
        int[,] arr = new int[10, 10];
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
                arr[i, j] = i * 10 + j + 1;

        Assert.True(Find(s, arr, 55, out int[] idx));
        Assert.Equal([5, 4], idx);

        Assert.False(Find(s, arr, 101, out _));
    }
}
