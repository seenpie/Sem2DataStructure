using Tasks.Common;

namespace Task08.ColorfulHouses;

[Task(8, "Цветные дома", "alt")]
public class ColorfulHousesSolutionAlt : IColorfulHousesSolution
{
    public void Run()
    {
        foreach (int colorCount in Solve(int.Parse(Console.ReadLine()), int.Parse(Console.ReadLine()), int.Parse(Console.ReadLine())))
        {
            Console.WriteLine(colorCount);
        }
    }
    public int[] Solve(int height, int width, int colors)
    {
        int housesCount = height * width;
        int houseCount = housesCount / colors;
        int remHousesCount = housesCount % colors;

        int[] counts = new int[colors];
        for (int i = 0; i < colors; i++)
        {
            counts[i] = remHousesCount > 0 ? houseCount + 1 : houseCount;
            remHousesCount--;
        }

        return counts;
    }
}
