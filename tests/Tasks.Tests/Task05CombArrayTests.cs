using Task05.CombArray;

namespace Tasks.Tests;

public class Task05CombArrayTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new CombArraySolution()];
        yield return [new CombArraySolutionAlt()];
    }

    private static int[] Run(ICombArraySolution s, int[] input)
        => s.FilterNonDecreasing(input);

    // ─── Пример из условия ────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TaskExample(ICombArraySolution s)
    {
        Assert.Equal(
            new[] { -6, -2, 0, 6, 11, 11, 14 },
            Run(s, [-6, -2, -3, 0, 6, 1, 3, 11, 11, 14]));
    }

    // ─── Базовые случаи ───────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void AlreadySorted_AllKept(ICombArraySolution s)
        => Assert.Equal(new[] {1, 2, 3, 4, 5}, Run(s, [1, 2, 3, 4, 5]));

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Descending_OnlyFirstKept(ICombArraySolution s)
        => Assert.Equal(new[] {5}, Run(s, [5, 4, 3, 2, 1]));

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SingleElement_Returned(ICombArraySolution s)
        => Assert.Equal(new[] {42}, Run(s, [42]));

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void EmptyArray_Returned(ICombArraySolution s)
        => Assert.Empty(Run(s, []));

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void AllEqual_AllKept(ICombArraySolution s)
        => Assert.Equal(new[] {3, 3, 3, 3}, Run(s, [3, 3, 3, 3]));

    // ─── Провалы и восстановления ─────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void DipThenFullRecovery_BothKept(ICombArraySolution s)
        => Assert.Equal(new[] {3, 5}, Run(s, [3, 1, 5]));

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void DipThenPartialRecovery_OnlyPeakKept(ICombArraySolution s)
        => Assert.Equal(new[] {10}, Run(s, [10, 3, 7]));

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MultipleDips_CorrectResult(ICombArraySolution s)
        => Assert.Equal(new[] {1, 5, 6}, Run(s, [1, 5, 3, 4, 2, 6]));

    // ─── Отрицательные и нули ─────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NegativeNumbers(ICombArraySolution s)
        => Assert.Equal(new[] {-5, -3, -1}, Run(s, [-5, -3, -4, -1]));

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ZerosKept(ICombArraySolution s)
        => Assert.Equal(new[] {-2, 0, 0, 1}, Run(s, [-2, 0, 0, 1]));

    // ─── Инварианты ───────────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Result_IsNonDecreasing(ICombArraySolution s)
    {
        int[] result = Run(s, [-6, -2, -3, 0, 6, 1, 3, 11, 11, 14]);
        for (int i = 1; i < result.Length; i++)
            Assert.True(result[i] >= result[i - 1]);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ResultElements_SubsetOfInput(ICombArraySolution s)
    {
        int[] input  = [-6, -2, -3, 0, 6, 1, 3, 11, 11, 14];
        int[] result = Run(s, input);
        var inputList = input.ToList();
        foreach (int x in result)
        {
            Assert.Contains(x, inputList);
            inputList.Remove(x);
        }
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ResultLength_LessOrEqualInput(ICombArraySolution s)
    {
        int[] input = [-6, -2, -3, 0, 6, 1, 3, 11, 11, 14];
        Assert.True(Run(s, input).Length <= input.Length);
    }
}
