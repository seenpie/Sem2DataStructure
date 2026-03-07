using Tasks.Common;

namespace Task09.CrossWord;

public interface ICrossWordSolution : ISolution
{
    /// <summary>
    /// Подбирает горизонтальные слова для микрокроссворда.
    /// Возвращает массив длиной keyWord.Length, где result[i] — слово,
    /// содержащее keyWord[i], либо null, если построить кроссворд невозможно.
    /// </summary>
    string[]? Solve(string[] wordBase, string keyWord);
}
