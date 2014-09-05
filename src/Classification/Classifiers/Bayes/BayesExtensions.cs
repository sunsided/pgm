using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Training;

namespace widemeadows.MachineLearning.Classification.Classifiers.Bayes
{
    /// <summary>
    /// Class BayesExtensions.
    /// </summary>
    public static class BayesExtensions
    {
        /// <summary>
        /// Trains a classifier given the corpora and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="classifier">The classifier.</param>
        /// <param name="trainingCorpora">The training corpora.</param>
        /// <returns>The classifier.</returns>
        [NotNull]
        public static T TrainedWith<T>([NotNull] this T classifier, [NotNull] IDictionary trainingCorpora)
            where T : BayesBase
        {
            classifier.Learn(trainingCorpora);
            return classifier;
        }
    }
}
