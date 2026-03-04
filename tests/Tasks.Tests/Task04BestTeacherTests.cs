using Task04.BestTeacher;

namespace Tasks.Tests;

public class Task04BestTeacherTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new BestTeacherSolution()];
        yield return [new BestTeacherAlt()];
    }

    private static void Find(IBestTeacherSolution s, double[,] m, out int idx, out double avg)
        => s.FindBestTeacher(m, out idx, out avg);

    // ─── Пример из условия ────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TaskExample_Index3_Average4_15(IBestTeacherSolution s)
    {
        double[,] marks = {
            {3.6, 3.1, 2.8, 1,   4,   3.3, 3.2, 3  },
            {3.5, 3.6, 4.1, 3.9, 3.5, 5,   4,   5  },
            {2.2, 2.7, 3.1, 3,   4.5, 2.2, 3.1, 3.7},
            {4.2, 3.4, 3,   4.3, 4.1, 4.6, 4.4, 4.5},
            {4.7, 4.1, 3.6, 2.1, 2.7, 2,   2.5, 2.7}
        };
        Find(s, marks, out int idx, out double avg);
        Assert.Equal(3, idx);
        Assert.Equal(4.15, avg, 2);
    }

    // ─── Позиция лучшего ──────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BestTeacher_IsFirst(IBestTeacherSolution s)
    {
        double[,] marks = { {5, 4, 3, 2}, {4, 3, 2, 1} };
        Find(s, marks, out int idx, out double avg);
        Assert.Equal(0, idx);
        Assert.Equal(3.5, avg, 2);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BestTeacher_IsLast(IBestTeacherSolution s)
    {
        double[,] marks = { {4, 3, 2, 1}, {5, 4, 3, 2} };
        Find(s, marks, out int idx, out double avg);
        Assert.Equal(1, idx);
        Assert.Equal(3.5, avg, 2);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BestTeacher_IsMiddle(IBestTeacherSolution s)
    {
        double[,] marks = { {4, 3, 2, 1}, {5, 4, 3, 2}, {3, 2, 1, 0} };
        Find(s, marks, out int idx, out double avg);
        Assert.Equal(1, idx);
        Assert.Equal(3.5, avg, 2);
    }

    // ─── При равенстве побеждает первый ───────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TiedTeachers_FirstWins(IBestTeacherSolution s)
    {
        double[,] marks = { {5, 4, 3, 2}, {5, 4, 3, 2} };
        Find(s, marks, out int idx, out _);
        Assert.Equal(0, idx);
    }

    // ─── Один преподаватель ───────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SingleTeacher_ReturnsIndex0(IBestTeacherSolution s)
    {
        double[,] marks = { {5, 4, 3, 2, 1} };
        Find(s, marks, out int idx, out double avg);
        Assert.Equal(0, idx);
        Assert.Equal(3.0, avg, 2);
    }

    // ─── Порядок оценок не влияет ─────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MarkOrder_DoesNotMatter(IBestTeacherSolution s)
    {
        double[,] asc  = { {1, 2, 3, 4, 5} };
        double[,] desc = { {5, 4, 3, 2, 1} };
        double[,] mix  = { {3, 1, 5, 2, 4} };

        Find(s, asc,  out _, out double avgAsc);
        Find(s, desc, out _, out double avgDesc);
        Find(s, mix,  out _, out double avgMix);

        Assert.Equal(3.0, avgAsc,  2);
        Assert.Equal(3.0, avgDesc, 2);
        Assert.Equal(3.0, avgMix,  2);
    }

    // ─── Дубликаты max/min — исключается по одному ────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void DuplicateMax_OnlyOneExcluded(IBestTeacherSolution s)
    {
        double[,] marks = { {5, 5, 3, 1} };  // (14-5-1)/2 = 4.0
        Find(s, marks, out _, out double avg);
        Assert.Equal(4.0, avg, 2);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void DuplicateMin_OnlyOneExcluded(IBestTeacherSolution s)
    {
        double[,] marks = { {5, 3, 1, 1} };  // (10-5-1)/2 = 2.0
        Find(s, marks, out _, out double avg);
        Assert.Equal(2.0, avg, 2);
    }

    // ─── Некорректные данные → false ──────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TooFewMarks_ReturnsFalse(IBestTeacherSolution s)
    {
        double[,] marks = { {4, 5} };  // только 2 студента — меньше минимума 3
        Assert.False(s.FindBestTeacher(marks, out _, out _));
    }

    // ─── Большой пример ───────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ManyTeachers_CorrectBest(IBestTeacherSolution s)
    {
        double[,] marks = {
            {1, 1, 1, 1, 1},     // avg = 1.0
            {2, 2, 2, 2, 2},     // avg = 2.0
            {3, 3, 3, 3, 3},     // avg = 3.0
            {5, 5, 4, 3, 1},     // (18-5-1)/3 = 4.0
        };
        Find(s, marks, out int idx, out double avg);
        Assert.Equal(3, idx);
        Assert.Equal(4.0, avg, 2);
    }
}
