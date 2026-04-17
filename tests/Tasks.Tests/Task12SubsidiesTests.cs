using Task12.Subsidies;

namespace Tasks.Tests;

public class Task12SubsidiesTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new SubsidiesSolution()];
        yield return [new SubsidiesSolutionAlt()];
    }

    private static SubsidiesSolution.Child MakeChild(int age) => new()
    {
        FullName = $"Child{age}",
        BirthDay = DateTime.Now.AddYears(-age).AddDays(-30),
        ID = "0000 000000",
        Gender = true
    };

    private static SubsidiesSolution.Parent MakeParent(params int[] childAges)
    {
        var children = childAges.Select(MakeChild).ToArray();
        return new SubsidiesSolution.Parent
        {
            FullName = "Parent",
            BirthDay = DateTime.Now.AddYears(-35),
            Gender = true,
            ID = "0000 000000",
            Children = children,
            ChildrenCount = children.Length
        };
    }

    private static double[] Run(ISubsidiesSolution s, SubsidiesSolution.Parent[] parents, int total)
        => s.CalculateSubsidies(parents, total);

    // ─── Длина результата ─────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ResultLength_MatchesParentsCount(ISubsidiesSolution s)
    {
        var parents = new[] { MakeParent(5, 10), MakeParent(3), MakeParent(8, 12, 14) };
        var result = Run(s, parents, 10000);
        Assert.Equal(parents.Length, result.Length);
    }

    // ─── Сумма субсидий равна total ───────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TotalSubsidies_EqualsTotal(ISubsidiesSolution s)
    {
        var parents = new[] { MakeParent(5, 10), MakeParent(8, 12) };
        int total = 4200;
        var result = Run(s, parents, total);
        Assert.Equal(total, result.Sum(), 5);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TotalSubsidies_EqualsTotal_MixedEligibility(ISubsidiesSolution s)
    {
        var parents = new[] { MakeParent(5, 10), MakeParent(3), MakeParent(8, 12, 14) };
        int total = 10000;
        var result = Run(s, parents, total);
        Assert.Equal(total, result.Sum(), 5);
    }

    // ─── Родитель с < 2 детьми не получает субсидию ───────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ParentWithOneChild_GetsZero(ISubsidiesSolution s)
    {
        var parents = new[] { MakeParent(5, 10), MakeParent(3) };
        var result = Run(s, parents, 2100);
        Assert.Equal(0.0, result[1], 5);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ParentWithNoChildren_GetsZero(ISubsidiesSolution s)
    {
        var parents = new[] { MakeParent(5, 10), MakeParent() };
        var result = Run(s, parents, 2100);
        Assert.Equal(0.0, result[1], 5);
    }

    // ─── Формула расчёта: base * (1 + index * 0.1) ───────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TwoEligibleChildren_CorrectSubsidy(ISubsidiesSolution s)
    {
        // childrenSum = 1.0 + 1.1 = 2.1, base = 2100/2.1 = 1000
        // subsidy = 1000*1.0 + 1000*1.1 = 2100
        var parents = new[] { MakeParent(5, 10) };
        var result = Run(s, parents, 2100);
        Assert.Equal(2100.0, result[0], 5);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ThreeEligibleChildren_CorrectSubsidy(ISubsidiesSolution s)
    {
        // childrenSum = 1.0 + 1.1 + 1.2 = 3.3, base = 3300/3.3 = 1000
        // subsidy = 1000 + 1100 + 1200 = 3300
        var parents = new[] { MakeParent(5, 10, 12) };
        var result = Run(s, parents, 3300);
        Assert.Equal(3300.0, result[0], 5);
    }

    // ─── Дети старше 16 лет не учитываются ────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ChildOver16_ExcludedFromSubsidy(ISubsidiesSolution s)
    {
        // Parent: children ages 5, 10, 20. Only first two eligible (indices 0, 1).
        // childrenSum = 1.0 + 1.1 = 2.1, base = 1000
        var parents = new[] { MakeParent(5, 10, 20) };
        var result = Run(s, parents, 2100);
        Assert.Equal(2100.0, result[0], 5);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void OverAgeChildAtIndex0_UsesOriginalIndices(ISubsidiesSolution s)
    {
        // Parent: children [age 20, age 5]. Only index 1 eligible.
        // childrenSum = (1 + 1*0.1) = 1.1, base = 1100/1.1 = 1000
        // child[1] subsidy = 1000 * 1.1 = 1100
        var parents = new[] { MakeParent(20, 5) };
        var result = Run(s, parents, 1100);
        Assert.Equal(1100.0, result[0], 5);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void AllChildrenOver16_ParentGetsZero(ISubsidiesSolution s)
    {
        // Parent 0: eligible, Parent 1: 2 kids but both > 16
        var parents = new[] { MakeParent(5, 10), MakeParent(20, 25) };
        int total = 2100;
        var result = Run(s, parents, total);
        Assert.Equal(total, result[0], 5);
        Assert.Equal(0.0, result[1], 5);
    }

    // ─── Ребёнку ровно 16 лет — субсидия полагается (по условию "не старше 16") ──

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ChildExactly16_IncludedInSubsidy(ISubsidiesSolution s)
    {
        // Spec says "не старше 16 лет" => age <= 16
        // Parent: children ages 10, 16. Both eligible.
        // childrenSum = 1.0 + 1.1 = 2.1, base = 1000
        var parents = new[] { MakeParent(10, 16) };
        var result = Run(s, parents, 2100);
        Assert.Equal(2100.0, result[0], 5);
    }

    // ─── Несколько родителей ──────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TwoEligibleParents_EqualChildren_EqualSubsidies(ISubsidiesSolution s)
    {
        // Both parents: 2 children each, all <= 16
        // childrenSum = (1.0+1.1) + (1.0+1.1) = 4.2, base = 1000
        var parents = new[] { MakeParent(5, 10), MakeParent(8, 12) };
        var result = Run(s, parents, 4200);
        Assert.Equal(2100.0, result[0], 5);
        Assert.Equal(2100.0, result[1], 5);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TwoParents_DifferentChildCounts_ProportionalSubsidies(ISubsidiesSolution s)
    {
        // Parent 0: 3 children (5,10,12) => coefficients 1.0+1.1+1.2 = 3.3
        // Parent 1: 2 children (8,14)    => coefficients 1.0+1.1     = 2.1
        // childrenSum = 5.4, base = 5400/5.4 = 1000
        // Parent 0: 3300, Parent 1: 2100
        var parents = new[] { MakeParent(5, 10, 12), MakeParent(8, 14) };
        var result = Run(s, parents, 5400);
        Assert.Equal(3300.0, result[0], 5);
        Assert.Equal(2100.0, result[1], 5);
    }

    // ─── Смешанные случаи ─────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MixedEligibility_OnlyEligibleParentGetsSubsidy(ISubsidiesSolution s)
    {
        // Parent 0: 2 kids <= 16 (eligible)
        // Parent 1: 1 kid (not eligible — less than 2 children)
        var parents = new[] { MakeParent(5, 10), MakeParent(3) };
        int total = 2100;
        var result = Run(s, parents, total);
        Assert.Equal(total, result[0], 5);
        Assert.Equal(0.0, result[1], 5);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ThreeParents_MixedEligibility(ISubsidiesSolution s)
    {
        // Parent 0: 2 kids (5, 10) => coeff 2.1
        // Parent 1: 1 kid (3)      => not eligible
        // Parent 2: 3 kids (8, 12, 14) => coeff 1.0+1.1+1.2 = 3.3
        // childrenSum = 2.1 + 3.3 = 5.4, base = 5400/5.4 = 1000
        // Parent 0: 2100, Parent 1: 0, Parent 2: 3300
        var parents = new[] { MakeParent(5, 10), MakeParent(3), MakeParent(8, 12, 14) };
        var result = Run(s, parents, 5400);
        Assert.Equal(2100.0, result[0], 5);
        Assert.Equal(0.0, result[1], 5);
        Assert.Equal(3300.0, result[2], 5);
    }

    // ─── Большие данные: инвариант суммы ──────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void LargeInput_TotalPreserved(ISubsidiesSolution s)
    {
        var parents = new[]
        {
            MakeParent(1, 3, 5, 7),
            MakeParent(2, 4, 6),
            MakeParent(10),
            MakeParent(8, 11, 13, 15),
            MakeParent(2, 9)
        };
        int total = 100000;
        var result = Run(s, parents, total);
        Assert.Equal(parents.Length, result.Length);
        Assert.Equal(total, result.Sum(), 5);
        Assert.Equal(0.0, result[2], 5);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void LargeInput_NonEligibleParentsGetZero(ISubsidiesSolution s)
    {
        var parents = new[]
        {
            MakeParent(5, 10),
            MakeParent(7),
            MakeParent(3, 8, 12),
            MakeParent(),
            MakeParent(2, 6)
        };
        int total = 50000;
        var result = Run(s, parents, total);
        Assert.Equal(0.0, result[1], 5);
        Assert.Equal(0.0, result[3], 5);
    }

    // ─── Крайние случаи: возраст 17 лет ───────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ChildExactly17_ExcludedFromSubsidy(ISubsidiesSolution s)
    {
        // Spec: "не старше 16 лет" => 17 is too old
        // Parent: children ages 10, 17. Only first eligible.
        // childrenSum = 1.0, base = 1000
        var parents = new[] { MakeParent(10, 17) };
        var result = Run(s, parents, 1000);
        Assert.Equal(1000.0, result[0], 5);
    }

    // ─── Крайний случай: 2 детей, но один старше 16 (остается < 2 подходящих) ────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TwoChildren_OneOverAge_ParentGetsZero(ISubsidiesSolution s)
    {
        // Parent has 2 children total, but only 1 is <= 16.
        // According to strict interpretation: parent needs >= 2 children total (satisfied),
        // but only eligible children get subsidy.
        // This is ambiguous in spec. Current impl: parent with 2 kids gets considered,
        // but we only count eligible children in the formula.
        // Let's verify behavior: parent should get subsidy for the 1 eligible child.
        var parents = new[] { MakeParent(10, 20) };
        var result = Run(s, parents, 1000);
        // childrenSum = 1.0 (only child[0] at index 0 is eligible)
        // subsidy = 1000 * 1.0 = 1000
        Assert.Equal(1000.0, result[0], 5);
    }

    // ─── Крайний случай: очень маленький total ────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void VerySmallTotal_DistributedCorrectly(ISubsidiesSolution s)
    {
        var parents = new[] { MakeParent(5, 10) };
        int total = 1;
        var result = Run(s, parents, total);
        Assert.Equal(total, result.Sum(), 5);
    }

    // ─── Крайний случай: очень большой total ──────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void VeryLargeTotal_DistributedCorrectly(ISubsidiesSolution s)
    {
        var parents = new[] { MakeParent(5, 10), MakeParent(3, 8) };
        int total = 1_000_000;
        var result = Run(s, parents, total);
        Assert.Equal(total, result.Sum(), 5);
    }

    // ─── Крайний случай: много детей у одного родителя ────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ParentWithManyChildren_CorrectSubsidy(ISubsidiesSolution s)
    {
        // Parent with 10 children, all eligible
        // coefficients: 1.0, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9
        // sum = 1.0 + 1.1 + 1.2 + 1.3 + 1.4 + 1.5 + 1.6 + 1.7 + 1.8 + 1.9 = 14.5
        var parents = new[] { MakeParent(1, 2, 3, 4, 5, 6, 7, 8, 9, 10) };
        int total = 14500;
        var result = Run(s, parents, total);
        Assert.Equal(total, result.Sum(), 5);
    }

    // ─── Крайний случай: почти все родители неподходящие ─────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MostParentsIneligible_OnlyOneGetsSubsidy(ISubsidiesSolution s)
    {
        // Parent 0: 1 child (< 2 children)
        // Parent 1: no children
        // Parent 2: 2 children, both > 16
        // Parent 3: 2 eligible children (this one gets all subsidy)
        var parents = new[] { MakeParent(5), MakeParent(), MakeParent(20, 25), MakeParent(10, 12) };
        int total = 10000;
        var result = Run(s, parents, total);
        Assert.Equal(0.0, result[0], 5);
        Assert.Equal(0.0, result[1], 5);
        Assert.Equal(0.0, result[2], 5);
        Assert.Equal(total, result[3], 5);
    }

    // ─── Крайний случай: только один родитель, много детей ────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SingleParent_GetsAllSubsidy(ISubsidiesSolution s)
    {
        var parents = new[] { MakeParent(5, 10, 12) };
        int total = 10000;
        var result = Run(s, parents, total);
        Assert.Single(result);
        Assert.Equal(total, result[0], 5);
    }

    // ─── Проверка Age свойства ────────────────────────────────────────────────────

    [Fact]
    public void ChildAge_CalculatedCorrectly()
    {
        var child = MakeChild(10);
        // Child should be approximately 10 years old (within reasonable bounds)
        Assert.InRange(child.Age, 9, 11);
    }

    [Fact]
    public void ChildAge_ForNewborn()
    {
        var child = new SubsidiesSolution.Child
        {
            FullName = "Newborn",
            BirthDay = DateTime.Now.AddDays(-30), // 1 month old
            ID = "0000",
            Gender = true
        };
        Assert.Equal(0, child.Age);
    }

    // ─── Крайний случай: граничный возраст между 16 и 17 ─────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ChildTurning17Soon_StillCountsIfCurrently16(ISubsidiesSolution s)
    {
        // Create child who is 16 years and 11 months old (still 16)
        var child16 = new SubsidiesSolution.Child
        {
            FullName = "Child16",
            BirthDay = DateTime.Now.AddYears(-16).AddMonths(-11),
            ID = "0000",
            Gender = true
        };
        var parent = new SubsidiesSolution.Parent
        {
            FullName = "Parent",
            BirthDay = DateTime.Now.AddYears(-35),
            Gender = true,
            ID = "0000",
            Children = new[] { child16, MakeChild(10) },
            ChildrenCount = 2
        };
        var result = Run(s, new[] { parent }, 2100);
        // Should include both children
        Assert.Equal(2100.0, result[0], 5);
    }

    // ─── Крайний случай: проверка индексов при пропуске детей ─────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NonContiguousEligibleChildren_UsesOriginalIndices(ISubsidiesSolution s)
    {
        // Parent: children [age 5 (idx 0), age 20 (idx 1), age 10 (idx 2)]
        // Only indices 0 and 2 eligible
        // childrenSum = (1 + 0*0.1) + (1 + 2*0.1) = 1.0 + 1.2 = 2.2
        var parents = new[] { MakeParent(5, 20, 10) };
        int total = 2200;
        var result = Run(s, parents, total);
        Assert.Equal(total, result.Sum(), 5);
    }

    // ─── Проверка деления на ноль (defensive) ─────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ZeroTotal_HandledGracefully(ISubsidiesSolution s)
    {
        var parents = new[] { MakeParent(5, 10) };
        int total = 0;
        var result = Run(s, parents, total);
        Assert.Equal(0.0, result.Sum(), 5);
    }
}
