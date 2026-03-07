using Task08.ColorfulHouses;

namespace Tasks.Tests;

public class Task08ColorfulHousesTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new ColorfulHousesSolution()];
        yield return [new ColorfulHousesSolutionAlt()];
    }

    /// <summary>
    /// Комбинирует решения с тестовыми параметрами (h, w, colors, expectedSum).
    /// </summary>
    public static IEnumerable<object[]> GetSumCases()
    {
        foreach (var sol in GetSolutions())
        {
            IColorfulHousesSolution s = (IColorfulHousesSolution)sol[0];
            yield return [s, 1,   1,   1,  1];
            yield return [s, 1,   1,   3,  1];
            yield return [s, 3,   4,   5, 12];
            yield return [s, 10,  10,  7, 100];
            yield return [s, 100, 100, 13, 10000];
        }
    }

    // ── Базовые случаи ────────────────────────────────────────────────────

    /// <summary>2×3=6 клеток, 2 цвета → [3, 3].</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void EvenDistribution(IColorfulHousesSolution s)
    {
        Assert.Equal([3, 3], s.Solve(2, 3, 2));
    }

    /// <summary>Один цвет получает все клетки.</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SingleColor_GetsAllCells(IColorfulHousesSolution s)
    {
        Assert.Equal([6], s.Solve(2, 3, 1));
    }

    /// <summary>1×1 поле, 1 цвет → [1].</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void OneByOne_OneColor(IColorfulHousesSolution s)
    {
        Assert.Equal([1], s.Solve(1, 1, 1));
    }

    // ── Неравномерное распределение ───────────────────────────────────────

    /// <summary>6 клеток, 4 цвета: первые два получают +1 → [2, 2, 1, 1].</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void UnevenDistribution_ExtraCellsToFirstColors(IColorfulHousesSolution s)
    {
        Assert.Equal([2, 2, 1, 1], s.Solve(2, 3, 4));
    }

    /// <summary>9 клеток, 2 цвета → [5, 4].</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void OddTotal_TwoColors(IColorfulHousesSolution s)
    {
        Assert.Equal([5, 4], s.Solve(3, 3, 2));
    }

    /// <summary>5 клеток, 3 цвета → [2, 2, 1].</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FiveCells_ThreeColors(IColorfulHousesSolution s)
    {
        Assert.Equal([2, 2, 1], s.Solve(1, 5, 3));
    }

    /// <summary>7 клеток, 3 цвета → [3, 2, 2].</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SevenCells_ThreeColors(IColorfulHousesSolution s)
    {
        Assert.Equal([3, 2, 2], s.Solve(1, 7, 3));
    }

    /// <summary>10 клеток (2×5), 3 цвета → [4, 3, 3].</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TenCells_ThreeColors(IColorfulHousesSolution s)
    {
        Assert.Equal([4, 3, 3], s.Solve(2, 5, 3));
    }

    // ── Количество цветов равно числу клеток ──────────────────────────────

    /// <summary>2×2=4 клетки, 4 цвета → каждый ровно по 1.</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ColorsEqualCells_EachColorGetsOne(IColorfulHousesSolution s)
    {
        Assert.Equal([1, 1, 1, 1], s.Solve(2, 2, 4));
    }

    // ── Цветов больше, чем клеток ─────────────────────────────────────────

    /// <summary>3 клетки, 5 цветов → [1, 1, 1, 0, 0].</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MoreColorsThanCells_TrailingZeros(IColorfulHousesSolution s)
    {
        Assert.Equal([1, 1, 1, 0, 0], s.Solve(1, 3, 5));
    }

    /// <summary>1 клетка, 3 цвета → только первый получает клетку.</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void OneCell_ManyColors(IColorfulHousesSolution s)
    {
        Assert.Equal([1, 0, 0], s.Solve(1, 1, 3));
    }

    // ── Длина возвращаемого массива ───────────────────────────────────────

    /// <summary>Длина результата всегда равна colorsCount.</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ResultLength_EqualsColorsCount(IColorfulHousesSolution s)
    {
        Assert.Equal(5, s.Solve(3, 4, 5).Length);
        Assert.Equal(7, s.Solve(2, 2, 7).Length);
        Assert.Single(s.Solve(1, 1, 1));
    }

    // ── Инвариант суммы ───────────────────────────────────────────────────

    /// <summary>Сумма всех значений всегда равна height * width.</summary>
    [Theory]
    [MemberData(nameof(GetSumCases))]
    public void SumOfResult_EqualsHeightTimesWidth(
        IColorfulHousesSolution s, int h, int w, int c, int expectedSum)
    {
        Assert.Equal(expectedSum, s.Solve(h, w, c).Sum());
    }

    // ── Порядок (round-robin отдаёт «лишние» первым) ──────────────────────

    /// <summary>Результат не возрастает: каждый следующий ≤ предыдущему.</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Result_IsNonIncreasing(IColorfulHousesSolution s)
    {
        int[] result = s.Solve(7, 11, 6);
        for (int i = 1; i < result.Length; i++)
            Assert.True(result[i] <= result[i - 1],
                $"result[{i}]={result[i]} > result[{i - 1}]={result[i - 1]}");
    }

    /// <summary>Разница между любыми двумя цветами не превышает 1.</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Result_DiffBetweenAnyTwoColors_AtMostOne(IColorfulHousesSolution s)
    {
        int[] result = s.Solve(5, 7, 6); // 35 клеток, 6 цветов
        Assert.True(result.Max() - result.Min() <= 1);
    }

    // ── Граничные и крупные значения ──────────────────────────────────────

    /// <summary>100×100 клеток, 100 цветов → каждый ровно 100.</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void LargeSquare_EqualColors(IColorfulHousesSolution s)
    {
        int[] result = s.Solve(100, 100, 100);
        Assert.All(result, v => Assert.Equal(100, v));
    }

    /// <summary>Квадрат 10×10, 1 цвет → [100].</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void LargeSquare_OneColor(IColorfulHousesSolution s)
    {
        Assert.Equal([100], s.Solve(10, 10, 1));
    }

    /// <summary>Нечётное поле 3×7=21, 4 цвета → [6, 5, 5, 5].</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NonSquareOddField(IColorfulHousesSolution s)
    {
        Assert.Equal([6, 5, 5, 5], s.Solve(3, 7, 4));
    }
}
