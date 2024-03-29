﻿using System;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Scores.Likelihoods;
using widemeadows.MachineLearning.Classification.Scores.Probabilities;

namespace widemeadows.MachineLearning.Classification.Scores.Combiners
{
    /// <summary>
    /// Class EtaEvidenceCombiner.
    /// <para>
    /// Combines evidence in a logarithmic manner.
    /// </para>
    /// </summary>
    public sealed class EtaEvidenceCombiner : IEvidenceCombiner
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="EtaEvidenceCombiner"/> class from being created.
        /// </summary>
        private EtaEvidenceCombiner() { }

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
        /// The eta term
        /// </summary>
        private double _eta;

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            _eta = 0;
        }

        /// <summary>
        /// Aggregates the specified probability.
        /// </summary>
        /// <param name="p">The p.</param>
        public void Combine(IProbability p)
        {
            _eta += Math.Log(1.0D - p.Value) - Math.Log(p.Value);
        }

        /// <summary>
        /// Aggregates the specified probability.
        /// </summary>
        /// <param name="p">The p.</param>
        public void Combine(ILogProbability p)
        {
            var logProbability = p.Value;
            var probability = Math.Exp(p.Value);

            _eta += Math.Log(1.0D - probability) - logProbability;
        }

        /// <summary>
        /// Calculates the combined probability.
        /// </summary>
        /// <returns>IProbability.</returns>
        [NotNull]
        public IProbability Calculate()
        {
            return new Probability(1.0D/(1.0D+Math.Exp(_eta)));
        }

        /// <summary>
        /// Calculates the combined log probability.
        /// </summary>
        /// <returns>IProbability.</returns>
        [NotNull]
        public ILikelihood CalculateLog()
        {
            return new Likelihood(- Math.Log(1.0D + Math.Exp(_eta)));
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
                return new EtaEvidenceCombiner();
            }
        }
    }
}
