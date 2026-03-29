using Task10.SortJaggedArray;

namespace Tasks.Tests;

public class Task10SortJaggedArrayTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new SortJaggedArraySolution()];
        yield return [new SortJaggedArraySolutionAlt()];
    }

    private static int[][] Sort(ISortJaggedArraySolution s, int[][] arr)
    {
        int counter = 0;
        s.SortTaxiArray(arr, ref counter);
        return arr;
    }

    // ─── Примеры из условия ───────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TaskExample1_SortedByLengthThenSum(ISortJaggedArraySolution s)
    {
        int[][] input =
        [
            [100, 289, 200, 101, 90, 230],        // len=6, sum=1010
            [290, 300, 303, 120, 150],             // len=5, sum=1163
            [80],                                  // len=1, sum=80
            [300, 60, 120, 400, 410],              // len=5, sum=1290
            [60, 100, 40],                         // len=3, sum=200
            [60, 160, 165, 120, 110, 230, 200, 30],// len=8, sum=1075
            [230, 200, 250, 100],                  // len=4, sum=780
            [100, 209, 175, 100],                  // len=4, sum=584
            [70, 120, 290],                        // len=3, sum=480
            [90, 80, 105, 140, 120],               // len=5, sum=535
        ];

        int[][] result = Sort(s, input);

        int[] expectedLengths = [8, 6, 5, 5, 5, 4, 4, 3, 3, 1];
        int[] expectedSums = [1075, 1010, 1290, 1163, 535, 780, 584, 480, 200, 80];

        for (int i = 0; i < result.Length; i++)
        {
            Assert.Equal(expectedLengths[i], result[i].Length);
            Assert.Equal(expectedSums[i], result[i].Sum());
        }
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TaskExample2_AlreadyInCorrectOrder_RemainsCorrect(ISortJaggedArraySolution s)
    {
        int[][] input =
        [
            [80],                                  // len=1
            [60, 100, 40],                         // len=3
            [70, 120, 290],                        // len=3
            [100, 209, 175, 100],                  // len=4
            [230, 200, 250, 100],                  // len=4
            [90, 80, 105, 140, 120],               // len=5
            [290, 300, 303, 120, 150],             // len=5
            [300, 60, 120, 400, 410],              // len=5
            [100, 289, 200, 101, 90, 230],         // len=6
            [60, 160, 165, 120, 110, 230, 200, 30],// len=8
        ];

        int[][] result = Sort(s, input);

        int[] expectedLengths = [8, 6, 5, 5, 5, 4, 4, 3, 3, 1];
        int[] expectedSums = [1075, 1010, 1290, 1163, 535, 780, 584, 480, 200, 80];

        for (int i = 0; i < result.Length; i++)
        {
            Assert.Equal(expectedLengths[i], result[i].Length);
            Assert.Equal(expectedSums[i], result[i].Sum());
        }
    }

    // ─── Сортировка по длине ───────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void LongerRowsFirst(ISortJaggedArraySolution s)
    {
        int[][] input = [[1], [1, 2], [1, 2, 3]];
        int[][] result = Sort(s, input);

        Assert.Equal([1, 2, 3], result[0]);
        Assert.Equal([1, 2], result[1]);
        Assert.Equal([1], result[2]);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void AlreadySortedByLength_NoChange(ISortJaggedArraySolution s)
    {
        int[][] input = [[10, 20, 30], [5, 10], [1]];
        int[][] result = Sort(s, input);

        Assert.Equal(3, result[0].Length);
        Assert.Equal(2, result[1].Length);
        Assert.Equal(1, result[2].Length);
    }

    // ─── Сортировка по сумме при одинаковой длине ─────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void EqualLengths_SortedBySumDescending(ISortJaggedArraySolution s)
    {
        int[][] input = [[1, 2], [50, 60], [10, 20]];
        int[][] result = Sort(s, input);

        Assert.Equal(110, result[0].Sum());
        Assert.Equal(30, result[1].Sum());
        Assert.Equal(3, result[2].Sum());
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void EqualLengthsAlreadySorted_NoChange(ISortJaggedArraySolution s)
    {
        int[][] input = [[100, 200], [50, 60], [1, 2]];
        int[][] result = Sort(s, input);

        Assert.Equal(300, result[0].Sum());
        Assert.Equal(110, result[1].Sum());
        Assert.Equal(3, result[2].Sum());
    }

    // ─── Смешанные критерии ────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void LengthTakesPriorityOverSum(ISortJaggedArraySolution s)
    {
        // Короткий с большой суммой должен оказаться после длинного с малой суммой
        int[][] input = [[1000], [1, 2]];
        int[][] result = Sort(s, input);

        Assert.True(result[0].Length == 2); // [1, 2] — длиннее
        Assert.True(result[1].Length == 1); // [1000] — хоть и больше сумма
    }

    // ─── Граничные случаи ─────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SingleRow_Unchanged(ISortJaggedArraySolution s)
    {
        int[][] input = [[5, 10, 15]];
        int[][] result = Sort(s, input);

        int[] only = Assert.Single(result);
        Assert.Equal([5, 10, 15], only);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TwoRows_CorrectOrder(ISortJaggedArraySolution s)
    {
        int[][] input = [[1], [2, 3]];
        int[][] result = Sort(s, input);

        Assert.True(result[0].Length == 2);
        Assert.True(result[1].Length == 1);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void AllRowsSameLengthAndSum_OrderIsIrrelevant_ButCountCorrect(ISortJaggedArraySolution s)
    {
        int[][] input = [[1, 2], [2, 1], [3, 0]];
        int[][] result = Sort(s, input);

        Assert.All(result, row => Assert.Equal(2, row.Length));
        Assert.All(result, row => Assert.Equal(3, row.Sum()));
    }

    // ─── Счётчик итераций ─────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void IterationCounter_SingleRow_IsZero(ISortJaggedArraySolution s)
    {
        int[][] input = [[1, 2, 3]];
        int counter = 0;
        s.SortTaxiArray(input, ref counter);

        Assert.Equal(0, counter);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void IterationCounter_MultipleRows_IsPositive(ISortJaggedArraySolution s)
    {
        int[][] input = [[1], [2, 3], [4, 5, 6]];
        int counter = 0;
        s.SortTaxiArray(input, ref counter);

        Assert.True(counter > 0);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void IterationCounter_TwoRows_IsOne(ISortJaggedArraySolution s)
    {
        int[][] input = [[1], [2, 3]];
        int counter = 0;
        s.SortTaxiArray(input, ref counter);

        Assert.Equal(1, counter);
    }

    // ─── Инварианты ───────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Result_SortedByLengthDescending(ISortJaggedArraySolution s)
    {
        int[][] input =
        [
            [1, 2],
            [1],
            [1, 2, 3, 4],
            [5, 6, 7],
        ];
        int[][] result = Sort(s, input);

        for (int i = 1; i < result.Length; i++)
            Assert.True(result[i].Length <= result[i - 1].Length);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Result_EqualLengthGroups_SortedBySumDescending(ISortJaggedArraySolution s)
    {
        int[][] input =
        [
            [1, 5],
            [3, 8],
            [2, 2],
            [10, 4],
        ];
        int[][] result = Sort(s, input);

        for (int i = 1; i < result.Length; i++)
            if (result[i].Length == result[i - 1].Length)
                Assert.True(result[i].Sum() <= result[i - 1].Sum());
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Result_ContainsSameRows(ISortJaggedArraySolution s)
    {
        int[][] input = [[100], [10, 20], [5, 5, 5]];
        var inputSums = input.Select(r => r.Sum()).OrderBy(x => x).ToList();

        int[][] result = Sort(s, input);
        var resultSums = result.Select(r => r.Sum()).OrderBy(x => x).ToList();

        Assert.Equal(inputSums, resultSums);
    }
}
