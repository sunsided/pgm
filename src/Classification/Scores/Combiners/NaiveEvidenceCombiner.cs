using System;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Scores.Likelihoods;
using widemeadows.MachineLearning.Classification.Scores.Probabilities;

namespace widemeadows.MachineLearning.Classification.Scores.Combiners
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
        public void Combine(IProbability p)
        {
            var value = p.Value;

            _term1 *= value;
            _term2 *= (1.0D - value);
        }

        /// <summary>
        /// Aggregates the specified probability.
        /// </summary>
        /// <param name="p">The p.</param>
        public void Combine(ILogProbability p)
        {
            var value = Math.Exp(p.Value);

            _term1 *= value;
            _term2 *= (1.0D - value);
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
        /// Calculates the combined log probability.
        /// </summary>
        /// <returns>IProbability.</returns>
        [NotNull]
        public ILikelihood CalculateLog()
        {
            return new Likelihood(Math.Log(_term1) - Math.Log(_term1 + _term2));
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
