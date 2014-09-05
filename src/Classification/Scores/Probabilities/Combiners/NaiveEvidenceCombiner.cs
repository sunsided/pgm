using System;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Scores.Probabilities.Combiners
{
    /// <summary>
    /// Class EvidenceCombiner.
    /// </summary>
    public sealed class NaiveEvidenceCombiner : IEvidenceCombiner
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="NaiveEvidenceCombiner"/> class from being created.
        /// </summary>
        private NaiveEvidenceCombiner() {}

        /// <summary>
        /// The default factory, lazy-evaluated
        /// </summary>
        private static readonly Lazy<IEvidenceCombinerFactory> DefaultFactoryLazy = new Lazy<IEvidenceCombinerFactory>(() => new InternalFactory());

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>The default.</value>
        [NotNull]
        public static IEvidenceCombinerFactory Factory { get { return DefaultFactoryLazy.Value; } }

        /// <summary>
        /// The 1st term, used for numerator and denominator
        /// </summary>
        private double _term1;

        /// <summary>
        /// The 2nd term, used for the denominator
        /// </summary>
        private double _term2;

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            _term1 = _term2 = 0;
        }

        /// <summary>
        /// Aggregates the specified probability.
        /// </summary>
        /// <param name="p">The p.</param>
        public void Combine([NotNull] IProbability p)
        {
            _term1 += p.Value;
            _term2 += (1.0D - p.Value);
        }

        /// <summary>
        /// Calculates the combined probability.
        /// </summary>
        /// <returns>IProbability.</returns>
        [NotNull]
        public IProbability Calculate()
        {
            return new Probability(_term1/(_term1 + _term2));
        }

        /// <summary>
        /// Class Factory. This class cannot be inherited.
        /// </summary>
        private sealed class InternalFactory : IEvidenceCombinerFactory
        {
            /// <summary>
            /// Creates this instance.
            /// </summary>
            /// <returns>IEvidenceCombiner.</returns>
            public IEvidenceCombiner Create()
            {
                return new NaiveEvidenceCombiner();
            }
        }
    }
}
