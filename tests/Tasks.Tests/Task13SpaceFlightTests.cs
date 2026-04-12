using Task13.SpaceFlight;

namespace Tasks.Tests;

public class Task13SpaceFlightTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new SpaceFlightSolution()];
    }

    private static double[] Run(ISpaceFlightSolution s, double[] applicants, int total)
        => s.SelectRichest(applicants, total);

    // ─── Базовые тесты ─────────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SelectTop3_FromSimpleArray_ReturnsCorrectValues(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 100, 500, 200, 300, 400 };
        var result = Run(s, applicants, 3);
        
        Assert.Equal(3, result.Length);
        Assert.Equal(500, result[0]);
        Assert.Equal(400, result[1]);
        Assert.Equal(300, result[2]);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SelectTop1_ReturnsMaximum(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 10.5, 20.3, 15.7, 8.2 };
        var result = Run(s, applicants, 1);
        
        Assert.Single(result);
        Assert.Equal(20.3, result[0]);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SelectAll_ReturnsSortedArray(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 10, 30, 20, 40 };
        var result = Run(s, applicants, 4);
        
        Assert.Equal(4, result.Length);
        Assert.Equal(40, result[0]);
        Assert.Equal(30, result[1]);
        Assert.Equal(20, result[2]);
        Assert.Equal(10, result[3]);
    }

    // ─── Проверка порядка убывания ─────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Result_IsInDescendingOrder(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 5, 2, 8, 1, 9, 3, 7, 4, 6 };
        var result = Run(s, applicants, 5);
        
        Assert.Equal(5, result.Length);
        for (int i = 0; i < result.Length - 1; i++)
        {
            Assert.True(result[i] >= result[i + 1], 
                $"Элементы не в порядке убывания: result[{i}]={result[i]}, result[{i+1}]={result[i+1]}");
        }
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void LargeSelection_MaintainsDescendingOrder(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 100, 50, 200, 25, 150, 75, 175, 125 };
        var result = Run(s, applicants, 6);
        
        Assert.Equal(6, result.Length);
        Assert.Equal(200, result[0]);
        Assert.Equal(175, result[1]);
        Assert.Equal(150, result[2]);
        Assert.Equal(125, result[3]);
        Assert.Equal(100, result[4]);
        Assert.Equal(75, result[5]);
    }

    // ─── Крайние случаи ────────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void EmptyArray_ReturnsEmpty(ISpaceFlightSolution s)
    {
        var applicants = Array.Empty<double>();
        var result = Run(s, applicants, 5);
        
        Assert.Empty(result);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NullArray_ReturnsEmpty(ISpaceFlightSolution s)
    {
        var result = Run(s, null!, 5);
        Assert.Empty(result);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ZeroTotal_ReturnsEmpty(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 100, 200, 300 };
        var result = Run(s, applicants, 0);
        
        Assert.Empty(result);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NegativeTotal_ReturnsEmpty(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 100, 200, 300 };
        var result = Run(s, applicants, -1);
        
        Assert.Empty(result);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TotalGreaterThanSize_ReturnsAllSorted(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 30, 10, 20 };
        var result = Run(s, applicants, 10);
        
        Assert.Equal(3, result.Length);
        Assert.Equal(30, result[0]);
        Assert.Equal(20, result[1]);
        Assert.Equal(10, result[2]);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SingleElement_SelectOne_ReturnsThatElement(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 42.5 };
        var result = Run(s, applicants, 1);
        
        Assert.Single(result);
        Assert.Equal(42.5, result[0]);
    }

    // ─── Дубликаты ─────────────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ArrayWithDuplicates_HandledCorrectly(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 100, 200, 100, 300, 200, 100 };
        var result = Run(s, applicants, 4);
        
        Assert.Equal(4, result.Length);
        Assert.Equal(300, result[0]);
        Assert.Equal(200, result[1]);
        Assert.Equal(200, result[2]);
        Assert.Equal(100, result[3]);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void AllElementsSame_ReturnsAllSame(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 100, 100, 100, 100, 100 };
        var result = Run(s, applicants, 3);
        
        Assert.Equal(3, result.Length);
        Assert.All(result, x => Assert.Equal(100, x));
    }

    // ─── Различные диапазоны значений ──────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void VerySmallValues_WorkCorrectly(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 0.001, 0.005, 0.003, 0.002, 0.004 };
        var result = Run(s, applicants, 3);
        
        Assert.Equal(3, result.Length);
        Assert.Equal(0.005, result[0], 10);
        Assert.Equal(0.004, result[1], 10);
        Assert.Equal(0.003, result[2], 10);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void VeryLargeValues_WorkCorrectly(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 1e15, 5e15, 3e15, 2e15, 4e15 };
        var result = Run(s, applicants, 3);
        
        Assert.Equal(3, result.Length);
        Assert.Equal(5e15, result[0]);
        Assert.Equal(4e15, result[1]);
        Assert.Equal(3e15, result[2]);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MixedIntegerAndDecimal_WorkCorrectly(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 100.5, 100, 100.2, 99.9, 101 };
        var result = Run(s, applicants, 3);
        
        Assert.Equal(3, result.Length);
        Assert.Equal(101, result[0]);
        Assert.Equal(100.5, result[1]);
        Assert.Equal(100.2, result[2]);
    }

    // ─── Большие данные ────────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void LargeArray_SelectSmallSubset(ISpaceFlightSolution s)
    {
        // Генерируем массив из 1000 элементов
        var random = new Random(42);
        var applicants = new double[1000];
        for (int i = 0; i < applicants.Length; i++)
        {
            applicants[i] = random.NextDouble() * 10000;
        }
        
        var result = Run(s, applicants, 10);
        
        Assert.Equal(10, result.Length);
        
        // Проверяем порядок убывания
        for (int i = 0; i < result.Length - 1; i++)
        {
            Assert.True(result[i] >= result[i + 1]);
        }
        
        // Проверяем, что выбраны действительно максимальные
        var sortedAll = applicants.OrderByDescending(x => x).ToArray();
        for (int i = 0; i < 10; i++)
        {
            Assert.Equal(sortedAll[i], result[i], 10);
        }
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SelectHalf_WorksCorrectly(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var result = Run(s, applicants, 5);
        
        Assert.Equal(5, result.Length);
        Assert.Equal(10, result[0]);
        Assert.Equal(9, result[1]);
        Assert.Equal(8, result[2]);
        Assert.Equal(7, result[3]);
        Assert.Equal(6, result[4]);
    }

    // ─── Проверка корректности отбора ──────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SelectedValues_AreFromOriginalArray(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 10.1, 20.2, 30.3, 40.4, 50.5 };
        var result = Run(s, applicants, 3);
        
        Assert.Equal(3, result.Length);
        Assert.All(result, x => Assert.Contains(x, applicants));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void UnsortedArray_ReturnsTopN(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 99, 1, 88, 2, 77, 3, 66, 4, 55, 5 };
        var result = Run(s, applicants, 5);
        
        Assert.Equal(5, result.Length);
        Assert.Equal(99, result[0]);
        Assert.Equal(88, result[1]);
        Assert.Equal(77, result[2]);
        Assert.Equal(66, result[3]);
        Assert.Equal(55, result[4]);
    }

    // ─── Граничные случаи по условию ──────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MaxSize1000_WorksCorrectly(ISpaceFlightSolution s)
    {
        var applicants = new double[1000];
        for (int i = 0; i < 1000; i++)
        {
            applicants[i] = 1000 - i; // От 1000 до 1
        }
        
        var result = Run(s, applicants, 100);
        
        Assert.Equal(100, result.Length);
        Assert.Equal(1000, result[0]);
        Assert.Equal(901, result[99]);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void AlmostSorted_WorksCorrectly(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 1, 2, 3, 4, 10, 6, 7, 8, 9, 5 };
        var result = Run(s, applicants, 5);
        
        Assert.Equal(5, result.Length);
        Assert.Equal(10, result[0]);
        Assert.Equal(9, result[1]);
        Assert.Equal(8, result[2]);
        Assert.Equal(7, result[3]);
        Assert.Equal(6, result[4]);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ReverseSorted_WorksCorrectly(ISpaceFlightSolution s)
    {
        var applicants = new double[] { 100, 90, 80, 70, 60, 50, 40, 30, 20, 10 };
        var result = Run(s, applicants, 4);
        
        Assert.Equal(4, result.Length);
        Assert.Equal(100, result[0]);
        Assert.Equal(90, result[1]);
        Assert.Equal(80, result[2]);
        Assert.Equal(70, result[3]);
    }
}
