using Tasks.Common;

namespace Task12.Subsidies;

[Task(12, "Субсидии")]
public class SubsidiesSolution : ISubsidiesSolution
{
    private const int MaxChildAge = 16;

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

    public struct Parent
    {
        public string FullName;
        public DateTime BirthDay;
        public bool Gender;
        public string ID;
        public Child[] Children;
        public int ChildrenCount;
    }

    public void Run()
    {
        Parent[] parents =
        {
            new Parent
            {
                FullName = "Иванова Мария Петровна",
                BirthDay = new DateTime(1985, 3, 15),
                Gender = false,
                ID = "4510 123456",
                ChildrenCount = 3,
                Children = new[]
                {
                    new Child { FullName = "Иванов Алексей", BirthDay = new DateTime(2014, 6, 10), ID = "0000", Gender = true },
                    new Child { FullName = "Иванова Софья", BirthDay = new DateTime(2017, 9, 22), ID = "0000", Gender = false },
                    new Child { FullName = "Иванов Дмитрий", BirthDay = new DateTime(2020, 1, 5), ID = "0000", Gender = true }
                }
            },
            new Parent
            {
                FullName = "Петров Сергей Иванович",
                BirthDay = new DateTime(1990, 7, 20),
                Gender = true,
                ID = "4512 654321",
                ChildrenCount = 1,
                Children = new[]
                {
                    new Child { FullName = "Петров Максим", BirthDay = new DateTime(2015, 4, 12), ID = "0000", Gender = true }
                }
            }
        };
        int total = 50000;

        Console.WriteLine($"Родителей: {parents.Length}, общая сумма субсидии: {total}");
        for (int i = 0; i < parents.Length; i++)
        {
            Console.WriteLine($"  {parents[i].FullName} — детей: {parents[i].Children.Length}");
            foreach (var child in parents[i].Children)
                Console.WriteLine($"    {child.FullName}, возраст {child.Age}");
        }

        Console.WriteLine();
        double[] subsidies = CalculateSubsidies(parents, total);
        for (int i = 0; i < parents.Length; i++)
            Console.WriteLine($"  {parents[i].FullName}: субсидия = {subsidies[i]:F2}");
    }

    public double[] CalculateSubsidies(Parent[] parents, int total)
    {
        double[] subsidies = new double[parents.Length];
        double[] coefficients = new double[parents.Length];
        double totalCoefficients = 0;

        for (int i = 0; i < parents.Length; i++)
        {
            Child[] children = parents[i].Children;
            
            if (children == null || children.Length < 2)
                continue;

            for (int j = 0; j < children.Length; j++)
            {
                if (children[j].Age <= MaxChildAge)
                {
                    double childCoefficient = 1 + j * 0.1;
                    coefficients[i] += childCoefficient;
                    totalCoefficients += childCoefficient;
                }
            }
        }

        if (totalCoefficients == 0)
            return subsidies;

        double baseSubsidy = total / totalCoefficients;

        for (int i = 0; i < parents.Length; i++)
            subsidies[i] = baseSubsidy * coefficients[i];

        return subsidies;
    }
}
