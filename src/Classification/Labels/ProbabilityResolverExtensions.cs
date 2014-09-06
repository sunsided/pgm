using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels.Priors;
using widemeadows.MachineLearning.Classification.Training;

namespace widemeadows.MachineLearning.Classification.Labels
{
    /// <summary>
    /// Class ProbabilityResolverExtensions.
    /// </summary>
    public static class ProbabilityResolverExtensions
    {
        /// <summary>
        /// Gets an <see cref="EqualDistributionProbability"/> resolver for the given labels.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <returns>EqualDistributionProbability.</returns>
        [NotNull]
        public static EqualDistributionProbability GetEqualDistribution([NotNull] this LabelRegistry registry)
        {
            return new EqualDistributionProbability(registry);
        }

        /// <summary>
        /// Gets an <see cref="EqualDistributionProbability"/> resolver for the given labels.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <returns>EqualDistributionProbability.</returns>
        [NotNull]
        public static EqualDistributionProbability GetEqualDistribution<T>([NotNull] this LabelRegistry<T> registry)
            where T: ILabel
        {
            return new EqualDistributionProbability(registry);
        }

        /// <summary>
        /// Gets an <see cref="EqualDistributionProbability"/> resolver for the given labels.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <returns>EqualDistributionProbability.</returns>
        [NotNull]
        public static DistributionFromVocabularySize GetDistributionFromVocabularySize([NotNull] this IDictionary registry)
        {
            return new DistributionFromVocabularySize(registry);
        }

        /// <summary>
        /// Gets an <see cref="EqualDistributionProbability"/> resolver for the given labels.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <returns>EqualDistributionProbability.</returns>
        [NotNull]
        public static DistributionFromVocabularySize GetDistributionFromVocabularySize<T>([NotNull] this IDictionary registry)
            where T : ILabel
        {
            return new DistributionFromVocabularySize(registry);
        }
    }
}
