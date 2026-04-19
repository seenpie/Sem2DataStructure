using Tasks.Common;
using System.Collections.Generic;

namespace Task14.SortPancakes;

[Task(14, "Сортировка блинов")]
public class SortPancakesSolution : ISortPancakesSolution
{
    public void Run()
    {
        
        List<int> pancakes = new List<int> { 4, 1, 7, 3, 2, 4, 8, 5, 6 };
        SortPancakes(pancakes);
        Console.WriteLine(string.Join(" ", pancakes));

        pancakes = new List<int> { 1, 2, 3, 4, 4, 5, 6, 7, 8, 10, 40 };
        SortPancakes(pancakes);
        Console.WriteLine(string.Join(" ", pancakes));

        pancakes = new List<int> { 8, 7, 6, 5, 4, 4, 3, 2, 1 };
        SortPancakes(pancakes);
        Console.WriteLine(string.Join(" ", pancakes));
    }

    public void SortPancakes(List<int> pancakes)
    {
        for (int i = 0; i < pancakes.Count - 1; i++)
        {
            int maxIndex = FindIndexOfMaxPancake(pancakes, i);

            if (maxIndex == i)
                continue;

            Flip(pancakes, maxIndex);
            Flip(pancakes, i);
        }
    }

    private void Flip(List<int> pancakes, int fromIndex)
    {
        int count = pancakes.Count - fromIndex;
        pancakes.Reverse(fromIndex, count);
    }

    private int FindIndexOfMaxPancake(List<int> pancakes, int fromIndex)
    {
        int maxIndex = fromIndex;
        for (int i = fromIndex + 1; i < pancakes.Count; i++)
        {
            if (pancakes[i] > pancakes[maxIndex])
            {
                maxIndex = i;
            }
        }
        return maxIndex;
    }
}
