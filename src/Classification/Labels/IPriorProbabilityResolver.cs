using JetBrains.Annotations;
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
    }
}
