using System.Collections.Generic;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Scores.Probabilities
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

        /// <summary>
        /// Creates many combiners.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>IList{IEvidenceCombiner}.</returns>
        [NotNull]
        IList<IEvidenceCombiner> CreateMany(int count);
    }
}
