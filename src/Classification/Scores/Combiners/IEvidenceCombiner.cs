using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Scores.Combiners
{
    /// <summary>
    /// Interface IEvidenceCombiner
    /// </summary>
    public interface IEvidenceCombiner
    {
        /// <summary>
        /// Resets this instance.
        /// </summary>
        void Reset();

        /// <summary>
        /// Aggregates the specified probability.
        /// </summary>
        /// <param name="p">The p.</param>
        void Combine([NotNull] IProbability p);

        /// <summary>
        /// Aggregates the specified log probability.
        /// </summary>
        /// <param name="p">The p.</param>
        void Combine([NotNull] ILogProbability p);

        /// <summary>
        /// Calculates the combined probability.
        /// </summary>
        /// <returns>IProbability.</returns>
        [NotNull]
        IProbability Calculate();
    }
}