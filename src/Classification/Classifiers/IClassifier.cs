using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Scores;

namespace widemeadows.MachineLearning.Classification.Classifiers
{
    /// <summary>
    /// Interface IClassifier
    /// </summary>
    public interface IClassifier<out TScoreCollection>
        where TScoreCollection: IScoreCollection
    {
        /// <summary>
        /// Classifies the specified observations.
        /// </summary>
        /// <param name="observations">The observations.</param>
        /// <returns>IScoreCollection{`0}.</returns>
        [NotNull]
        TScoreCollection Classify([NotNull] IObservationSequence observations);
    }
}
