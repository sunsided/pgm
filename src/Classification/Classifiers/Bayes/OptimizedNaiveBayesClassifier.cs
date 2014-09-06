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
        private Dictionary<LabeledObservationKey, ConditionalLogProbabilityOL> _conditionalLogProbabilitiesPerLabel = new Dictionary<LabeledObservationKey, ConditionalLogProbabilityOL>();

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

            // the (summed) probabilities log P(o|l) of an observation given the label
            var conditionalLogProbabilitiesPerLabel = new Dictionary<LabeledObservationKey, ConditionalLogProbabilityOL>(capacity: classCount * observationCount);

            // iterate over all possible classes
            foreach (var corpus in trainingCorpora)
            {
                var label = corpus.Label;
                double weighting = Math.Log(corpus.DocumentCount);

                // iterate over all observations
                foreach (var observation in allObservations)
                {
                    // initialize the sum to zero
                    var observationProbabilityGivenLabel = 0.0D;

                    // calculate the probability P(o|l) of the observation by adding each P(o|l,d)
                    foreach (var document in corpus)
                    {
                        var logProbability = GetConditionalLogProbabilityOfObservationGivenLabel(observation, label, document);
                        observationProbabilityGivenLabel += Math.Exp(logProbability.Value);
                    }

                    // scale by number of documents
                    var observationLogProbabilityGivenLabel = (Math.Log(observationProbabilityGivenLabel) - weighting).AsLogProbability(observation, given: label);

                    // store the conditional probability
                    var key = new LabeledObservationKey(label, observation);
                    conditionalLogProbabilitiesPerLabel.Add(key, observationLogProbabilityGivenLabel);
                }
            }

            // swap references
            _conditionalLogProbabilitiesPerLabel = conditionalLogProbabilitiesPerLabel;
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
            var conditionalLogProbabilities = new ConditionalLogProbabilityOL[labelCount];

            // iterate over each observation o in the document d
            foreach (var observation in observations)
            {
                // calculate the joint probabilities P(o,c) for the observation o and each label c
                for (int c = 0; c < labelCount; ++c)
                {
                    var corpus = TrainingCorpora[c];
                    var label = corpus.Label;
                    conditionalLogProbabilities[c] = GetConditionalProbabilityGivenLabel(observation, label, corpus);
                }

                // TODO: Apply text-length normalization

                // calculate total log probability log P(o)
                // sadly this isn't the most performant operation in log domain due to the exponential function
                // var totalLogProbability = Math.Log(jointLogProbabilities.Sum(x => Math.Exp(x.Value))).AsLogProbability(observation);
                double totalProbability = 0.0D;
                for (int c = 0; c < labelCount; ++c)
                {
                    // get the prior probability
                    var prior = GetLabelPriorLogProbability(c);

                    // sum over all P(o|l)*P(l)
                    var jointLogProbability = conditionalLogProbabilities[c] + prior;
                    totalProbability += Math.Exp(jointLogProbability.Value);
                }

                // fetch the log probability
                var totalLogProbability = Math.Log(totalProbability).AsLogProbability(observation);

                // calculate the posterior P(c|o)
                for (int c = 0; c < labelCount; ++c)
                {
                    // get the prior probability
                    var prior = GetLabelPriorLogProbability(c);

                    var conditionalLogProbability = conditionalLogProbabilities[c];
                    var jointLogProbability = conditionalLogProbability + prior;

                    var logPosterior = GetLogPosteriorGivenJointProbability(jointLogProbability, totalLogProbability);

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
        /// Gets the label's prior log probability.
        /// </summary>
        /// <param name="c">The label index.</param>
        /// <returns>LogProbabilityL.</returns>
        [NotNull]
        private LogProbabilityL GetLabelPriorLogProbability(int c)
        {
            var corpus = TrainingCorpora[c];
            var label = corpus.Label;
            var prior = PriorResolver.GetPriorLogProbability(label);
            return prior;
        }

        /// <summary>
        /// Calculates the joint probability <c>P(o,c) = P(o|c) * P(c)</c>.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <param name="label">The label.</param>
        /// <param name="documents">The documents.</param>
        /// <returns>ILikelihood.</returns>
        [NotNull]
        protected ConditionalLogProbabilityOL GetConditionalProbabilityGivenLabel([NotNull] IObservation observation, [NotNull] ILabel label, [NotNull] IEnumerable<ILabeledDocument> documents)
        {
            var lookup = _conditionalLogProbabilitiesPerLabel;

            // lookup the probability
            var key = new LabeledObservationKey(label, observation);
            ConditionalLogProbabilityOL logProbability;
            if (lookup.TryGetValue(key, out logProbability)) return logProbability;

            // that's a mismatch, so get funky using math
            return GetConditionalLogProbabilityOfObservationGivenLabel(observation, label, documents);
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
