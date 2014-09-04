using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Scores;

namespace widemeadows.MachineLearning.Classification.Classifiers
{
    /// <summary>
    /// Interface IClassifier
    /// </summary>
    public interface IClassifier
    {
        /// <summary>
        /// Classifies the specified observations.
        /// </summary>
        /// <param name="observations">The observations.</param>
        /// <returns>IScoreCollection{`0}.</returns>
        [NotNull]
        IScoreCollection Classify([NotNull] IObservationSequence observations);
    }
}
