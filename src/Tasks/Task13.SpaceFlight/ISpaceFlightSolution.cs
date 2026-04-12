using Tasks.Common;

namespace Task13.SpaceFlight;

public interface ISpaceFlightSolution : ISolution
{
    double[] SelectRichest(double[] applicants, int total);
}
