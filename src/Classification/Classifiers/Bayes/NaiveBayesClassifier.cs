using System.Collections.Generic;
using System.Diagnostics;
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
    /// This class implements a trivial (i.e. unoptimized) Naive Bayes classifier that
    /// estimates the posteiror probability P(c|d) of a class given a document.
    /// </para>
    /// <para>
    /// This code lacks speed and calculation optimizations in order to efficiently
    /// show the algorithm. A 
    /// </para>
    /// </summary>
    public class NaiveBayesClassifier : BayesBase, IClassifier<ITargetScoreCollection<ProbabilityL>>
    {
        /// <summary>
        /// The prior resolver
        /// </summary>
        [NotNull]
        private readonly IPriorProbabilityResolver _priorResolver;

        /// <summary>
        /// The evidence combiner factory
        /// </summary>
        [NotNull]
        private readonly IEvidenceCombinerFactory _evidenceCombiner;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NaiveBayesClassifier" /> class.
        /// </summary>
        /// <param name="priorResolver">The prior probability resolver.</param>
        /// <param name="evidenceCombiner">The evidence combiner.</param>
        public NaiveBayesClassifier([NotNull] IPriorProbabilityResolver priorResolver, [NotNull] IEvidenceCombinerFactory evidenceCombiner)
        {
            _priorResolver = priorResolver;
            _evidenceCombiner = evidenceCombiner;
        }

        /// <summary>
        /// Learns the posterior probabilities from specified training corpora.
        /// <para>
        /// This method is called internally by <see cref="BayesBase.Learn" />.
        /// </para>
        /// </summary>
        /// <param name="trainingCorpora">The training corpora.</param>
        protected override void LearnInternal(IIndexedCollectionAccess<ITrainingCorpusAccess> trainingCorpora)
        {
            // this would be a good place to prepare a probability distribution
            // table, lookups, etc.
            // for the sake of simplicity of the algorithm below, this method is
            // intentionally left empty here.
        }

        /// <summary>
        /// Classifies the specified observations.
        /// </summary>
        /// <param name="observations">The observations.</param>
        /// <returns>IScoreCollection{IScore}.</returns>
        public ITargetScoreCollection<ProbabilityL> Classify(IObservationSequence observations)
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
            var evidenceCombiners = _evidenceCombiner.CreateMany(labelCount);

            // prepare the joint probability array
            var jointProbabilities = new JointProbabilityOL[labelCount];

            // iterate over each observation o in the document d
            foreach (var observation in observations)
            {
                // calculate the joint probabilities P(o,c) for the observation o and each label c
                for (int c = 0; c < labelCount; ++c)
                {
                    var corpus = TrainingCorpora[c];
                    var label = corpus.Label;
                    jointProbabilities[c] = GetJointProbabilityGivenObservation(observation, label, corpus);
                }

                // calculate total probability P(o)
                var totalProbability = jointProbabilities.Sum(x => x.Value).AsProbability(observation);

                // calculate the posterior P(c|o)
                for (int c = 0; c < labelCount; ++c)
                {
                    var jointProbability = jointProbabilities[c];
                    var probability = GetPosteriorGivenJointProbability(jointProbability, totalProbability);

                    // combine the evidence
                    evidenceCombiners[c].Combine(probability);
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
        /// Gets the conditional probability of observation given label.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <param name="label">The label.</param>
        /// <param name="documents">The sequence of all documents.</param>
        /// <returns>ConditionalProbabilityOL.</returns>
        [NotNull]
        private ConditionalProbabilityOL GetConditionalProbabilityOfObservationGivenLabel([NotNull] IObservation observation, [NotNull] ILabel label, [NotNull] IEnumerable<ILabeledDocument> documents)
        {
#if false
            // Option 1: regular, naive combination of probabilities by multiplication. 
            // Note that since all probabilities are significantly smaller than one, this value exponentially tends to it's limit at zero.

            var probability = documents.Mul(document => GetConditionalProbabilityOfObservationGivenLabel(observation, label, document).Value);

#else
            // Option 2: logarithmic combination of probabilities. While being computationally more expensive,
            // this method efficiently counters floating-point underflow due to machine precision.

            // exploits the fact that a*b = log(a)+log(b)
            var probability = documents.LogMul(document => GetConditionalProbabilityOfObservationGivenLabel(observation, label, document).Value);

#endif

            // returnify
            return new ConditionalProbabilityOL(probability, observation, label);
        }

        /// <summary>
        /// Gets the conditional probability of observation given label.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <param name="label">The label.</param>
        /// <param name="document">The document.</param>
        /// <returns>ConditionalProbabilityOL.</returns>
        [NotNull]
        private ConditionalProbabilityOL GetConditionalProbabilityOfObservationGivenLabel([NotNull] IObservation observation, [NotNull] ILabel label, [NotNull] IDocument document)
        {
            var p = document.GetProbability(observation, LaplaceSmoothing);
            return new ConditionalProbabilityOL(p, observation, label);
        }

        /// <summary>
        /// Calculates the joint probability <c>P(o,c) = P(o|c) * P(c)</c>.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <param name="label">The label.</param>
        /// <param name="documents">The documents.</param>
        /// <returns>ILikelihood.</returns>
        [NotNull]
        private JointProbabilityOL GetJointProbabilityGivenObservation([NotNull] IObservation observation, [NotNull] ILabel label, [NotNull] IEnumerable<ILabeledDocument> documents)
        {
            var po = GetConditionalProbabilityOfObservationGivenLabel(observation, label, documents);
            var pc = _priorResolver.GetPriorProbability(label);
            return po*pc;
        }

        /// <summary>
        /// Gets the posterior <c>P(c|d)</c> given the joint probability <c>P(c,d)</c>.
        /// </summary>
        /// <param name="jointProbability">The joint probability.</param>
        /// <param name="totalProbability">The total probability.</param>
        /// <returns>IProbability.</returns>
        [NotNull, Pure]
        private static ConditionalProbabilityLO GetPosteriorGivenJointProbability([NotNull] JointProbabilityOL jointProbability, [NotNull] ProbabilityO totalProbability)
        {
            return jointProbability/totalProbability;
        }
    }
}
