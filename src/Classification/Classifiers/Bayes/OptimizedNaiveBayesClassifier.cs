using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Scores;
using widemeadows.MachineLearning.Classification.Scores.Combiners;
using widemeadows.MachineLearning.Classification.Scores.LogProbabilities;
using widemeadows.MachineLearning.Classification.Scores.Probabilities;
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
    public class OptimizedNaiveBayesClassifier : BayesBase, IClassifier<ITargetScoreCollection<ProbabilityL>>
    {
        /// <summary>
        /// The observation probabilities per document
        /// </summary>
        private Dictionary<LabeledObservationKey, JointLogProbabilityOL> _jointLogProbabilitiesPerLabel = new Dictionary<LabeledObservationKey, JointLogProbabilityOL>();

        /// <summary>
        /// The prior resolver
        /// </summary>
        [NotNull]
        protected readonly IPriorProbabilityResolver PriorResolver;

        /// <summary>
        /// The evidence combiner factory
        /// </summary>
        [NotNull]
        protected readonly IEvidenceCombinerFactory EvidenceCombiner;

        /// <summary>
        /// The probability calculator
        /// </summary>
        protected readonly IProbabilityCalculator ProbabilityCalculator;

        /// <summary>
        /// Initializes a new instance of the <see cref="NaiveBayesClassifier" /> class.
        /// </summary>
        /// <param name="priorResolver">The prior probability resolver.</param>
        /// <param name="evidenceCombiner">The evidence combiner.</param>
        public OptimizedNaiveBayesClassifier([NotNull] IPriorProbabilityResolver priorResolver, [NotNull] IEvidenceCombinerFactory evidenceCombiner, [NotNull] IProbabilityCalculator probabilityCalculator)
        {
            PriorResolver = priorResolver;
            EvidenceCombiner = evidenceCombiner;
            ProbabilityCalculator = probabilityCalculator;
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
                        var logProbability = GetConditionalLogProbabilityOfObservationGivenLabel(observation, label, document);
                        observationLogProbabilitiesPerLabel[key] += logProbability.Value;
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
        /// Classifies the specified observations.
        /// </summary>
        /// <param name="observations">The observations.</param>
        /// <returns>IScoreCollection{IScore}.</returns>
        public virtual ITargetScoreCollection<ProbabilityL> Classify(IObservationSequence observations)
        {
            // Given
            // ============
            //
            // c = {c1, c2, ..., cN}    
            // d = {o1, o2, ..., oM}    (document := all observations)

            // Bayes' Theorem
            // ========================
            //
            //            P(d|c1) * P(c1)
            // P(c1|d) = -----------------
            //                P(d)

            // Law of total probability
            // ==========================================
            //
            // P(d) = P(d|c1)*P(c1) + P(d|c2)*P(c2) + ...

            var labelCount = TrainingCorpora.Count;
            if (labelCount == 0) Trace.TraceWarning("The collection of training corpora was empty. Missed a call to Learn()?");

            // prepare the evidence combiners
            var evidenceCombiners = EvidenceCombiner.CreateMany(labelCount);

            // prepare the joint probability array
            var jointLogProbabilities = new JointLogProbabilityOL[labelCount];

            // iterate over each observation o in the document d
            foreach (var observation in observations)
            {
                // calculate the joint probabilities P(o,c) for the observation o and each label c
                for (int c = 0; c < labelCount; ++c)
                {
                    var corpus = TrainingCorpora[c];
                    var label = corpus.Label;
                    jointLogProbabilities[c] = GetJointProbabilityGivenObservation(observation, label, corpus);
                }

                // calculate total probability P(o)
                var totalProbability = Math.Log(jointLogProbabilities.Sum(x => Math.Exp(x.Value))).AsLogProbability(observation);

                // calculate the posterior P(c|o)
                for (int c = 0; c < labelCount; ++c)
                {
                    var jointLogProbability = jointLogProbabilities[c];
                    var logPosterior = GetLogPosteriorGivenJointProbability(jointLogProbability, totalProbability);

                    // combine the evidence
                    evidenceCombiners[c].Combine(logPosterior);
                }
            }

            // prepare the resulting score collection
            var scoreCollection = new MaximizationTargetScoreCollection<ProbabilityL>();
            for (int c = 0; c < labelCount; ++c)
            {
                var label = TrainingCorpora[c].Label;
                var probability = evidenceCombiners[c].Calculate();
                scoreCollection.TryAdd(new ProbabilityL(probability.Value, label));
            }

            return scoreCollection;
        }

        /// <summary>
        /// Calculates the joint probability <c>P(o,c) = P(o|c) * P(c)</c>.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <param name="label">The label.</param>
        /// <param name="documents">The documents.</param>
        /// <returns>ILikelihood.</returns>
        [NotNull]
        protected JointLogProbabilityOL GetJointProbabilityGivenObservation([NotNull] IObservation observation, [NotNull] ILabel label, [NotNull] IEnumerable<ILabeledDocument> documents)
        {
            var lookup = _jointLogProbabilitiesPerLabel;

            // lookup the probability
            var key = new LabeledObservationKey(label, observation);
            JointLogProbabilityOL logProbability;
            if (lookup.TryGetValue(key, out logProbability)) return logProbability;

            // that's a mismatch, so get funky using math
            var po = GetConditionalLogProbabilityOfObservationGivenLabel(observation, label, documents);
            var pc = PriorResolver.GetPriorLogProbability(label);
            return po + pc;
        }
        
        /// <summary>
        /// Gets the conditional probability of observation given label.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <param name="label">The label.</param>
        /// <param name="documents">The sequence of all documents.</param>
        /// <returns>ConditionalProbabilityOL.</returns>
        [NotNull]
        protected virtual ConditionalLogProbabilityOL GetConditionalLogProbabilityOfObservationGivenLabel([NotNull] IObservation observation, [NotNull] ILabel label, [NotNull] IEnumerable<ILabeledDocument> documents)
        {
#if false
            // Option 1: regular, naive combination of probabilities by multiplication. 
            // Note that since all probabilities are significantly smaller than one, this value exponentially tends to it's limit at zero.

            var probability = documents.Mul(document => GetConditionalProbabilityOfObservationGivenLabel(observation, label, document).Value);

#else
            // Option 2: logarithmic combination of probabilities. While being computationally more expensive,
            // this method efficiently counters floating-point underflow due to machine precision.

            // exploits the fact that a*b = log(a)+log(b)
            var logProbability = documents.Sum(document => GetConditionalLogProbabilityOfObservationGivenLabel(observation, label, document).Value);

#endif

            // returnify
            return new ConditionalLogProbabilityOL(logProbability, observation, label);
        }

        /// <summary>
        /// Gets the conditional probability of observation given label.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <param name="label">The label.</param>
        /// <param name="document">The document.</param>
        /// <returns>ConditionalProbabilityOL.</returns>
        [NotNull]
        protected virtual ConditionalLogProbabilityOL GetConditionalLogProbabilityOfObservationGivenLabel([NotNull] IObservation observation, [NotNull] ILabel label, [NotNull] IDocument document)
        {
            var frequency = document.GetFrequency(observation);
            var documentLength = document.Length;

            var p = ProbabilityCalculator.GetProbability(frequency, documentLength, LaplaceSmoothing);
            return new ConditionalLogProbabilityOL(Math.Log(p), observation, label);
        }

        /// <summary>
        /// Gets the posterior <c>P(c|d)</c> given the joint probability <c>P(c,d)</c>.
        /// </summary>
        /// <param name="jointProbability">The joint probability.</param>
        /// <param name="totalProbability">The total probability.</param>
        /// <returns>IProbability.</returns>
        [NotNull, Pure]
        protected static ConditionalLogProbabilityLO GetLogPosteriorGivenJointProbability([NotNull] JointLogProbabilityOL jointProbability, [NotNull] LogProbabilityO totalProbability)
        {
            return jointProbability - totalProbability;
        }
    }
}
