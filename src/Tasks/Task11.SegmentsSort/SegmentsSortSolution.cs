using Tasks.Common;

namespace Task11.SegmentsSort;

[Task(11, "Сортировка отрезков")]
public class SegmentsSortSolution : ISegmentsSortSolution
{
    public void Run()
    {
        Segment[] segments =
        [
            new(new Segment.Point(0, 0), new Segment.Point(3,  4),  1.0, ConsoleColor.Red),    // длина 5
            new(new Segment.Point(1, 1), new Segment.Point(6,  13), 2.0, ConsoleColor.Blue),   // длина 13
            new(new Segment.Point(0, 0), new Segment.Point(1,  0),  1.5, ConsoleColor.Green),  // длина 1
            new(new Segment.Point(2, 3), new Segment.Point(5,  7),  1.0, ConsoleColor.Yellow), // длина 5
            new(new Segment.Point(0, 0), new Segment.Point(8,  6),  2.5, ConsoleColor.White),  // длина 10
        ];

        Console.WriteLine("До сортировки:");
        PrintSegments(segments);

        QuickSegmentsSort(segments, 0, segments.Length - 1);

        Console.WriteLine("\nПосле сортировки (по возрастанию длины):");
        PrintSegments(segments);
    }

    private static void PrintSegments(Segment[] segments)
    {
        foreach (Segment seg in segments)
            Console.WriteLine($"  [{seg.StartPoint.X};{seg.StartPoint.Y}] -> [{seg.EndPoint.X};{seg.EndPoint.Y}]" +
                              $"  цвет={seg.ColorLine}  толщина={seg.WidthLine}");
    }

    public struct Segment
    {
        public struct Point
        {
            public double X;
            public double Y;

            public Point(double x, double y)
            {
                X = x;
                Y = y;
            }
        }

        public Point StartPoint;
        public Point EndPoint;
        public double WidthLine;
        public ConsoleColor ColorLine;

        public Segment(Point start, Point end, double width, ConsoleColor color)
        {
            StartPoint = start;
            EndPoint = end;
            WidthLine = width;
            ColorLine = color;
        }

        public double GetQuadraticLength()
        {
            double dX = EndPoint.X - StartPoint.X;
            double dY = EndPoint.Y - StartPoint.Y;
            return dX * dX + dY * dY;
        }
    }

    public static void QuickSegmentsSort(Segment[] arr, int low, int high)
    {
        if (low >= high)
            return;

        int pivot = Partition(arr, low, high);

        QuickSegmentsSort(arr, low, pivot - 1); 
        QuickSegmentsSort(arr, pivot + 1, high); 
    }

    private static int Partition(Segment[] arr, int low, int high)
    {
        Segment pivot = arr[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (arr[j].GetQuadraticLength() < pivot.GetQuadraticLength())
                Swap(++i, j, arr); 
        }

        Swap(i+1, high, arr); 

        return i + 1;
    }

    private static void Swap(int i, int j, Segment[] arr)
    {
        Segment temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }
}
