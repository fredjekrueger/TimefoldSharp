using TimefoldSharp.Core.API.Score;

namespace TimefoldSharp.Core.API.Domain.Solution
{
    public interface SolutionCloner
    {
        ISolution CloneSolution(ISolution original);
    }
}
