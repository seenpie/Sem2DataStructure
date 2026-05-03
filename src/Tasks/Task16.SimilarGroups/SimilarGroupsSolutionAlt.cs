using Tasks.Common;

namespace Task16.SimilarGroups;

[Task(16, "Группы похожих", "alt")]

public class SimilarGroupsSolutionAlt : ISimilarGroupsSolution
{
    public void Run()
    {
        SHuman[] group = [
            new SHuman("Пушкин", "Александр", "Сергеевич", 1799),
            new SHuman("Ломоносов", "Михаил", "Васильевич", 1711),
            new SHuman("Тютчев", "Фёдор", "Иванович", 1803),
            new SHuman("Суворов", "Александр", "Васильевич", 1729),
            new SHuman("Менделеев", "Дмитрий", "Иванович", 1834),
            new SHuman("Ахматова", "Анна", "Андреевна", 1889),
            new SHuman("Володин", "Александр", "Моисеевич", 1919),
            new SHuman("Мухина", "Вера", "Игнатьевна", 1889),
            new SHuman("Верещагин", "Петр", "Петрович", 1834),
        ];

        List<List<SHuman>> cluster = ClusterByField(group);

        for (int i = 0; i < cluster.Count; i++)
        {
            Console.WriteLine($"Группа {i + 1}");
            foreach (SHuman h in cluster[i])
            {
                Console.WriteLine($"{h.Firstname} {h.Surname} {h.Patronymic} {h.Year}");
            }
            Console.WriteLine();
        }
    }

    //Объединяет людей в группы по совпадению хотя бы одного поля (Используется Union-Find алгоритм с локальной реализацией)
    public List<List<SHuman>> ClusterByField(SHuman[] group)
    {
        List<List<int>> indexGroups = BuildIndexGroups(group);
        //Массив, где по индексу элемента указан его родитель (с кем он связан)
        int[] parents = new int[group.Length];
        for (int i = 0; i < group.Length; i++) parents[i] = i;

        //Ищет корень элемента (со сжатием путей)
        int Find(int idx)
        {
            if (idx != parents[idx])
                parents[idx] = Find(parents[idx]);

            return parents[idx];
        }

        //Объединяет два элемента, привязывая корень первого ко второму
        void Union(int idx1, int idx2)
        {
            int parent1 = Find(idx1);
            int parent2 = Find(idx2);
            if (parent1 != parent2)
                parents[parent1] = parent2;
        }

        foreach (List<int> value in indexGroups)
        {
            for (int i = 0; i < value.Count - 1; i++)
                Union(value[i], value[i + 1]);
        }

        Dictionary<int, List<SHuman>> clusters = new Dictionary<int, List<SHuman>>();
        for (int i = 0; i < group.Length; i++)
        {
            int root = Find(i);
            if (!clusters.ContainsKey(root))
                clusters[root] = new List<SHuman>();

            clusters[root].Add(group[i]);
        }

        return [.. clusters.Values];
    }

    //Создает инвертированный индекс, для каждого значения поля : список индексов людей
    private List<List<int>> BuildIndexGroups(SHuman[] group)
    {
        Dictionary<string, List<int>> surnames = new Dictionary<string, List<int>>();
        Dictionary<string, List<int>> firstnames = new Dictionary<string, List<int>>();
        Dictionary<string, List<int>> patronymics = new Dictionary<string, List<int>>();
        Dictionary<int, List<int>> years = new Dictionary<int, List<int>>();

        for (int i = 0; i < group.Length; i++)
        {
            SHuman human = group[i];
            if (!surnames.ContainsKey(human.Surname))
                surnames[human.Surname] = new List<int>();
            surnames[human.Surname].Add(i);

            if (!firstnames.ContainsKey(human.Firstname))
                firstnames[human.Firstname] = new List<int>();
            firstnames[human.Firstname].Add(i);

            if (!patronymics.ContainsKey(human.Patronymic))
                patronymics[human.Patronymic] = new List<int>();
            patronymics[human.Patronymic].Add(i);

            if (!years.ContainsKey(human.Year))
                years[human.Year] = new List<int>();
            years[human.Year].Add(i);
        }

        return
        [
            .. surnames.Values,
            .. firstnames.Values,
            .. patronymics.Values,
            .. years.Values,
        ];
    }

    public List<List<SimilarGroups.SHuman>> FindSimilarGroups(SimilarGroups.SHuman[] group)
    {
        return ClusterByField(group);
    }
}
