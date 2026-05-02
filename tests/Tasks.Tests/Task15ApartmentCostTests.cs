using Task15.ApartmentCost;
using Apt = Task15.ApartmentCost.ApartmentCostSolution.SApartment;

namespace Tasks.Tests;

public class Task15ApartmentCostTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new ApartmentCostSolution()];
        yield return [new ApartmentCostSolutionAlt()];
    }

    private static Apt MakeApt(double[] roomAreas, double kitchen, double other, double price) => new()
    {
        RoomAreas = roomAreas,
        KitchenArea = kitchen,
        OtherArea = other,
        Price = price
    };

    private static double Run(IApartmentCostSolution s, List<Apt> apts, int rooms, double totalSquare)
        => s.CalculateMedianPrice(apts, rooms, totalSquare);

    // ─── Медиана: нечётное количество ─────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void OddCount_ReturnsMiddleElement(IApartmentCostSolution s)
    {
        // 3 двухкомнатные квартиры с площадью > 40
        var apts = new List<Apt>
        {
            MakeApt(new[] { 18.0, 14.0 }, 10, 8, 5_000_000),  // площадь 50
            MakeApt(new[] { 20.0, 16.0 }, 12, 10, 7_000_000), // площадь 58
            MakeApt(new[] { 16.0, 15.0 }, 10, 6, 6_000_000),  // площадь 47
        };
        // Цены отсортированные: 5M, 6M, 7M → медиана = 6M
        Assert.Equal(6_000_000, Run(s, apts, 2, 40), 5);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SingleMatch_ReturnsThatPrice(IApartmentCostSolution s)
    {
        var apts = new List<Apt>
        {
            MakeApt(new[] { 20.0, 15.0 }, 10, 5, 4_500_000), // площадь 50, 2 комнаты
            MakeApt(new[] { 10.0 }, 8, 5, 2_000_000),         // 1 комната — не подходит
        };
        Assert.Equal(4_500_000, Run(s, apts, 2, 40), 5);
    }

    // ─── Медиана: чётное количество ───────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void EvenCount_ReturnsAverageOfTwoMiddle(IApartmentCostSolution s)
    {
        var apts = new List<Apt>
        {
            MakeApt(new[] { 18.0, 14.0 }, 10, 8, 5_000_000),
            MakeApt(new[] { 20.0, 16.0 }, 12, 10, 7_000_000),
            MakeApt(new[] { 16.0, 15.0 }, 10, 6, 6_000_000),
            MakeApt(new[] { 22.0, 18.0 }, 12, 8, 8_000_000),
        };
        // Цены: 5M, 6M, 7M, 8M → медиана = (6M + 7M) / 2 = 6.5M
        Assert.Equal(6_500_000, Run(s, apts, 2, 40), 5);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TwoMatches_ReturnsAverage(IApartmentCostSolution s)
    {
        var apts = new List<Apt>
        {
            MakeApt(new[] { 20.0, 15.0 }, 10, 6, 3_000_000), // площадь 51
            MakeApt(new[] { 25.0, 20.0 }, 12, 8, 5_000_000), // площадь 65
        };
        Assert.Equal(4_000_000, Run(s, apts, 2, 40), 5);
    }

    // ─── Фильтрация по количеству комнат ──────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FilterByRooms_OnlyMatchingRoomCount(IApartmentCostSolution s)
    {
        var apts = new List<Apt>
        {
            MakeApt(new[] { 30.0 }, 10, 10, 3_000_000),               // 1 комната
            MakeApt(new[] { 20.0, 15.0 }, 10, 6, 5_000_000),         // 2 комнаты ✓
            MakeApt(new[] { 18.0, 14.0, 12.0 }, 10, 6, 7_000_000),   // 3 комнаты
            MakeApt(new[] { 22.0, 16.0 }, 12, 8, 6_000_000),         // 2 комнаты ✓
        };
        // Только 2-комнатные: 5M, 6M → медиана = 5.5M
        Assert.Equal(5_500_000, Run(s, apts, 2, 40), 5);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FilterByRooms_ThreeRooms(IApartmentCostSolution s)
    {
        var apts = new List<Apt>
        {
            MakeApt(new[] { 20.0, 15.0 }, 10, 6, 5_000_000),
            MakeApt(new[] { 18.0, 14.0, 12.0 }, 10, 6, 7_000_000),  // 3 комнаты ✓
            MakeApt(new[] { 20.0, 16.0, 14.0 }, 12, 8, 9_000_000),  // 3 комнаты ✓
        };
        Assert.Equal(8_000_000, Run(s, apts, 3, 40), 5);
    }

    // ─── Фильтрация по площади ────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FilterByArea_OnlyAboveThreshold(IApartmentCostSolution s)
    {
        var apts = new List<Apt>
        {
            MakeApt(new[] { 10.0, 8.0 }, 6, 4, 2_000_000),   // площадь 28 — не подходит
            MakeApt(new[] { 20.0, 15.0 }, 10, 6, 5_000_000),  // площадь 51 ✓
            MakeApt(new[] { 15.0, 10.0 }, 8, 5, 3_000_000),   // площадь 38 — не подходит
            MakeApt(new[] { 25.0, 20.0 }, 12, 8, 7_000_000),  // площадь 65 ✓
        };
        Assert.Equal(6_000_000, Run(s, apts, 2, 40), 5);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FilterByArea_ExactlyEqualToThreshold_Excluded(IApartmentCostSolution s)
    {
        // Условие: площадь > totalSquare (строго больше)
        var apts = new List<Apt>
        {
            MakeApt(new[] { 20.0, 10.0 }, 8, 2, 4_000_000),  // площадь = 40 — НЕ подходит
            MakeApt(new[] { 20.0, 11.0 }, 8, 2, 5_000_000),  // площадь = 41 ✓
        };
        Assert.Equal(5_000_000, Run(s, apts, 2, 40), 5);
    }

    // ─── Нет подходящих квартир ───────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NoMatches_ReturnsMinusOne(IApartmentCostSolution s)
    {
        var apts = new List<Apt>
        {
            MakeApt(new[] { 10.0, 8.0 }, 6, 4, 2_000_000),  // площадь 28 < 40
            MakeApt(new[] { 12.0, 9.0 }, 7, 5, 3_000_000),  // площадь 33 < 40
        };
        Assert.Equal(-1, Run(s, apts, 2, 40), 5);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NoMatchingRooms_ReturnsMinusOne(IApartmentCostSolution s)
    {
        var apts = new List<Apt>
        {
            MakeApt(new[] { 30.0 }, 10, 5, 3_000_000),              // 1 комната
            MakeApt(new[] { 20.0, 15.0, 10.0 }, 10, 5, 7_000_000), // 3 комнаты
        };
        Assert.Equal(-1, Run(s, apts, 2, 40), 5);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void EmptyList_ReturnsMinusOne(IApartmentCostSolution s)
    {
        Assert.Equal(-1, Run(s, new List<Apt>(), 2, 40), 5);
    }

    // ─── Проверка вычисления TotalArea ────────────────────────────────────────────

    [Fact]
    public void TotalArea_SumsAllAreas()
    {
        var apt = MakeApt(new[] { 18.0, 14.0, 12.0 }, 10.0, 6.0, 0);
        // 18 + 14 + 12 + 10 + 6 = 60
        Assert.Equal(60.0, apt.TotalArea, 5);
    }

    [Fact]
    public void TotalArea_SingleRoom()
    {
        var apt = MakeApt(new[] { 25.0 }, 8.0, 5.0, 0);
        Assert.Equal(38.0, apt.TotalArea, 5);
    }

    [Fact]
    public void RoomCount_MatchesArrayLength()
    {
        Assert.Equal(1, MakeApt(new[] { 10.0 }, 5, 3, 0).RoomCount);
        Assert.Equal(2, MakeApt(new[] { 10.0, 12.0 }, 5, 3, 0).RoomCount);
        Assert.Equal(4, MakeApt(new[] { 10.0, 12.0, 8.0, 6.0 }, 5, 3, 0).RoomCount);
    }

    // ─── Порядок цен не важен ─────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void UnsortedPrices_MedianStillCorrect(IApartmentCostSolution s)
    {
        // Цены идут не по порядку
        var apts = new List<Apt>
        {
            MakeApt(new[] { 20.0, 15.0 }, 10, 6, 9_000_000),
            MakeApt(new[] { 22.0, 16.0 }, 12, 8, 3_000_000),
            MakeApt(new[] { 18.0, 14.0 }, 10, 6, 7_000_000),
            MakeApt(new[] { 24.0, 18.0 }, 14, 8, 5_000_000),
            MakeApt(new[] { 19.0, 13.0 }, 9, 7, 1_000_000),
        };
        // Отсортированные цены: 1M, 3M, 5M, 7M, 9M → медиана = 5M
        Assert.Equal(5_000_000, Run(s, apts, 2, 40), 5);
    }

    // ─── Одинаковые цены ──────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void AllSamePrice_MedianEqualsThatPrice(IApartmentCostSolution s)
    {
        var apts = new List<Apt>
        {
            MakeApt(new[] { 20.0, 15.0 }, 10, 6, 5_000_000),
            MakeApt(new[] { 22.0, 16.0 }, 12, 8, 5_000_000),
            MakeApt(new[] { 18.0, 14.0 }, 10, 6, 5_000_000),
        };
        Assert.Equal(5_000_000, Run(s, apts, 2, 40), 5);
    }

    // ─── Большой набор данных ─────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void LargeDataset_MedianCorrect(IApartmentCostSolution s)
    {
        var apts = new List<Apt>();
        for (int i = 1; i <= 100; i++)
        {
            apts.Add(MakeApt(new[] { 20.0, 15.0 }, 10, 6, i * 100_000));
        }
        // Цены: 100K, 200K, ..., 10_000K
        // Медиана чётного набора: (50-й + 51-й) / 2 = (5_000_000 + 5_100_000) / 2
        Assert.Equal(5_050_000, Run(s, apts, 2, 40), 5);
    }

    // ─── Смешанная фильтрация ─────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MixedFiltering_OnlyMatchingBothConditions(IApartmentCostSolution s)
    {
        var apts = new List<Apt>
        {
            MakeApt(new[] { 30.0 }, 10, 5, 2_000_000),                // 1 комната — нет
            MakeApt(new[] { 10.0, 8.0 }, 5, 3, 1_500_000),           // площадь 26 < 30 — нет
            MakeApt(new[] { 20.0, 15.0 }, 10, 6, 5_000_000),         // 2 комн, площадь 51 ✓
            MakeApt(new[] { 18.0, 14.0, 10.0 }, 10, 6, 7_000_000),   // 3 комнаты — нет
            MakeApt(new[] { 22.0, 16.0 }, 12, 8, 6_000_000),         // 2 комн, площадь 58 ✓
            MakeApt(new[] { 8.0, 7.0 }, 5, 3, 1_000_000),            // площадь 23 < 30 — нет
            MakeApt(new[] { 25.0, 20.0 }, 14, 10, 8_000_000),        // 2 комн, площадь 69 ✓
        };
        // Подходящие: 5M, 6M, 8M → медиана = 6M
        Assert.Equal(6_000_000, Run(s, apts, 2, 30), 5);
    }

    // ─── Дробные цены ─────────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FractionalPrices_MedianCorrect(IApartmentCostSolution s)
    {
        var apts = new List<Apt>
        {
            MakeApt(new[] { 20.0, 15.0 }, 10, 6, 3_500_000.50),
            MakeApt(new[] { 22.0, 16.0 }, 12, 8, 4_200_000.75),
        };
        // (3_500_000.50 + 4_200_000.75) / 2 = 3_850_000.625
        Assert.Equal(3_850_000.625, Run(s, apts, 2, 40), 5);
    }
}
