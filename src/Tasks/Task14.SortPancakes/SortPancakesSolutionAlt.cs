using Tasks.Common;

namespace Task14.SortPancakes;

[Task(14, "Сортировка блинов", "alt")]
public class SortPancakesSolutionAlt : ISortPancakesSolution
{
    public void Run()
    {
        List<int> plate1 = [4, 1, 7, 3, 2, 4, 8, 5, 6];
        List<int> plate2 = [1, 2, 3, 4, 4, 5, 6, 7, 8];
        List<int> plate3 = [8, 7, 6, 5, 4, 4, 3, 2, 1];
        List<int> plate4 = [5, 1, 8];
        RunTest(plate1);
        RunTest(plate2);
        RunTest(plate3);
        RunTest(plate4);
    }

    private void RunTest(List<int> plate)
    {
        SortPancakes(plate);

        foreach (int size in plate)
        {
            Console.WriteLine(size);
        }

        Console.WriteLine();
    }

    public void SortPancakes(List<int> plate)
    {
        for (int i = 0; i < plate.Count - 1; i++)
        {
            int maxSizeIdx = FindMaxSizeIdx(plate, i);

            if (maxSizeIdx != i)
            {
                TossPancakes(plate, maxSizeIdx);
                TossPancakes(plate, i);
            }
        }
    }

    private int FindMaxSizeIdx(List<int> plate, int endIdx)
    {
        int maxSizeIdx = plate.Count - 1;
        for (int j = maxSizeIdx; j >= endIdx; j--)
        {
            // >=: ищу самый левый максимум, чтобы избежать перестановок с одинаковыми значениями
            if (plate[j] >= plate[maxSizeIdx]) maxSizeIdx = j;
        }
        return maxSizeIdx;
    }

    private void TossPancakes(List<int> plate, int pancakeIdx)
    {
        int left = pancakeIdx;
        int right = plate.Count - 1;

        while (left < right)
        {
            (plate[left], plate[right]) = (plate[right], plate[left]);
            left++;
            right--;
        }
    }
}
