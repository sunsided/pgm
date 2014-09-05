using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Scores.Probabilities
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
        /// Calculates the combined probability.
        /// </summary>
        /// <returns>IProbability.</returns>
        [NotNull]
        IProbability Calculate();
    }
}