using Tasks.Common;

namespace Task12.Subsidies;

public interface ISubsidiesSolution : ISolution
{
    double[] CalculateSubsidies(SubsidiesSolution.Parent[] parents, int total);
}
