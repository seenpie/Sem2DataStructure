using Task16.SimilarGroups;

namespace Tasks.Tests;

public class Task16SimilarGroupsTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new SimilarGroupsSolution()];
        yield return [new SimilarGroupsSolutionAlt()];
    }

    // ---------- Граничные случаи ----------

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void EmptyArray_ReturnsEmptyList(ISimilarGroupsSolution s)
    {
        SHuman[] input = Array.Empty<SHuman>();

        List<List<SHuman>> result = s.FindSimilarGroups(input);

        Assert.Empty(result);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SinglePerson_ReturnsOneGroupOfOne(ISimilarGroupsSolution s)
    {
        SHuman[] input = { new SHuman("Иванов", "Иван", "Иванович", 1990) };

        List<List<SHuman>> result = s.FindSimilarGroups(input);

        Assert.Single(result);
        Assert.Single(result[0]);
        Assert.Equal("Иванов", result[0][0].Surname);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TwoCompletelyDifferent_ReturnsTwoGroups(ISimilarGroupsSolution s)
    {
        SHuman[] input = {
            new SHuman("Пушкин", "Александр", "Сергеевич", 1799),
            new SHuman("Менделеев", "Дмитрий", "Иванович", 1834),
        };

        List<List<int>> expected = new List<List<int>> {
            new List<int> { 0 },
            new List<int> { 1 },
        };

        AssertPartition(input, expected, s.FindSimilarGroups(input));
    }

    // ---------- Совпадение по каждому полю отдельно ----------

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MatchBySurname_OneGroup(ISimilarGroupsSolution s)
    {
        SHuman[] input = {
            new SHuman("Иванов", "Алексей", "Петрович", 1980),
            new SHuman("Иванов", "Борис", "Сергеевич", 1990),
        };

        List<List<int>> expected = new List<List<int>> { new List<int> { 0, 1 } };

        AssertPartition(input, expected, s.FindSimilarGroups(input));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MatchByFirstname_OneGroup(ISimilarGroupsSolution s)
    {
        SHuman[] input = {
            new SHuman("Иванов", "Александр", "Петрович", 1980),
            new SHuman("Петров", "Александр", "Сергеевич", 1990),
        };

        List<List<int>> expected = new List<List<int>> { new List<int> { 0, 1 } };

        AssertPartition(input, expected, s.FindSimilarGroups(input));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MatchByPatronymic_OneGroup(ISimilarGroupsSolution s)
    {
        SHuman[] input = {
            new SHuman("Иванов", "Алексей", "Петрович", 1980),
            new SHuman("Сидоров", "Борис", "Петрович", 1990),
        };

        List<List<int>> expected = new List<List<int>> { new List<int> { 0, 1 } };

        AssertPartition(input, expected, s.FindSimilarGroups(input));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void MatchByYear_OneGroup(ISimilarGroupsSolution s)
    {
        SHuman[] input = {
            new SHuman("Иванов", "Алексей", "Петрович", 1990),
            new SHuman("Сидоров", "Борис", "Сергеевич", 1990),
        };

        List<List<int>> expected = new List<List<int>> { new List<int> { 0, 1 } };

        AssertPartition(input, expected, s.FindSimilarGroups(input));
    }

    // ---------- Транзитивность ----------

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TransitiveLinkThroughThird_OneGroup(ISimilarGroupsSolution s)
    {
        // Высоцкий и Смыслов между собой ничего не делят.
        // Лановой связан с Высоцким (отчество "Семенович")
        // и со Смысловым (имя "Василий") — должен склеить всех в одну группу.
        SHuman[] input = {
            new SHuman("Высоцкий", "Владимир", "Семенович", 1938),
            new SHuman("Смыслов", "Василий", "Васильевич", 1921),
            new SHuman("Лановой", "Василий", "Семенович", 1934),
        };

        List<List<int>> expected = new List<List<int>> { new List<int> { 0, 1, 2 } };

        AssertPartition(input, expected, s.FindSimilarGroups(input));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ChainOfFive_LinkedByDifferentFields_OneGroup(ISimilarGroupsSolution s)
    {
        // Цепочка: каждый последующий связан с предыдущим РАЗНЫМ полем.
        // 0—1: фамилия,  1—2: имя,  2—3: отчество,  3—4: год.
        // Транзитивно — все в одной группе.
        SHuman[] input = {
            new SHuman("A", "X1", "P1", 1900),
            new SHuman("A", "X2", "P2", 1901),
            new SHuman("B", "X2", "P3", 1902),
            new SHuman("C", "X3", "P3", 1903),
            new SHuman("D", "X4", "P4", 1903),
        };

        List<List<int>> expected = new List<List<int>> { new List<int> { 0, 1, 2, 3, 4 } };

        AssertPartition(input, expected, s.FindSimilarGroups(input));
    }

    // ---------- Слияние нескольких групп одним «связующим» человеком ----------

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void OneLinker_MergesThreeIndependentGroups(ISimilarGroupsSolution s)
    {
        // Сначала три ни с чем не связанные группы (по 1 человеку),
        // затем 4-й — у него с каждой из них общее ОДНО поле:
        //   с #0 — фамилия "A", с #1 — имя "Y", с #2 — отчество "R".
        // Все четверо должны оказаться в одной группе.
        SHuman[] input = {
            new SHuman("A", "X", "P", 1900),
            new SHuman("B", "Y", "Q", 1901),
            new SHuman("C", "Z", "R", 1902),
            new SHuman("A", "Y", "R", 1903),
        };

        List<List<int>> expected = new List<List<int>> { new List<int> { 0, 1, 2, 3 } };

        AssertPartition(input, expected, s.FindSimilarGroups(input));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void OneLinker_MergesTwoOfThreeGroups_ThirdStaysAlone(ISimilarGroupsSolution s)
    {
        // Сначала две независимые группы (#0 и #1), затем независимый #2,
        // затем #3 связан только с #0 и #1 — сливает их, а #2 остаётся отдельно.
        SHuman[] input = {
            new SHuman("A", "X", "P", 1900),
            new SHuman("B", "Y", "Q", 1901),
            new SHuman("Z", "Z", "Z", 1999),
            new SHuman("A", "Y", "W", 1950),
        };

        List<List<int>> expected = new List<List<int>> {
            new List<int> { 0, 1, 3 },
            new List<int> { 2 },
        };

        AssertPartition(input, expected, s.FindSimilarGroups(input));
    }

    // ---------- Крайние случаи "всё одинаковое" / "всё разное" ----------

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void AllUniqueFields_NGroups(ISimilarGroupsSolution s)
    {
        SHuman[] input = {
            new SHuman("A", "X", "P", 1901),
            new SHuman("B", "Y", "Q", 1902),
            new SHuman("C", "Z", "R", 1903),
            new SHuman("D", "W", "S", 1904),
        };

        List<List<int>> expected = new List<List<int>> {
            new List<int> { 0 },
            new List<int> { 1 },
            new List<int> { 2 },
            new List<int> { 3 },
        };

        AssertPartition(input, expected, s.FindSimilarGroups(input));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void AllSameYear_OneGroup(ISimilarGroupsSolution s)
    {
        SHuman[] input = {
            new SHuman("A", "X", "P", 1900),
            new SHuman("B", "Y", "Q", 1900),
            new SHuman("C", "Z", "R", 1900),
            new SHuman("D", "W", "S", 1900),
        };

        List<List<int>> expected = new List<List<int>> { new List<int> { 0, 1, 2, 3 } };

        AssertPartition(input, expected, s.FindSimilarGroups(input));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TwoIdenticalPeople_SameGroup(ISimilarGroupsSolution s)
    {
        // Полные дубли — все 4 поля совпадают. Должны оказаться в одной группе.
        SHuman[] input = {
            new SHuman("Иванов", "Иван", "Иванович", 1980),
            new SHuman("Иванов", "Иван", "Иванович", 1980),
        };

        List<List<int>> expected = new List<List<int>> { new List<int> { 0, 1 } };

        AssertPartition(input, expected, s.FindSimilarGroups(input));
    }

    // ---------- Большой пример из условия задачи ----------

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TaskExample_NinePeople_ThreeGroups(ISimilarGroupsSolution s)
    {
        // Связи:
        //   Пушкин(0)—Суворов(3): имя "Александр"
        //   Пушкин(0)—Володин(6): имя "Александр"
        //   Суворов(3)—Ломоносов(1): отчество "Васильевич"
        //   Тютчев(2)—Менделеев(4): отчество "Иванович"
        //   Менделеев(4)—Верещагин(8): год 1834
        //   Ахматова(5)—Мухина(7): год 1889
        SHuman[] input = {
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

        List<List<int>> expected = new List<List<int>> {
            new List<int> { 0, 1, 3, 6 }, // Пушкин, Ломоносов, Суворов, Володин
            new List<int> { 2, 4, 8 },    // Тютчев, Менделеев, Верещагин
            new List<int> { 5, 7 },       // Ахматова, Мухина
        };

        AssertPartition(input, expected, s.FindSimilarGroups(input));
    }

    // ---------- Инвариант: каждый человек попадает ровно в одну группу ----------

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void EveryPersonAppearsExactlyOnce(ISimilarGroupsSolution s)
    {
        SHuman[] input = {
            new SHuman("A", "X", "P", 1900),
            new SHuman("A", "Y", "Q", 1901),
            new SHuman("B", "Y", "R", 1902),
            new SHuman("C", "Z", "S", 1903),
            new SHuman("D", "W", "T", 1903),
        };

        List<List<SHuman>> result = s.FindSimilarGroups(input);

        int total = 0;
        foreach (List<SHuman> g in result)
        {
            total += g.Count;
        }

        Assert.Equal(input.Length, total);
    }

    // ---------- Вспомогательные методы ----------

    /// <summary>
    /// Сравнивает фактическое разбиение с ожидаемым, абстрагируясь от:
    ///   1) порядка групп в результирующем списке,
    ///   2) порядка людей внутри каждой группы.
    ///
    /// Алгоритм:
    ///   1) Каждого человека из actual сопоставляем с индексом в input
    ///      (с пометкой "использован" — чтобы корректно работать с дублями).
    ///   2) Сортируем индексы внутри каждой группы.
    ///   3) Сортируем сами группы по первому индексу.
    ///   4) То же самое делаем с expected и сравниваем поэлементно.
    /// </summary>
    private static void AssertPartition(
        SHuman[] input,
        List<List<int>> expectedIndexGroups,
        List<List<SHuman>> actualGroups)
    {
        // 1. Превращаем actual в индексы людей в input
        bool[] used = new bool[input.Length];
        List<List<int>> actualIndexGroups = new List<List<int>>();

        foreach (List<SHuman> group in actualGroups)
        {
            List<int> indices = new List<int>();
            foreach (SHuman person in group)
            {
                int idx = FindIndex(input, person, used);
                Assert.True(idx >= 0,
                    $"Человек из результата не найден в input или уже учтён: " +
                    $"{person.Surname} {person.Firstname} {person.Patronymic} {person.Year}");
                indices.Add(idx);
            }
            indices.Sort();
            actualIndexGroups.Add(indices);
        }

        // 2. Каждый человек попал ровно в одну группу
        int actualTotal = 0;
        foreach (List<int> g in actualIndexGroups)
        {
            actualTotal += g.Count;
        }
        Assert.Equal(input.Length, actualTotal);

        // 3. Канонизируем expected
        List<List<int>> expectedSorted = new List<List<int>>();
        foreach (List<int> g in expectedIndexGroups)
        {
            List<int> copy = new List<int>(g);
            copy.Sort();
            expectedSorted.Add(copy);
        }

        // 4. Сортируем сами группы по первому индексу
        actualIndexGroups.Sort((a, b) => a[0].CompareTo(b[0]));
        expectedSorted.Sort((a, b) => a[0].CompareTo(b[0]));

        Assert.Equal(expectedSorted.Count, actualIndexGroups.Count);
        for (int i = 0; i < expectedSorted.Count; i++)
        {
            Assert.Equal(expectedSorted[i], actualIndexGroups[i]);
        }
    }

    /// <summary>
    /// Возвращает индекс первого "ещё не использованного" элемента,
    /// все 4 поля которого совпадают с target. Помечает найденный как использованный.
    /// Нужно для корректной обработки полных дубликатов в input.
    /// </summary>
    private static int FindIndex(SHuman[] arr, SHuman target, bool[] used)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (used[i]) continue;
            if (arr[i].Surname == target.Surname &&
                arr[i].Firstname == target.Firstname &&
                arr[i].Patronymic == target.Patronymic &&
                arr[i].Year == target.Year)
            {
                used[i] = true;
                return i;
            }
        }
        return -1;
    }
}
