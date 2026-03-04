using Task07.CalculateOddIndicesAverage;

namespace Tasks.Tests;

public class Task07CalculateOddIndicesAverageTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new CalculateOddIndicesAverageSolution()];
    }

    private static double Calculate(ICalculateOddIndicesAverageSolution s, double[,] arr)
        => s.CalculateOddIndicesAverage(arr);

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MyTest1(ICalculateOddIndicesAverageSolution s)
    {
        double[,] test1 = {{1, 2, 3, 4}, {1, 2, 3, 4}};

        double result = Calculate(s, test1);
        Assert.Equal(3, result);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MyTest2(ICalculateOddIndicesAverageSolution s)
    {
        double[,] test2 = new double[1, 5];
        double[,] test3 = new double[5, 1];
        double[,] test4 = new double[0, 0];

        double[][,] tests = [test2, test3, test4];

        foreach (var test in tests)
        {
            double result = Calculate(s, test);
            Assert.Equal(0, result);
        }
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MyTest3(ICalculateOddIndicesAverageSolution s)
    {
        double[,] test5 =
        {
            {10, 20, 30, 40, 50},
            { 1,  2,  3,  4,  5},
            {10, 20, 30, 40, 50},
            { 5, 10, 15, 20, 25}
        };
        double result = Calculate(s, test5);
        Assert.Equal(9, result);
    }
}
