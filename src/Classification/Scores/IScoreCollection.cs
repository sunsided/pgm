using System.Collections.Concurrent;

namespace widemeadows.MachineLearning.Classification.Scores
{
    /// <summary>
    /// Interface IScoreCollection
    /// </summary>
    /// <typeparam name="TScore">The type of the t score.</typeparam>
    public interface IScoreCollection<TScore> : IProducerConsumerCollection<TScore>, IScoreEnumeration<TScore>, IScoreCollection
        where TScore: IScore
    {
    }

    /// <summary>
    /// Interface IScoreCollection
    /// </summary>
    public interface IScoreCollection
    {
    }
}