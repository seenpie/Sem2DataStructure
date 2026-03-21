using Task11.SegmentsSort;
using S = Task11.SegmentsSort.SegmentsSortSolution.Segment;
using P = Task11.SegmentsSort.SegmentsSortSolution.Segment.Point;

namespace Tasks.Tests;

public class Task11SegmentsSortTests
{
    private static S Seg(double x1, double y1, double x2, double y2)
        => new(new P(x1, y1), new P(x2, y2), 1.0, ConsoleColor.White);

    private static S[] Sort(S[] arr)
    {
        SegmentsSortSolution.QuickSegmentsSort(arr, 0, arr.Length - 1);
        return arr;
    }

    // ─── Базовые случаи ───────────────────────────────────────────────────────

    [Fact]
    public void TwoSegments_ShorterFirst()
    {
        S[] result = Sort([Seg(0, 0, 4, 3), Seg(0, 0, 3, 0)]); // длины: 5, 3

        Assert.True(result[0].GetQuadraticLength() < result[1].GetQuadraticLength());
    }

    [Fact]
    public void ThreeSegments_SortedAscending()
    {
        S[] result = Sort([Seg(0, 0, 10, 0), Seg(0, 0, 3, 0), Seg(0, 0, 5, 0)]);

        Assert.Equal(9.0,   result[0].GetQuadraticLength(), precision: 10);  // 3²
        Assert.Equal(25.0,  result[1].GetQuadraticLength(), precision: 10);  // 5²
        Assert.Equal(100.0, result[2].GetQuadraticLength(), precision: 10);  // 10²
    }

    [Fact]
    public void AlreadySorted_NoChange()
    {
        S[] result = Sort([Seg(0, 0, 1, 0), Seg(0, 0, 2, 0), Seg(0, 0, 3, 0)]);

        Assert.Equal(1.0, result[0].GetQuadraticLength(), precision: 10);  // 1²
        Assert.Equal(4.0, result[1].GetQuadraticLength(), precision: 10);  // 2²
        Assert.Equal(9.0, result[2].GetQuadraticLength(), precision: 10);  // 3²
    }

    [Fact]
    public void ReverseOrder_FullyReversed()
    {
        S[] result = Sort([Seg(0, 0, 5, 0), Seg(0, 0, 3, 0), Seg(0, 0, 1, 0)]);

        Assert.Equal(1.0,  result[0].GetQuadraticLength(), precision: 10);  // 1²
        Assert.Equal(9.0,  result[1].GetQuadraticLength(), precision: 10);  // 3²
        Assert.Equal(25.0, result[2].GetQuadraticLength(), precision: 10);  // 5²
    }

    // ─── Диагональные отрезки (тройки Пифагора) ──────────────────────────────

    [Fact]
    public void DiagonalSegments_CorrectLengthComputed()
    {
        // 3-4-5 → квадрат 25;  5-12-13 → квадрат 169
        S[] result = Sort([Seg(0, 0, 5, 12), Seg(0, 0, 3, 4)]);

        Assert.Equal(25.0,  result[0].GetQuadraticLength(), precision: 10);  // 3²+4²
        Assert.Equal(169.0, result[1].GetQuadraticLength(), precision: 10);  // 5²+12²
    }

    // ─── Граничные случаи ─────────────────────────────────────────────────────

    [Fact]
    public void SingleSegment_Unchanged()
    {
        S only = Assert.Single(Sort([Seg(1, 2, 4, 6)]));

        Assert.Equal(25.0, only.GetQuadraticLength(), precision: 10);  // 3²+4²
    }

    [Fact]
    public void ZeroLengthSegment_ComesFirst()
    {
        S[] result = Sort([Seg(0, 0, 1, 0), Seg(3, 3, 3, 3)]); // длины: 1, 0

        Assert.Equal(0.0, result[0].GetQuadraticLength(), precision: 10);
        Assert.Equal(1.0, result[1].GetQuadraticLength(), precision: 10);
    }

    [Fact]
    public void EqualLengthSegments_AllRemain()
    {
        S[] result = Sort([Seg(0, 0, 3, 4), Seg(0, 0, 4, 3), Seg(0, 0, 5, 0)]); // все длиной 5

        Assert.All(result, seg => Assert.Equal(25.0, seg.GetQuadraticLength(), precision: 10));  // 3²+4²=5²+0²=...
    }

    // ─── Поля структуры сохраняются ───────────────────────────────────────────

    [Fact]
    public void FieldsPreserved_AfterSort()
    {
        S red  = new(new P(0, 0), new P(10, 0), width: 3.0, ConsoleColor.Red);  // длина 10
        S blue = new(new P(0, 0), new P(1,  0), width: 0.5, ConsoleColor.Blue); // длина 1
        S[] result = Sort([red, blue]);

        Assert.Equal(ConsoleColor.Blue, result[0].ColorLine);
        Assert.Equal(0.5, result[0].WidthLine, precision: 10);
        Assert.Equal(ConsoleColor.Red, result[1].ColorLine);
        Assert.Equal(3.0, result[1].WidthLine, precision: 10);
    }

    // ─── Инварианты ───────────────────────────────────────────────────────────

    [Fact]
    public void Result_SortedAscendingByLength()
    {
        S[] result = Sort([Seg(0,0,7,0), Seg(0,0,2,0), Seg(0,0,5,0), Seg(0,0,1,0), Seg(0,0,9,0)]);

        for (int i = 1; i < result.Length; i++)
            Assert.True(result[i].GetQuadraticLength() >= result[i - 1].GetQuadraticLength());
    }

    [Fact]
    public void Result_ContainsSameElements()
    {
        S[] input = [Seg(0,0,3,0), Seg(0,0,1,0), Seg(0,0,7,0)];
        var before = input.Select(x => x.GetQuadraticLength()).OrderBy(x => x).ToList();

        S[] result = Sort(input);

        Assert.Equal(before, result.Select(x => x.GetQuadraticLength()).OrderBy(x => x).ToList());
    }

    [Fact]
    public void Result_CountUnchanged()
    {
        S[] input = [Seg(0,0,1,0), Seg(0,0,2,0), Seg(0,0,3,0), Seg(0,0,4,0)];
        Assert.Equal(input.Length, Sort(input).Length);
    }
}
