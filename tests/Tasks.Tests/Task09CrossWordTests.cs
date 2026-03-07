using Task09.CrossWord;

namespace Tasks.Tests;

public class Task09CrossWordTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new CrossWordSolution()];
    }

    // ── Вспомогательный метод ─────────────────────────────────────────────

    private static void AssertValidCrossword(string[] result, string keyWord)
    {
        Assert.Equal(keyWord.Length, result.Length);
        for (int i = 0; i < keyWord.Length; i++)
            Assert.Contains(keyWord[i], result[i]);
        Assert.Equal(result.Length, result.Distinct().Count());
    }

    // ── Примеры из условия ────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Example1_лопух(ICrossWordSolution s)
    {
        string[] wordBase = ["смех", "потолок", "булка", "плющ", "бум"];
        string[]? result = s.Solve(wordBase, "лопух");
        Assert.NotNull(result);
        AssertValidCrossword(result, "лопух");
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Example2_комп(ICrossWordSolution s)
    {
        string[] wordBase = ["экран", "мышь", "блок", "процессор"];
        string[]? result = s.Solve(wordBase, "комп");
        Assert.NotNull(result);
        AssertValidCrossword(result, "комп");
    }

    /// <summary>Оригинальный тест 3 из Main — невозможный случай: только одно слово с 'м'.</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Example3_мама_Impossible(ICrossWordSolution s)
    {
        string[] wordBase = ["мир", "арбуз", "астра", "лето"];
        Assert.Null(s.Solve(wordBase, "мама"));
    }

    /// <summary>Расширенная база — кроссворд для "мама" становится возможным.</summary>
    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Example3_мама_Possible(ICrossWordSolution s)
    {
        string[] wordBase = ["мир", "арбуз", "масло", "астра"];
        string[]? result = s.Solve(wordBase, "мама");
        Assert.NotNull(result);
        AssertValidCrossword(result, "мама");
    }

    // ── Простые случаи ────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SingleLetterKeyWord(ICrossWordSolution s)
    {
        string[]? result = s.Solve(["кот"], "к");
        Assert.NotNull(result);
        AssertValidCrossword(result, "к");
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void WordInBaseMatchesKeyWord(ICrossWordSolution s)
    {
        string[]? result = s.Solve(["мир", "рак", "яма"], "мир");
        Assert.NotNull(result);
        AssertValidCrossword(result, "мир");
    }

    // ── Слово содержит несколько букв ключа — не используется дважды ──────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NoWordUsedTwice(ICrossWordSolution s)
    {
        // "аист" содержит и 'а', и 'и' — не должно использоваться дважды
        string[] wordBase = ["аист", "рот", "ель", "нос"];
        string[]? result = s.Solve(wordBase, "арен");
        Assert.NotNull(result);
        AssertValidCrossword(result, "арен");
    }

    // ── Невозможные случаи ────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Impossible_NoWordContainsLetter(ICrossWordSolution s)
    {
        string[] wordBase = ["кот", "дом", "лес"];
        Assert.Null(s.Solve(wordBase, "ю"));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Impossible_WordBaseSmallerThanKeyWord(ICrossWordSolution s)
    {
        string[] wordBase = ["кот", "дом"];
        Assert.Null(s.Solve(wordBase, "море"));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Impossible_NotEnoughUniqueWords(ICrossWordSolution s)
    {
        // Ключ "аа" — нужны два разных слова с 'а', а есть только одно
        string[] wordBase = ["арка", "бор", "сон"];
        Assert.Null(s.Solve(wordBase, "аа"));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Impossible_EmptyWordBase(ICrossWordSolution s)
    {
        Assert.Null(s.Solve([], "ключ"));
    }

    // ── Структура результата ──────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ResultLength_EqualsKeyWordLength(ICrossWordSolution s)
    {
        string[]? result = s.Solve(["смех", "потолок", "булка", "плющ", "бум"], "лопух");
        Assert.NotNull(result);
        Assert.Equal("лопух".Length, result.Length);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ResultWords_AreFromWordBase(ICrossWordSolution s)
    {
        string[] wordBase = ["смех", "потолок", "булка", "плющ", "бум"];
        string[]? result = s.Solve(wordBase, "лопух");
        Assert.NotNull(result);
        Assert.All(result, word => Assert.Contains(word, wordBase));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ResultWords_AreUnique(ICrossWordSolution s)
    {
        string[]? result = s.Solve(["смех", "потолок", "булка", "плющ", "бум"], "лопух");
        Assert.NotNull(result);
        Assert.Equal(result!.Length, result.Distinct().Count());
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void EachResultWord_ContainsCorrectKeyLetter(ICrossWordSolution s)
    {
        string keyWord = "лопух";
        string[]? result = s.Solve(["смех", "потолок", "булка", "плющ", "бум"], keyWord);
        Assert.NotNull(result);
        for (int i = 0; i < keyWord.Length; i++)
            Assert.Contains(keyWord[i], result[i]);
    }

    // ── Ключ из одной буквы встречается в нескольких словах ───────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void KeyWordOneChar_MultipleMatches_ReturnsSome(ICrossWordSolution s)
    {
        string[]? result = s.Solve(["арка", "аист", "яма"], "а");
        Assert.NotNull(result);
        AssertValidCrossword(result, "а");
    }
}
