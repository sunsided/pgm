using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Observations;

namespace widemeadows.MachineLearning.Classification.Training
{
    /// <summary>
    /// Interface IDistinctObservationRegistry
    /// </summary>
    public interface IDistinctObservationRegistry
    {
        /// <summary>
        /// Registers an observation.
        /// </summary>
        /// <param name="observation">The observation.</param>
        void RegisterDistinctObservation([NotNull] IObservation observation);
    }
}
