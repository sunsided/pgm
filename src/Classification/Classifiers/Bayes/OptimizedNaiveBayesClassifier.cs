﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Scores;
using widemeadows.MachineLearning.Classification.Scores.Probabilities;
using widemeadows.MachineLearning.Classification.Scores.Probabilities.Combiners;
using widemeadows.MachineLearning.Classification.Training;

namespace widemeadows.MachineLearning.Classification.Classifiers.Bayes
{
    /// <summary>
    /// Class NaiveBayesClassifier. This class cannot be inherited.
    /// <para>
    /// This class implements an optimized Naive Bayes classifier that
    /// estimates the posterior probability P(c|d) of a class given a document.
    /// </para>
    /// <para>
    /// This code implementation label probabilities given each class.
    /// </para>
    /// </summary>
    public class OptimizedNaiveBayesClassifier : NaiveBayesClassifier
    {
        /// <summary>
        /// The observation probabilities per document
        /// </summary>
        private Dictionary<LabeledObservationKey, JointLogProbabilityOL> _jointLogProbabilitiesPerLabel = new Dictionary<LabeledObservationKey, JointLogProbabilityOL>();

        /// <summary>
        /// Initializes a new instance of the <see cref="NaiveBayesClassifier" /> class.
        /// </summary>
        /// <param name="priorResolver">The prior probability resolver.</param>
        /// <param name="evidenceCombiner">The evidence combiner.</param>
        public OptimizedNaiveBayesClassifier([NotNull] IPriorProbabilityResolver priorResolver, [NotNull] IEvidenceCombinerFactory evidenceCombiner, [NotNull] IProbabilityCalculator probabilityCalculator)
            : base(priorResolver, evidenceCombiner, probabilityCalculator)
        {
        }

        /// <summary>
        /// Learns the posterior probabilities from specified training corpora.
        /// <para>
        /// This method is called internally by <see cref="BayesBase.Learn" />.
        /// </para>
        /// </summary>
        /// <param name="trainingCorpora">The training corpora.</param>
        protected override void LearnInternal(IDictionary trainingCorpora)
        {
            // determine all observations from the corpora
            var allObservations = DetermineObservations(trainingCorpora);
            var observationCount = allObservations.Count;
            var classCount = trainingCorpora.Count;

            // the (summed) logarithmic probabilities log P(o|l) of an observation given the label
            var observationLogProbabilitiesPerLabel = new Dictionary<LabeledObservationKey, ConditionalLogProbabilityOL>(capacity: observationCount);

            // the (summed) probabilities log P(o|l) of an observation given the label
            var jointLogProbabilitiesPerLabel = new Dictionary<LabeledObservationKey, JointLogProbabilityOL>(capacity: classCount * observationCount);

            // iterate over all possible classes
            foreach (var corpus in trainingCorpora)
            {
                var label = corpus.Label;

                // start with a clean log-P dictionary
                observationLogProbabilitiesPerLabel.Clear();

                // iterate over all observations
                foreach (var observation in allObservations)
                {
                    // get the key and initialize the sum to zero
                    var key = new LabeledObservationKey(label, observation);
                    observationLogProbabilitiesPerLabel.Add(key, new ConditionalLogProbabilityOL(0, observation, label));

                    // calculate the probability P(o|l) of the observation by multiplying each P(o|l,d)
                    // this is done by summing over each log P(o|l,d)
                    foreach (var document in corpus)
                    {
                        var probability = GetConditionalProbabilityOfObservationGivenLabel(observation, label, document);
                        observationLogProbabilitiesPerLabel[key] += Math.Log(probability.Value);
                    }
                }

                // get the label's prior probability
                var prior = PriorResolver.GetPriorLogProbability(label);

                // iterate over each observation again and calculate
                // P(o|l) = e^(log P(o|l))
                foreach (var pair in observationLogProbabilitiesPerLabel)
                {
                    var key = pair.Key;
                    var logProbability = pair.Value;
                    
                    // multiply with the label's a priori probability
                    var jointLogProbability = logProbability + prior;

                    // add the probability
                    jointLogProbabilitiesPerLabel.Add(key, jointLogProbability);
                }
            }

            // swap references
            _jointLogProbabilitiesPerLabel = jointLogProbabilitiesPerLabel;
        }
        
        /// <summary>
        /// Determines all observations in all documents.
        /// </summary>
        /// <param name="trainingCorpora">The training corpora.</param>
        /// <returns>HashSet&lt;IObservation&gt;.</returns>
        [NotNull]
        private static HashSet<IObservation> DetermineObservations([NotNull] IDictionary trainingCorpora)
        {
            return new HashSet<IObservation>(trainingCorpora.GetDistinctObservations().Select(pair => pair.Key));
        }

        /// <summary>
        /// Calculates the joint probability <c>P(o,c) = P(o|c) * P(c)</c>.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <param name="label">The label.</param>
        /// <param name="documents">The documents.</param>
        /// <returns>ILikelihood.</returns>
        protected override JointProbabilityOL GetJointProbabilityGivenObservation(IObservation observation, ILabel label, IEnumerable<ILabeledDocument> documents)
        {
            var lookup = _jointLogProbabilitiesPerLabel;

            // lookup the probability
            var key = new LabeledObservationKey(label, observation);
            JointLogProbabilityOL logProbability;
            if (lookup.TryGetValue(key, out logProbability)) return logProbability.ToJointProbability();

            // that's a mismatch, so get funky using math
            var probability = GetJointProbabilityGivenObservationUncached(observation, label, documents);
            return probability;
        }

        /// <summary>
        /// Calculates the joint probability <c>P(o,c) = P(o|c) * P(c)</c>.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <param name="label">The label.</param>
        /// <param name="documents">The documents.</param>
        /// <returns>ILikelihood.</returns>
        [NotNull]
        protected JointProbabilityOL GetJointProbabilityGivenObservationUncached([NotNull] IObservation observation, [NotNull] ILabel label, [NotNull] IEnumerable<ILabeledDocument> documents)
        {
            return base.GetJointProbabilityGivenObservation(observation, label, documents);
        }
    }
}
