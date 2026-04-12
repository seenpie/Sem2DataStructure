using System;

using Tasks.Common;

namespace Task13.SpaceFlight;

[Task(13, "Космический полет")]
public class SpaceFlightSolution : ISpaceFlightSolution
{
    public void Run()
    {
        int total = int.Parse(Console.ReadLine());
        int size = int.Parse(Console.ReadLine());
        double[] applicants = new double[size];

        for (int i = 0; i < size; i++)
        {
            applicants[i] = double.Parse(Console.ReadLine());
        }

        double[] richest = SelectRichest(applicants, total);

        foreach (var wealth in richest)
        {
            Console.WriteLine(wealth);
        }
    }

    public double[] SelectRichest(double[] applicants, int total)
    {
        if (applicants == null || applicants.Length == 0 || total <= 0)
            return Array.Empty<double>();

        if (total >= applicants.Length)
        {
            // Если нужно отобрать всех или больше, просто сортируем весь массив
            var result = new double[applicants.Length];
            Array.Copy(applicants, result, applicants.Length);
            Array.Sort(result);
            Array.Reverse(result);
            return result;
        }

        // Массив для хранения самых богатых претендентов
        double[] richest = new double[total];
        
        // Заполняем первые total элементов
        Array.Copy(applicants, richest, total);

        // Обрабатываем оставшихся претендентов
        for (int i = total; i < applicants.Length; i++)
        {
            // Находим индекс минимального элемента среди отобранных
            int minIndex = 0;
            double minValue = richest[0];
            
            for (int j = 1; j < richest.Length; j++)
            {
                if (richest[j] < minValue)
                {
                    minValue = richest[j];
                    minIndex = j;
                }
            }

            // Если текущий претендент богаче минимума - заменяем
            if (applicants[i] > minValue)
            {
                richest[minIndex] = applicants[i];
            }
        }

        // Сортируем результат в порядке убывания
        Array.Sort(richest);
        Array.Reverse(richest);

        return richest;
    }
}
