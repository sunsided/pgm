using System.Collections.Generic;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Scores.Probabilities.Combiners
{
    /// <summary>
    /// Class EvidenceCombinerExtensions.
    /// </summary>
    public static class EvidenceCombinerExtensions
    {
        /// <summary>
        /// Creates many combiners.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="count">The count.</param>
        /// <returns>IList{IEvidenceCombiner}.</returns>
        [NotNull]
        public static IList<IEvidenceCombiner> CreateMany([NotNull] this IEvidenceCombinerFactory factory, int count)
        {
            var array = new IEvidenceCombiner[count];
            for (int i = 0; i < count; ++i)
            {
                array[i] = factory.Create();
            }
            return array;
        }
    }
}
