using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

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
            return new EqualDistributionProbability(registry.AsReadOnly());
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
            return new EqualDistributionProbability((IReadOnlyCollection<ILabel>) registry.AsReadOnly());
        }
    }
}
