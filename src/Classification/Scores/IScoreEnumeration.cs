using System.Collections.Generic;

namespace widemeadows.MachineLearning.Classification.Scores
{
    /// <summary>
    /// Interface IScoreCollection
    /// </summary>
    /// <typeparam name="TScore">The type of the score.</typeparam>
    public interface IScoreEnumeration<out TScore> : IEnumerable<TScore>
        where TScore: IScore
    {
    }
}