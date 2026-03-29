using Tasks.Common;

namespace Task11.SegmentsSort;

[Task(11, "Сортировка отрезков", "alt")]
public class SegmentsSortSolutionAlt : ISegmentsSortSolution
{
    public struct Point
    {
        public readonly double X;
        public readonly double Y;

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }
    }

    public struct Segment
    {
        public readonly Point P1;
        public readonly Point P2;
        public readonly string Color;
        public readonly double Width;
        public readonly double LengthSquared;

        public Segment(Point p1, Point p2, string color, double width)
        {
            P1 = p1;
            P2 = p2;
            Color = color;
            Width = width;

            Point P3 = P1 - P2;
            LengthSquared = P3.X * P3.X + P3.Y * P3.Y;
        }

        public double GetLength()
        {
            return Math.Sqrt(LengthSquared);
        }
    }

    public void Run()
    {
        Segment[] data1 = [new Segment(new Point(2, 3), new Point(5, 1), "red", 1.0)];
        Segment[] data2 = [
            new Segment(new Point(10, 20), new Point(30, 40), "red", 1.2),
            new Segment(new Point(0, 0), new Point(100, 0), "green", 2.0),
            new Segment(new Point(5, 5), new Point(8, 9), "blue", 0.8),
            new Segment(new Point(50, 50), new Point(55, 55), "yellow", 1.5),
            new Segment(new Point(1, 1), new Point(4, 5), "purple", 1.0),
            new Segment(new Point(20, 15), new Point(25, 20), "orange", 1.3),
            new Segment(new Point(0, 10), new Point(10, 0), "cyan", 1.1),
            new Segment(new Point(3, 7), new Point(8, 2), "magenta", 1.8)
        ];
        Segment[] data3 = [
            new Segment(new Point(10, 20), new Point(30, 40), "red", 1.2),
            new Segment(new Point(0, 0), new Point(100, 0), "green", 2.0),
            new Segment(new Point(5, 5), new Point(8, 9), "blue", 0.8),
            new Segment(new Point(50, 50), new Point(55, 55), "yellow", 1.5),
            new Segment(new Point(1, 1), new Point(4, 5), "purple", 1.0),
            new Segment(new Point(20, 15), new Point(25, 20), "orange", 1.3),
            new Segment(new Point(0, 10), new Point(10, 0), "cyan", 1.1),
            new Segment(new Point(3, 7), new Point(8, 2), "magenta", 1.8)
        ];

        RunTest(data1, "test 1 (по возрастанию)");
        RunTest(data2, "test 2 (по возрастанию)");
        RunTest(data3, "test 3 (по убыванию)", false);
    }

    private void RunTest(Segment[] data, string label, bool asc = true)
    {
        Console.WriteLine(label);
        Console.WriteLine("до");
        Print(data);
        SortSegments(data, asc);
        Console.WriteLine("после");
        Print(data);
        Console.WriteLine();
    }

    private void Print(Segment[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            Segment seg = data[i];
            Console.WriteLine($"{i + 1}) длина={seg.GetLength():F2};P1:[{seg.P1.X},{seg.P1.Y}];P2:[{seg.P2.X},{seg.P2.Y}]цвет={seg.Color};толщина={seg.Width}");
        }
    }

    private void SortSegments(Segment[] segments, bool asc = true)
    {
        QuickSort(segments, 0, segments.Length - 1, asc);
    }

    private void QuickSort(Segment[] segments, int low, int high, bool asc)
    {
        if (low >= high) return;

        int j = Partition(segments, low, high, asc);
        QuickSort(segments, low, j - 1, asc);
        QuickSort(segments, j + 1, high, asc);
    }

    private int Partition(Segment[] segments, int low, int high, bool asc)
    {
        int i = low;
        int j = high + 1;
        double pivot = segments[low].LengthSquared;

        while (true)
        {
            do
            {
                i++;
                if (i == high) break;
            }
            while (asc ? pivot > segments[i].LengthSquared : pivot < segments[i].LengthSquared);

            do
            {
                j--;
            }
            while (asc ? pivot < segments[j].LengthSquared : pivot > segments[j].LengthSquared);

            if (i >= j) break;

            (segments[i], segments[j]) = (segments[j], segments[i]);
        }

        (segments[low], segments[j]) = (segments[j], segments[low]);
        return j;
    }
}
