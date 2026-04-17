using Tasks.Common;

namespace Task12.Subsidies;

[Task(12, "Субсидии", "alt")]
public class SubsidiesSolutionAlt : ISubsidiesSolution
{
    public struct Child
    {
        public string FullName;
        public DateTime BirthDay;
        public string ID;
        public bool Gender;
        public int Age
        {
            get
            {
                TimeSpan ts = DateTime.Now - BirthDay;
                DateTime res = DateTime.MinValue.Add(ts);
                return res.Year - 1;
            }
        }
    }

    // public struct Parent
    // {
    //     public string FullName;
    //     public DateTime BirthDay;
    //     public bool Gender;
    //     public int Age
    //     {
    //         get
    //         {
    //             TimeSpan ts = DateTime.Now - BirthDay;
    //             DateTime res = DateTime.MinValue.Add(ts);
    //             return res.Year - 1;
    //         }
    //     }
    //     public string ID;
    //     public Child[] Children;
    //     public int ChildrenCount
    //     {
    //         get
    //         {
    //             return Children.Length;
    //         }
    //     }
    // }

    public void Run()
    {
        SubsidiesSolution.Parent[] testData = CreateTestArray();
        int total = 10000;
        double[] subsidies = CalculateSubsidies(testData, total);

        double subsSum = 0;
        foreach (double sub in subsidies)
        {
            Console.WriteLine(sub);
            subsSum += sub;
        }
        Console.WriteLine(subsSum);
    }

    public double[] CalculateSubsidies(SubsidiesSolution.Parent[] parents, int total)
    {
        double[] subsidies = new double[parents.Length];
        double coeffSum = 0;

        for (int i = 0; i < parents.Length; i++)
        {
            SubsidiesSolution.Parent parent = parents[i];
            if (!CanGetSubsidy(parent, out int childrenForSubs)) continue;
            double coeff = CalcCoeff(childrenForSubs);
            subsidies[i] = coeff;
            coeffSum += coeff;
        }

        double subBase = total / coeffSum;

        for (int i = 0; i < subsidies.Length; i++)
        {
            subsidies[i] *= subBase;
        }

        return subsidies;
    }

    private static bool CanGetSubsidy(SubsidiesSolution.Parent parent, out int childrenForSubs)
    {
        childrenForSubs = 0;

        if (parent.Children.Length < 2) return false;

        foreach (SubsidiesSolution.Child child in parent.Children)
        {
            if (child.Age <= 16) childrenForSubs++;
        }

        return childrenForSubs > 0;
    }

    private static double CalcCoeff(int childrenForSubs)
    {
        // свернул Σ(1 + i * 0.1) (для i = 0 ... childrenForSubs - 1)
        // в n * (0.95 + 0.05 * n)
        return childrenForSubs * (0.95 + 0.05 * childrenForSubs);
    }

    public SubsidiesSolution.Parent[] CreateTestArray()
    {
        SubsidiesSolution.Child c1 = new() { BirthDay = new DateTime(2010, 5, 2) };
        SubsidiesSolution.Child c2 = new() { BirthDay = new DateTime(2015, 8, 20) };
        SubsidiesSolution.Child c3 = new() { BirthDay = new DateTime(2018, 3, 10) };
        SubsidiesSolution.Child c4 = new() { BirthDay = new DateTime(2005, 1, 15) };
        SubsidiesSolution.Child c5 = new() { BirthDay = new DateTime(2020, 11, 30) };
        SubsidiesSolution.Child c6 = new() { BirthDay = new DateTime(1998, 7, 7) };

        SubsidiesSolution.Parent p1 = new() { Children = [c1, c2, c3] };
        SubsidiesSolution.Parent p2 = new() { Children = [c2, c3, c4] };
        SubsidiesSolution.Parent p3 = new() { Children = [c3, c4, c5] };
        SubsidiesSolution.Parent p4 = new() { Children = [c4, c5, c6] };
        SubsidiesSolution.Parent p5 = new() { Children = [c5, c1, c2, c3] };
        SubsidiesSolution.Parent p6 = new() { Children = [c1, c4] };
        SubsidiesSolution.Parent p7 = new() { Children = [c4, c6, c6] };
        SubsidiesSolution.Parent p8 = new() { Children = [] };

        return [p1, p2, p3, p4, p5, p6, p7, p8];
    }
}
