using Tasks.Common;

namespace Task15.ApartmentCost;

[Task(15, "Подсчет стоимости квартир")]
public class ApartmentCostSolution : IApartmentCostSolution
{
    public struct SApartment
    {
        public double[] RoomAreas;
        public double KitchenArea;
        public double OtherArea;
        public double Price;

        public int RoomCount 
        {
            get 
            {
                return RoomAreas.Length;
            }
        }

        public double TotalArea
        {
            get
            {
                double total = KitchenArea + OtherArea;
                if (RoomAreas.Length > 0)
                {
                    for (int i = 0; i < RoomAreas.Length; i++)
                        total += RoomAreas[i];
                }
                return total;
            }
        }
    }

    public void Run()
    {
        var apartments = new List<SApartment>
        {
            new SApartment
            {
                RoomAreas = new[] { 18.0, 14.0 },
                KitchenArea = 10.0,
                OtherArea = 8.0,
                Price = 5_000_000
            },
            new SApartment
            {
                RoomAreas = new[] { 20.0, 16.0 },
                KitchenArea = 12.0,
                OtherArea = 10.0,
                Price = 6_500_000
            },
            new SApartment
            {
                RoomAreas = new[] { 15.0, 12.0 },
                KitchenArea = 8.0,
                OtherArea = 5.0,
                Price = 4_000_000
            },
            new SApartment
            {
                RoomAreas = new[] { 22.0, 18.0, 14.0 },
                KitchenArea = 14.0,
                OtherArea = 12.0,
                Price = 9_000_000
            },
        };

        int rooms = 2;
        double totalSquare = 40.0;

        Console.WriteLine($"Квартир: {apartments.Count}");
        Console.WriteLine($"Фильтр: комнат = {rooms}, площадь > {totalSquare}");

        double median = CalculateMedianPrice(apartments, rooms, totalSquare);
        Console.WriteLine($"Медиана цены: {median:F2}");
    }
    public double CalculateMedianPrice(List<SApartment> apartments, int rooms, double totalSquare)
    {
        // Шаг 1: отбираем цены подходящих квартир
        var prices = new List<double>();

        for (int i = 0; i < apartments.Count; i++)
        {
            if (apartments[i].RoomCount == rooms && apartments[i].TotalArea > totalSquare)
            {
                prices.Add(apartments[i].Price);
            }
        }

        if (prices.Count == 0)
            return -1;

        // Шаг 2: сортируем цены для вычисления медианы
        prices.Sort();

        // Шаг 3: вычисляем медиану
        int mid = prices.Count / 2;

        if (prices.Count % 2 == 1)
            return prices[mid];

        return (prices[mid - 1] + prices[mid]) / 2.0;
    }
}
