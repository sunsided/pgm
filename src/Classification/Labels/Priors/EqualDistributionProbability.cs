﻿using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Scores.LogProbabilities;
using widemeadows.MachineLearning.Classification.Scores.Probabilities;

namespace widemeadows.MachineLearning.Classification.Labels.Priors
{
    /// <summary>
    /// Class EqualDistributionProbabilityResolver.
    /// </summary>
    public sealed class EqualDistributionProbability : IPriorProbabilityResolver
    {
        /// <summary>
        /// The labels
        /// </summary>
        [NotNull]
        private readonly IIndexedCollectionAccess<ILabel> _labels;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualDistributionProbability"/> class.
        /// </summary>
        /// <param name="labels">The labels.</param>
        public EqualDistributionProbability([NotNull] IIndexedCollectionAccess<ILabel> labels)
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
            return new ProbabilityL(1.0D/_labels.Count, label);
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
