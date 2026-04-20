using Tasks.Common;

namespace Task15.ApartmentCost;

[Task(15, "Подсчет стоимости квартир", "alt")]
public class ApartmentCostSolutionAlt : IApartmentCostSolution
{
    public readonly struct Address(string city, string street, int floor, int doorNumber, int houseNumber)
    {
        public string City { get; } = city;
        public string Street { get; } = street;
        public int HouseNumber { get; } = houseNumber;
        public int DoorNumber { get; } = doorNumber;
        public int Floor { get; } = floor;
    }

    public readonly struct Apartment
    {
        public Address Address { get; }
        public IReadOnlyList<double> LivingSquares { get; }
        public double KitchenSquare { get; }
        public double OtherSquare { get; }
        public double TotalSquare { get; }
        public int RoomCount { get; }

        public Apartment(Address address, List<double> livingSquares, double kitchenSquare, double otherSquare)
        {
            if (livingSquares.Count < 1)
                throw new ArgumentException("Кол-во жилых комнат должно быть больше 0", nameof(livingSquares));

            double livingSquaresSum = 0;
            foreach (double square in livingSquares)
            {
                if (square <= 0)
                    throw new ArgumentException("Площадь каждой комнаты должна быть положительной", nameof(livingSquares));
                livingSquaresSum += square;
            }

            if (kitchenSquare < 0 || otherSquare < 0)
                throw new ArgumentException("Площадь не должна быть отрицательной");

            Address = address;
            LivingSquares = livingSquares.ToArray();
            KitchenSquare = kitchenSquare;
            OtherSquare = otherSquare;
            RoomCount = LivingSquares.Count;
            TotalSquare = livingSquaresSum + kitchenSquare + otherSquare;
        }
    }

    public readonly struct SaleOffer(decimal price, Apartment apartment)
    {
        public decimal Price { get; } = price;
        public Apartment Apartment { get; } = apartment;
    }

    public void Run()
    {
        List<SaleOffer> testOffers = new List<SaleOffer>
        {
        new SaleOffer(
            100000,
            new Apartment(
            new Address("A", "S1", 1, 10, 1),
            new List<double> { 15 },
            5,
            2
            )
        ),
        new SaleOffer(
            200000,
            new Apartment(
            new Address("B", "S2", 2, 20, 2),
            new List<double> { 12, 10 },
            6, 3
            )
        ),
        new SaleOffer(
            250000,
            new Apartment(
            new Address("A", "S3", 3, 30, 3),
            new List<double> { 14, 11 },
            7, 4
            )
        ),
        new SaleOffer(
            180000,
            new Apartment(
            new Address("A", "S1", 4, 40, 1),
            new List<double> { 10, 8, 7 },
            5, 2
            )
        ),
        new SaleOffer(
            120000,
            new Apartment(
            new Address("C", "S4", 1, 15, 4),
            new List<double> { 20 },
            8, 5
            )
        )
        };

        decimal median = CalcMedian(testOffers, 2, 30);
        Console.WriteLine($"Медиана: {median}");
    }

    public decimal CalcMedian(List<SaleOffer> saleOffers, int rooms, double totalSquare)
    {
        List<decimal> prices = FilterAparts(saleOffers, rooms, totalSquare);
        if (prices.Count < 1)
            throw new InvalidOperationException("Нет подходящих объявлений");

        if ((prices.Count & 1) == 0)
        {
            decimal val1 = QuickSelect(prices, (prices.Count / 2) - 1, 0, prices.Count - 1);
            decimal val2 = QuickSelect(prices, prices.Count / 2, 0, prices.Count - 1);
            return (val1 + val2) / 2;
        }

        return QuickSelect(prices, prices.Count / 2, 0, prices.Count - 1);
    }

    private T QuickSelect<T>(List<T> prices, int idx, int left, int right) where T : IComparable<T>
    {
        if (idx < left || idx > right)
            throw new ArgumentOutOfRangeException();

        while (left <= right)
        {
            int pivotIdx = Partition(prices, left, right);

            if (idx == pivotIdx)
            {
                return prices[idx];
            }
            else if (idx > pivotIdx)
            {
                left = pivotIdx + 1;
            }
            else
            {
                right = pivotIdx - 1;
            }
        }

        throw new InvalidOperationException();
    }

    private static List<decimal> FilterAparts(List<SaleOffer> saleOffers, int rooms, double totalSquare)
    {
        List<decimal> prices = [];

        foreach (SaleOffer offer in saleOffers)
        {
            Apartment apart = offer.Apartment;
            if (apart.TotalSquare > totalSquare && apart.RoomCount == rooms)
            {
                prices.Add(offer.Price);
            }
        }

        return prices;
    }

    private int Partition<T>(List<T> prices, int left, int right) where T : IComparable<T>
    {
        int i = left;
        int j = right + 1;
        T pivot = prices[left];

        while (true)
        {
            do { i++; }
            while (i <= right && pivot.CompareTo(prices[i]) > 0);

            do { j--; }
            while (pivot.CompareTo(prices[j]) < 0);

            if (i >= j) break;

            (prices[i], prices[j]) = (prices[j], prices[i]);
        }

        (prices[left], prices[j]) = (prices[j], prices[left]);
        return j;
    }

    public double CalculateMedianPrice(List<ApartmentCostSolution.SApartment> apartments, int rooms, double totalSquare)
    {
        List<double> prices = [.. apartments
            .Where(apt => apt.RoomCount == rooms && apt.TotalArea > totalSquare)
            .Select(apt => apt.Price)];

        if (prices.Count < 1)
            return -1;

        if ((prices.Count & 1) == 0)
        {
            double val1 = QuickSelect(prices, (prices.Count / 2) - 1, 0, prices.Count - 1);
            double val2 = QuickSelect(prices, prices.Count / 2, 0, prices.Count - 1);
            return (val1 + val2) / 2;
        }

        return QuickSelect(prices, prices.Count / 2, 0, prices.Count - 1);
    }
}
