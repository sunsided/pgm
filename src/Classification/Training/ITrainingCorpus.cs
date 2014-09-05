using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Observations;

namespace widemeadows.MachineLearning.Classification.Training
{
    /// <summary>
    /// Interface ITrainingCorpusRegistration
    /// </summary>
    public interface ITrainingCorpus : ITrainingCorpusAccess
    {
        /// <summary>
        /// Adds the sequence.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <returns>IDocument{`1}.</returns>
        [NotNull]
        IDocument AddSequence([NotNull] IObservationSequence sequence);
    }
}
