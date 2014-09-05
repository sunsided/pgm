using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Scores.Combiners
{
    /// <summary>
    /// Interface IEvidenceCombinerFactory
    /// </summary>
    public interface IEvidenceCombinerFactory
    {
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>IEvidenceCombiner.</returns>
        [NotNull]
        IEvidenceCombiner Create();
    }
}
