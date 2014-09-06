using System.Diagnostics;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Scores.LogProbabilities;
using widemeadows.MachineLearning.Classification.Scores.Probabilities;
using widemeadows.MachineLearning.Classification.Training;

namespace widemeadows.MachineLearning.Classification.Labels.Priors
{
    /// <summary>
    /// Class EqualDistributionProbabilityResolver.
    /// </summary>
    public sealed class DistributionFromVocabularySize : IPriorProbabilityResolver
    {
        /// <summary>
        /// The labels
        /// </summary>
        [NotNull]
        private readonly IDictionary _labels;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualDistributionProbability"/> class.
        /// </summary>
        /// <param name="labels">The labels.</param>
        public DistributionFromVocabularySize([NotNull] IDictionary labels)
        {
            _labels = labels;
        }

        /// <summary>
        /// Gets the a priori probability for the given label.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns>System.Double.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ProbabilityL GetPriorProbability(ILabel label)
        {
            Debug.Assert(_labels.Count > 0, "Expected the number of labels to be greater than zero.");

            var set = _labels[label];
            var vocabularySize = _labels.VocabularySize;
            var setSize = set.SetVocabularySize;

            return new ProbabilityL((double)setSize/vocabularySize, label);
        }

        /// <summary>
        /// Gets the a priori log-probability for the given label.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns>System.Double.</returns>
        public LogProbabilityL GetPriorLogProbability(ILabel label)
        {
            return GetPriorProbability(label).ToLogProbability();
        }
    }
}
