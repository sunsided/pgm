using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Scores
{
    /// <summary>
    /// Interface ITargetScoreCollection
    /// </summary>
    /// <typeparam name="TScore">The type of the t score.</typeparam>
    public interface ITargetScoreCollection<TScore> : IScoreCollection<TScore> 
        where TScore : IScore
    {
        /// <summary>
        /// Gets the best score or <see langword="null"/> if no scores are given.
        /// </summary>
        /// <value>The best score.</value>
        /// <seealso cref="Minimum"/>
        /// <seealso cref="Maximum"/>
        [CanBeNull]
        TScore BestScore { get; }

        /// <summary>
        /// Gets the minimum value or <see langword="null"/> if the collection is empty.
        /// </summary>
        /// <value>The minimum value or <see langword="null"/> if the collection is empty.</value>
        [CanBeNull]
        TScore Minimum { get; }

        /// <summary>
        /// Gets the maximum value or <see langword="null"/> if the collection is empty.
        /// </summary>
        /// <value>The maximum value or <see langword="null"/> if the collection is empty.</value>
        [CanBeNull]
        TScore Maximum { get; }
    }
}