using System;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Scores;

namespace widemeadows.MachineLearning.Classification.Classifiers.Bayes
{
    /// <summary>
    /// Class NaiveBayesClassifier. This class cannot be inherited.
    /// </summary>
    public sealed class NaiveBayesClassifier : IClassifier
    {
        /// <summary>
        /// Classifies the specified observations.
        /// </summary>
        /// <param name="observations">The observations.</param>
        /// <returns>IScoreCollection{IScore}.</returns>
        public IScoreCollection Classify(IObservationSequence observations)
        {
            throw new NotImplementedException();
        }
    }
}
