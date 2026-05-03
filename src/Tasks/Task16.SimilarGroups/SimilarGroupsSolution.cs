using Tasks.Common;

namespace Task16.SimilarGroups;

public struct SHuman
{
    public string Surname;          // фамилия
    public string Firstname;        // имя
    public string Patronymic;       // отчество
    public int Year;                // год рождения

    public SHuman(string surname, string firstname, string patronymic, int year)
    {
        this.Surname = surname;
        this.Firstname = firstname;
        this.Patronymic = patronymic;
        this.Year = year;
    }
}

[Task(16, "Группы похожих")]
public class SimilarGroupsSolution : ISimilarGroupsSolution
{
    public void Run()
    {
        SHuman[] group = {
            new SHuman("Пушкин", "Александр", "Сергеевич", 1799),
            new SHuman("Ломоносов", "Михаил", "Васильевич", 1711),
            new SHuman("Тютчев", "Фёдор", "Иванович", 1803),
            new SHuman("Суворов", "Александр", "Васильевич", 1729),
            new SHuman("Менделеев", "Дмитрий", "Иванович", 1834),
            new SHuman("Ахматова", "Анна", "Андреевна", 1889),
            new SHuman("Володин", "Александр", "Моисеевич", 1919),
            new SHuman("Мухина", "Вера", "Игнатьевна", 1889),
            new SHuman("Верещагин", "Петр", "Петрович", 1834),
        };

        List<List<SHuman>> groups = FindSimilarGroups(group);

        Console.WriteLine($"Всего людей: {group.Length}");
        Console.WriteLine($"Найдено групп: {groups.Count}");
        for (int i = 0; i < groups.Count; i++)
        {
            Console.WriteLine($"\nГруппа {i + 1} ({groups[i].Count} чел.):");
            foreach (SHuman h in groups[i])
            {
                Console.WriteLine($"  {h.Surname} {h.Firstname} {h.Patronymic}, {h.Year}");
            }
        }
    }

    // Время O(N²): для k-го человека обходим до k-1 уже добавленных
    // (распределённых по группам), сумма 1+2+...+(N-1) = N(N-1)/2.
    // Память O(N): каждый человек хранится ровно в одной группе.
    public List<List<SHuman>> FindSimilarGroups(SHuman[] group)
    {
        List<List<SHuman>> groups = new List<List<SHuman>>();
        foreach (SHuman human in group)
        {
            int firstMatchIndex = -1;
            int i = 0;
            while (i < groups.Count)
            {
                bool hasSimilar = false;
                foreach (SHuman otherHuman in groups[i])
                {
                    if (IsSimilar(human, otherHuman))
                    {
                        hasSimilar = true;
                        break;
                    }
                }

                if (hasSimilar)
                {
                    if (firstMatchIndex == -1)
                    {
                        firstMatchIndex = i;
                        groups[i].Add(human);
                        i++;
                    }
                    else
                    {
                        groups[firstMatchIndex].AddRange(groups[i]);
                        groups[i] = groups[groups.Count - 1];
                        groups.RemoveAt(groups.Count - 1);
                    }
                }
                else
                {
                    i++;
                }
            }

            if (firstMatchIndex == -1)
            {
                groups.Add(new List<SHuman> { human });
            }
        }

        return groups;
    }

    private bool IsSimilar(SHuman human1, SHuman human2)
    {
        return human1.Surname == human2.Surname ||
               human1.Firstname == human2.Firstname ||
               human1.Patronymic == human2.Patronymic ||
               human1.Year == human2.Year;
    }
}
