using Tasks.Common;

namespace Task01.Kaleidoscope;

[Task(1, "Калейдоскоп", "alt")]
public class KaleidoscopeSolutionAlt: IKaleidoscopeSolution
{ 
    private readonly int[] Colors = [0, 8, 7, 15, 14, 6, 12, 4, 5, 13, 11, 3, 9, 1, 2, 10];
    private const int SingleBlockWidth = 1;
    private const char SingleBlockSymbol = ' ';

    public void Run()
    {
        byte input = ReadByteInRange("Введите размер калейдоскопа (половина стороны)", 3, 20);
        DrawKaleidoscope(input);
    }

    private byte ReadByteInRange(string label, byte min, byte max)
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

    public void DrawKaleidoscope(int size)
    {
        if (size < 3 || size > 20) throw new ArgumentException("argument must be 3 <= size <= 20");

        int[,] kaleidoscope = GenerateKaleidoscopeInner(size);

        // отрисовка верхней части
        for (int row = 0; row < size; row++)
        {
            DrawRow(row, size, kaleidoscope);
        }
        
        // отрисовка нижней части
        for (int row = size - 1; row >= 0; row--)
        {
            DrawRow(row, size, kaleidoscope);
        }
    }

    private int[,] GenerateKaleidoscopeInner(int size)
    {
        int[,] kaleidoscope = new int[size, size];

        for (int i = 0; i < size; i++)
        {
            int? prevColorIndex = null;
            for (int j = 0; j < size; j++)
            {
                if (i > j)
                {
                    prevColorIndex = kaleidoscope[j, i];
                    kaleidoscope[i, j] = kaleidoscope[j, i];
                }
                else
                {
                    int currentIndex = GetRandomIndex(Colors.Length, prevColorIndex);
                    kaleidoscope[i, j] = currentIndex;
                    prevColorIndex = currentIndex;
                }
            }
        }

        return kaleidoscope;
    }

    private int GetRandomIndex(int range, int? prevIndex)
    {
        int randomIndex;
        
        if (prevIndex.HasValue)
        {
            int value;
            do
            { 
                value = Random.Shared.Next(-2, 3);
            } while (value == 0);
            
            randomIndex = (prevIndex.Value + value) % range;
            if (randomIndex < 0) randomIndex += range;
        }
        else
        {
            randomIndex = Random.Shared.Next(0, range);
        }
        
        return randomIndex;
    }

    private void DrawRow(int row, int size, int[,] kaleidoscope)
    { 
        // отрисовка левой части
        for (int col = 0; col < size; col++)
        {
            DrawBlock((ConsoleColor)Colors[kaleidoscope[row, col]]);
        }
          
        // отрисовка правой части
        for (int col = size - 1; col >= 0; col--)
        { 
            DrawBlock((ConsoleColor)Colors[kaleidoscope[row, col]]);
        }

        Console.WriteLine();
    }

    private void DrawBlock(ConsoleColor color)
    {
        Console.BackgroundColor = color;
        Console.Write(new string(SingleBlockSymbol, SingleBlockWidth));
        Console.ResetColor();
    }

    public int[,] GenerateKaleidoscope(int halfSize)
    {
        throw new NotImplementedException();
    }

    public void PrintKaleidoscope(int[,] matrix)
    {
        throw new NotImplementedException();
    }
}