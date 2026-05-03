using Tasks.Common;

namespace Task16.SimilarGroups;

public interface ISimilarGroupsSolution : ISolution
{
    public List<List<SHuman>> FindSimilarGroups(SHuman[] group);
}
