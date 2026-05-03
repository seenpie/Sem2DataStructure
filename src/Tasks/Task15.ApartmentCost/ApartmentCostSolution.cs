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
    /// <summary>
    /// Вычисляет медиану цены квартир, удовлетворяющих фильтру.
    ///
    /// Алгоритм:
    ///   1. Фильтрация — проходим по списку, отбираем цены квартир,
    ///      у которых кол-во комнат == rooms и общая площадь > totalSquare.
    ///   2. Сортировка — упорядочиваем отобранные цены по возрастанию.
    ///   3. Медиана — центральное значение отсортированного массива:
    ///      - нечётное кол-во: средний элемент          [1, 3, 5] → 3
    ///      - чётное кол-во:   среднее двух центральных [1, 3, 5, 7] → (3+5)/2 = 4
    ///
    /// Сложность:
    ///   Время:  O(n + k log k), где n — всего квартир, k — подходящих
    ///   Память: O(k) — список отфильтрованных цен
    /// </summary>
    /// <returns>Медиана цены, или -1 если подходящих квартир нет</returns>
    public double CalculateMedianPrice(List<SApartment> apartments, int rooms, double totalSquare)
    {
        // Шаг 1: фильтрация — отбираем цены подходящих квартир — O(n)
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

        // Шаг 2: сортировка цен по возрастанию — O(k log k)
        prices.Sort();

        // Шаг 3: вычисление медианы — O(1)
        int mid = prices.Count / 2;

        // Нечётное кол-во — берём средний элемент
        if (prices.Count % 2 == 1)
            return prices[mid];

        // Чётное кол-во — среднее арифметическое двух центральных
        return (prices[mid - 1] + prices[mid]) / 2.0;
    }
}
