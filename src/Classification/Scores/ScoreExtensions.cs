using System;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Scores.Probabilities;

namespace widemeadows.MachineLearning.Classification.Scores
{
    public static class ScoreExtensions
    {
        /// <summary>
        /// Gets the <paramref name="value"/> as a <see cref="Probability"/>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Probability.</returns>
        [NotNull]
        public static Probability AsProbability(this double value)
        {
            return new Probability(value);
        }

        /// <summary>
        /// Gets the <paramref name="value" /> as a <see cref="Probability" />
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="observation">The observation.</param>
        /// <returns>Probability.</returns>
        [NotNull]
        public static ProbabilityO AsProbability(this double value, [NotNull] IObservation observation)
        {
            return new ProbabilityO(value, observation);
        }

        /// <summary>
        /// Gets the <paramref name="value" /> as a <see cref="Probability" />
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="label">The label.</param>
        /// <returns>Probability.</returns>
        [NotNull]
        public static ProbabilityL AsProbabilityL(this double value, [NotNull] ILabel label)
        {
            return new ProbabilityL(value, label);
        }

        /// <summary>
        /// Gets the <paramref name="value"/> as a <see cref="Likelihood"/>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Likelihood.</returns>
        [NotNull]
        public static Likelihood AsLikelihood(this double value)
        {
            return new Likelihood(value);
        }
    }
}
