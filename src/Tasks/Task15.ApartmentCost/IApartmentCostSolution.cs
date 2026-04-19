using Tasks.Common;

namespace Task15.ApartmentCost;

public interface IApartmentCostSolution : ISolution
{
    double CalculateMedianPrice(List<ApartmentCostSolution.SApartment> apartments, int rooms, double totalSquare);
}
