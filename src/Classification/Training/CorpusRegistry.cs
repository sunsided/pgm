using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;

namespace widemeadows.MachineLearning.Classification.Training
{
    /// <summary>
    /// Class CorpusRegistry. This class cannot be inherited.
    /// </summary>
    public sealed class CorpusRegistry : Registry<ITrainingCorpusAccess>
    {
        /// <summary>
        /// Adds and registers a new corpus.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns>TrainingCorpus.</returns>
        [NotNull]
        public ITrainingCorpus Add([NotNull] ILabel label)
        {
            var corpus = new TrainingCorpus(label);
            return Add(corpus);
        }
    }
}
