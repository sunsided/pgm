using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Scores;
using widemeadows.MachineLearning.Classification.Scores.Probabilities;
using widemeadows.MachineLearning.Classification.Training;

namespace widemeadows.MachineLearning.Classification.Classifiers.Bayes
{
    /// <summary>
    /// Class NaiveBayesClassifier. This class cannot be inherited.
    /// </summary>
    public sealed class NaiveBayesClassifier : BayesBase, IClassifier<ITargetScoreCollection<ProbabilityL>>
    {
        /// <summary>
        /// The training corpora
        /// </summary>
        [NotNull]
        private readonly ReadOnlyCollection<ITrainingCorpus> _trainingCorpora;

        /// <summary>
        /// The prior resolver
        /// </summary>
        [NotNull]
        private readonly IProbabilityResolver _priorResolver;

        /// <summary>
        /// The evidence combiner factory
        /// </summary>
        [NotNull]
        private readonly IEvidenceCombinerFactory _evidenceCombiner;

        /// <summary>
        /// Initializes a new instance of the <see cref="NaiveBayesClassifier" /> class.
        /// </summary>
        /// <param name="trainingCorpora">The training corpora.</param>
        /// <param name="priorResolver">The prior probability resolver.</param>
        /// <param name="evidenceCombiner">The evidence combiner.</param>
        public NaiveBayesClassifier([NotNull] ReadOnlyCollection<ITrainingCorpus> trainingCorpora, [NotNull] IProbabilityResolver priorResolver, [NotNull] IEvidenceCombinerFactory evidenceCombiner)
        {
            _trainingCorpora = trainingCorpora;
            _priorResolver = priorResolver;
            _evidenceCombiner = evidenceCombiner;
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

            var labelCount = _trainingCorpora.Count;

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
                    var corpus = _trainingCorpora[c];
                    var label = corpus.Label;
                    jointProbabilities[c] = GetJointProbabilityGivenObservation(observation, label, corpus);
                }

                // calculate total probability P(o)
                var totalProbability = jointProbabilities.Sum(x => x.Value).AsProbability(observation);

                // calculate the probabilities
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
                var label = _trainingCorpora[c].Label;
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
            var product = 1.0D;
            foreach (var document in documents)
            {
                product *= GetConditionalProbabilityOfObservationGivenLabel(observation, label, document).Value;
            }
            return new ConditionalProbabilityOL(product, observation, label);
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
        /// <param name="label">The label.</param>
        /// <param name="observation">The observation.</param>
        /// <returns>ILikelihood.</returns>
        [NotNull]
        private JointProbabilityOL GetJointProbabilityGivenObservation([NotNull] IObservation observation, [NotNull] ILabel label, [NotNull] IEnumerable<ILabeledDocument> documents)
        {
            var po = GetConditionalProbabilityOfObservationGivenLabel(observation, label, documents);
            var pc = _priorResolver.GetProbability(label);
            return po*pc;
        }

        /// <summary>
        /// Gets the posterior <c>P(c|d)</c> given the joint probability <c>P(c,d)</c>.
        /// </summary>
        /// <param name="jointProbability">The joint probability.</param>
        /// <param name="totalProbability">The total probability.</param>
        /// <returns>IProbability.</returns>
        [NotNull]
        private ConditionalProbabilityLO GetPosteriorGivenJointProbability([NotNull] JointProbabilityOL jointProbability, [NotNull] ProbabilityO totalProbability)
        {
            return jointProbability/totalProbability;
        }
    }
}
