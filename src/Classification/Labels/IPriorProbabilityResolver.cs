using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Scores.LogProbabilities;
using widemeadows.MachineLearning.Classification.Scores.Probabilities;

namespace widemeadows.MachineLearning.Classification.Labels
{
    /// <summary>
    /// Interface IPriorProbabilityResolver
    /// </summary>
    public interface IPriorProbabilityResolver
    {
        /// <summary>
        /// Gets the a priori probability for the given label.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns>System.Double.</returns>
        [NotNull]
        ProbabilityL GetPriorProbability([NotNull] ILabel label);

        /// <summary>
        /// Gets the a priori log-probability for the given label.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns>System.Double.</returns>
        [NotNull]
        LogProbabilityL GetPriorLogProbability([NotNull] ILabel label);
    }
}
