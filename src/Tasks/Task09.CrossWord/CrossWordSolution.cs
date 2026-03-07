using Tasks.Common;

namespace Task09.CrossWord;

[Task(9, "Кроссворд")]
public class CrossWordSolution : ICrossWordSolution
{
    public void Run()
    {
        int wordCount = int.Parse(Console.ReadLine()!);
        string[] wordBase = new string[wordCount];
        for (int i = 0; i < wordCount; i++)
            wordBase[i] = Console.ReadLine()!;
        string keyWord = Console.ReadLine()!;

        string[]? result = Solve(wordBase, keyWord);
        if (result is null)
            Console.WriteLine("Невозможно построить кроссворд с таким набором слов.");
        else
            PrintCrossword(result, keyWord);
    }

    public string[]? Solve(string[] wordBase, string keyWord)
    {
        if (wordBase.Length < keyWord.Length)
            return null;

        string[] resultWords = new string[keyWord.Length];
        bool[] usedWords = new bool[wordBase.Length];

        return TryFindSolution(wordBase, keyWord, usedWords, resultWords, 0)
            ? resultWords
            : null;
    }

    private static bool TryFindSolution(
        string[] wordBase, string keyWord,
        bool[] usedWords, string[] horizontalWords, int currentIndex)
    {
        if (currentIndex == keyWord.Length)
            return true;

        for (int i = 0; i < wordBase.Length; i++)
        {
            string currentWord = wordBase[i];
            if (!usedWords[i] && currentWord.Contains(keyWord[currentIndex]))
            {
                usedWords[i] = true;
                horizontalWords[currentIndex] = currentWord;

                if (TryFindSolution(wordBase, keyWord, usedWords, horizontalWords, currentIndex + 1))
                    return true;

                usedWords[i] = false;
                horizontalWords[currentIndex] = "";
            }
        }

        return false;
    }

    private static void PrintCrossword(string[] words, string keyWord)
    {
        int maxLeftPadding = 0;
        for (int i = 0; i < words.Length; i++)
        {
            int idx = words[i].IndexOf(keyWord[i]);
            if (idx > maxLeftPadding)
                maxLeftPadding = idx;
        }

        for (int i = 0; i < words.Length; i++)
        {
            string word = words[i];
            int letterIndex = word.IndexOf(keyWord[i]);
            int spaceCount = (maxLeftPadding - letterIndex) * 2;
            Console.Write(new string(' ', spaceCount));

            for (int k = 0; k < word.Length; k++)
            {
                if (k == letterIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(word[k]);
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(word[k]);
                }
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
}
