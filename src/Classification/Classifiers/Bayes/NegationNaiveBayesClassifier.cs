using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Scores;
using widemeadows.MachineLearning.Classification.Scores.Combiners;
using widemeadows.MachineLearning.Classification.Scores.Likelihoods;
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
    public sealed class NegationNaiveBayesClassifier : OptimizedNaiveBayesClassifierBase, IClassifier<ITargetScoreCollection<LogLikelihoodL>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NaiveBayesClassifier" /> class.
        /// </summary>
        /// <param name="priorResolver">The prior probability resolver.</param>
        /// <param name="evidenceCombiner">The evidence combiner.</param>
        public NegationNaiveBayesClassifier([NotNull] IPriorProbabilityResolver priorResolver, [NotNull] IEvidenceCombinerFactory evidenceCombiner, [NotNull] IProbabilityCalculator probabilityCalculator)
            : base(priorResolver, evidenceCombiner, probabilityCalculator)
        {
        }
        
        /// <summary>
        /// Classifies the specified observations.
        /// </summary>
        /// <param name="observations">The observations.</param>
        /// <returns>IScoreCollection{IScore}.</returns>
        public ITargetScoreCollection<LogLikelihoodL> Classify(IObservationSequence observations)
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

                // http://www.aclweb.org/anthology/R11-1083

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
                    // combine the probabilities from all labels except c
                    for (int j = 0; j < labelCount; ++j)
                    {
                        if (j == c) continue;

                        // combine the evidence
                        var conditionalLogProbability = conditionalLogProbabilities[j] + GetLabelPriorLogProbability(j) - totalLogProbability;
                        evidenceCombiners[c].Combine(conditionalLogProbability);
                    }
                }
            }

            // prepare the resulting score collection
            var scoreCollection = new MinimizationTargetScoreCollection<LogLikelihoodL>();
            for (int c = 0; c < labelCount; ++c)
            {
                var label = TrainingCorpora[c].Label;
                var likelihood = evidenceCombiners[c].CalculateLog();
                scoreCollection.TryAdd(new LogLikelihoodL(likelihood.Value, label));
            }

            return scoreCollection;
        }

    }
}
