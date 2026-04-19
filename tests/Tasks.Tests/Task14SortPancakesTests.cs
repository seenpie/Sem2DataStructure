using Task14.SortPancakes;

namespace Tasks.Tests;

public class Task14SortPancakesTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new SortPancakesSolution()];
    }

    private static List<int> Run(ISortPancakesSolution s, List<int> pancakes)
    {
        s.SortPancakes(pancakes);
        return pancakes;
    }

    private static void AssertSortedDescending(List<int> pancakes)
    {
        for (int i = 0; i < pancakes.Count - 1; i++)
        {
            Assert.True(pancakes[i] >= pancakes[i + 1],
                $"Элементы не в порядке убывания: pancakes[{i}]={pancakes[i]}, pancakes[{i + 1}]={pancakes[i + 1]}");
        }
    }

    // ─── Базовые тесты (примеры из задания) ────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Example1_UnsortedList_SortsDescending(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 4, 1, 7, 3, 2, 4, 8, 5, 6 };
        var expected = new List<int> { 8, 7, 6, 5, 4, 4, 3, 2, 1 };

        Run(s, pancakes);

        Assert.Equal(expected, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Example2_AscendingList_SortsDescending(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 1, 2, 3, 4, 4, 5, 6, 7, 8 };
        var expected = new List<int> { 8, 7, 6, 5, 4, 4, 3, 2, 1 };

        Run(s, pancakes);

        Assert.Equal(expected, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Example3_AlreadyDescending_Unchanged(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 8, 7, 6, 5, 4, 4, 3, 2, 1 };
        var expected = new List<int> { 8, 7, 6, 5, 4, 4, 3, 2, 1 };

        Run(s, pancakes);

        Assert.Equal(expected, pancakes);
    }

    // ─── Вырожденные случаи ────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void EmptyList_RemainsEmpty(ISortPancakesSolution s)
    {
        var pancakes = new List<int>();

        Run(s, pancakes);

        Assert.Empty(pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SingleElement_Unchanged(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 42 };

        Run(s, pancakes);

        Assert.Single(pancakes);
        Assert.Equal(42, pancakes[0]);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TwoElements_AlreadyDescending_Unchanged(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 5, 2 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 5, 2 }, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TwoElements_Ascending_Swapped(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 2, 5 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 5, 2 }, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TwoElementsEqual_Unchanged(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 7, 7 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 7, 7 }, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ThreeElements_AllPermutations_SortedCorrectly(ISortPancakesSolution s)
    {
        int[][] permutations =
        [
            [1, 2, 3], [1, 3, 2], [2, 1, 3],
            [2, 3, 1], [3, 1, 2], [3, 2, 1],
        ];

        foreach (var perm in permutations)
        {
            var pancakes = new List<int>(perm);
            Run(s, pancakes);
            Assert.Equal(new List<int> { 3, 2, 1 }, pancakes);
        }
    }

    // ─── Повторяющиеся элементы ───────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void AllElementsEqual_Unchanged(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 5, 5, 5, 5, 5, 5 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 5, 5, 5, 5, 5, 5 }, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ManyDuplicates_SortedDescending(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 3, 1, 2, 3, 1, 2, 3, 1, 2 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 3, 3, 3, 2, 2, 2, 1, 1, 1 }, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TwoDistinctValues_SortedDescending(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 1, 2, 1, 2, 1, 2, 1 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 2, 2, 2, 1, 1, 1, 1 }, pancakes);
    }

    // ─── Позиции максимума ─────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MaxAtBeginning_SortedDescending(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 10, 2, 5, 3, 1 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 10, 5, 3, 2, 1 }, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MaxAtEnd_SortedDescending(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 2, 5, 3, 1, 10 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 10, 5, 3, 2, 1 }, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MaxInMiddle_SortedDescending(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 2, 5, 10, 3, 1 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 10, 5, 3, 2, 1 }, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MinAtBeginning_SortedDescending(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 1, 5, 3, 4, 2 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 5, 4, 3, 2, 1 }, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MinAtEnd_SortedDescending(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 5, 3, 4, 2, 1 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 5, 4, 3, 2, 1 }, pancakes);
    }

    // ─── Диапазоны значений ────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NegativeValues_SortedDescending(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { -3, -1, -7, -2, -5 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { -1, -2, -3, -5, -7 }, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MixedPositiveAndNegative_SortedDescending(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { -5, 3, -10, 0, 7, -2, 4 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 7, 4, 3, 0, -2, -5, -10 }, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ZerosAndPositives_SortedDescending(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 0, 3, 0, 1, 0, 2 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 3, 2, 1, 0, 0, 0 }, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void AllZeros_Unchanged(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 0, 0, 0, 0 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 0, 0, 0, 0 }, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void IntMinAndMax_SortedDescending(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 0, int.MaxValue, -1, int.MinValue, 1 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { int.MaxValue, 1, 0, -1, int.MinValue }, pancakes);
    }

    // ─── Свойства алгоритма ────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SortsInPlace_SameListReference(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 3, 1, 4, 1, 5, 9, 2, 6 };
        var reference = pancakes;

        s.SortPancakes(pancakes);

        Assert.Same(reference, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void PreservesCount(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3 };

        Run(s, pancakes);

        Assert.Equal(10, pancakes.Count);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void PreservesMultiset_NoElementsLostOrAdded(ISortPancakesSolution s)
    {
        var original = new List<int> { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5 };
        var pancakes = new List<int>(original);

        Run(s, pancakes);

        Assert.Equal(
            original.OrderBy(x => x).ToList(),
            pancakes.OrderBy(x => x).ToList());
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ResultIsSortedDescending(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 17, 3, 25, 8, 1, 42, 11, 6, 30, 19, 4, 22 };

        Run(s, pancakes);

        AssertSortedDescending(pancakes);
    }

    // ─── Особые конфигурации ───────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void AlmostSortedDescending_SortedCorrectly(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 10, 9, 8, 6, 7, 5, 4, 3, 2, 1 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 }, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void AlmostSortedAscending_SortedCorrectly(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 1, 2, 3, 4, 6, 5, 7, 8, 9, 10 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 }, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void PairwiseSwapped_SortedCorrectly(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 2, 1, 4, 3, 6, 5, 8, 7 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 8, 7, 6, 5, 4, 3, 2, 1 }, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MaxAppearsMultipleTimes_AllMaxesAtFront(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 1, 5, 2, 5, 3, 5, 4 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 5, 5, 5, 4, 3, 2, 1 }, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MinAppearsMultipleTimes_AllMinsAtEnd(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 1, 5, 1, 3, 1, 4, 1, 2 };

        Run(s, pancakes);

        Assert.Equal(new List<int> { 5, 4, 3, 2, 1, 1, 1, 1 }, pancakes);
    }

    // ─── Проверка на случайных данных ──────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void RandomData_MatchesBuiltInDescendingSort(ISortPancakesSolution s)
    {
        var random = new Random(42);
        var pancakes = new List<int>();
        for (int i = 0; i < 100; i++)
        {
            pancakes.Add(random.Next(-500, 500));
        }
        var expected = pancakes.OrderByDescending(x => x).ToList();

        Run(s, pancakes);

        Assert.Equal(expected, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void LargeRandomData_MatchesBuiltInDescendingSort(ISortPancakesSolution s)
    {
        var random = new Random(12345);
        var pancakes = new List<int>();
        for (int i = 0; i < 1000; i++)
        {
            pancakes.Add(random.Next(0, 10_000));
        }
        var expected = pancakes.OrderByDescending(x => x).ToList();

        Run(s, pancakes);

        Assert.Equal(expected, pancakes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ManyDuplicatesFromSmallAlphabet_MatchesBuiltInDescendingSort(ISortPancakesSolution s)
    {
        var random = new Random(7);
        var pancakes = new List<int>();
        for (int i = 0; i < 200; i++)
        {
            pancakes.Add(random.Next(0, 5));
        }
        var expected = pancakes.OrderByDescending(x => x).ToList();

        Run(s, pancakes);

        Assert.Equal(expected, pancakes);
    }

    // ─── Идемпотентность ───────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SortingTwice_GivesSameResult(ISortPancakesSolution s)
    {
        var pancakes = new List<int> { 4, 1, 7, 3, 2, 4, 8, 5, 6 };

        Run(s, pancakes);
        var afterFirstSort = new List<int>(pancakes);
        Run(s, pancakes);

        Assert.Equal(afterFirstSort, pancakes);
    }
}
