using Tasks.Common;

namespace Task02.Nword;

[Task(2, "N-буквенные слова", "alt")]
public class NwordAlt : INwordSolution
{
  private const string Letters = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

  public void Run()
  {
      byte m = ReadByteInRange("введите m", 1, 33);
      byte n = ReadByteInRange("введите n", 1, 33);

      try
      {
          Console.WriteLine("итеративно:");
          PrintIterative(n, m);
          Console.WriteLine("рекурсивно:");
          PrintRecursive(n, m);
      }
      catch (OverflowException)
      {
          Console.WriteLine($"Ошибка: Результат {m}^{n} слишком велик и не помещается в стандартное целое число.");
      }
      catch (InvalidOperationException e)
      {
          Console.WriteLine(e.Message);
      }
  }

  public IEnumerable<string> GenerateWords(int wordLength, string alphabet)
  {
      throw new NotImplementedException();
  }

  private static byte ReadByteInRange(string label, byte min, byte max)
  {
      Console.WriteLine(label);
      Console.WriteLine($"Значение от {min} до {max} (включительно)");

      if (byte.TryParse(Console.ReadLine(), out byte input) && input >= min && input <= max)
      {
          return input;
      }

      Console.WriteLine("Ошибка ввода, попробуйте еще раз");
      return ReadByteInRange(label, min, max);
  }

  private static void PrintIterative(int n, int m)
  {
      int length = checked((int)Math.Pow(m, n));
      // чтобы не вызывать I/O на каждый символ
      char[] buffer = new char[n];

      for (int i = 0; i < length; i++)
      {
          int temp = i;

          // система счисления = m, нужно перевести из 10-сс в m-сс
          for (int j = n - 1; j >= 0; j--)
          {
              int digit = temp % m;
              buffer[j] = Letters[digit];
              temp /= m;
          }

          Console.WriteLine(new string(buffer));
      }
  }

  private static void PrintRecursive(int n, int m)
  {
      int length = checked((int)Math.Pow(m, n));

      if (length > 1_000_000)
      {
          throw new InvalidOperationException($"Количество комбинаций ({length:N0}) слишком велико");
      }

      char[] buffer = new char[n];
      PrintRecursiveWithBuffer(n, m, buffer);
  }

  // сначала в ширину, потом вглубь
  private static void PrintRecursiveWithBuffer(int n, int m, char[] buffer, int currentLetterIndex = 0, int pos = 0)
  {
      if (currentLetterIndex == m) return;

      buffer[pos] = Letters[currentLetterIndex];

      if (pos == n - 1)
      {
          Console.WriteLine(new string(buffer));
      }
      else
      {
          PrintRecursiveWithBuffer(n, m, buffer, 0, pos + 1);
      }

      PrintRecursiveWithBuffer(n, m, buffer, currentLetterIndex + 1, pos);
  }
}
