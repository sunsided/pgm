using System;
using System.Diagnostics;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Scores.Probabilities;

namespace widemeadows.MachineLearning.Classification.Scores.LogProbabilities
{
    /// <summary>
    /// Class LogProbabilityL.
    /// <para>
    /// Describes the log probability <c>log P(l)</c> of the label.
    /// </para>
    /// </summary>
    [DebuggerDisplay("log P({Label})={Value}")]
    public sealed class LogProbabilityL : LogProbability
    {
        /// <summary>
        /// Gets or sets the  label.
        /// </summary>
        /// <value>The label.</value>
        [NotNull]
        public ILabel Label { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbabilityL" /> class.
        /// </summary>
        /// <param name="logProbability">The log probability.</param>
        /// <param name="label">The label.</param>
        public LogProbabilityL(double logProbability, [NotNull] ILabel label)
            : base(logProbability)
        {
            Label = label;
        }

        /// <summary>
        /// Converts this log probability <c>log P(l)</c> to a probability <c>P(l)</c>
        /// </summary>
        /// <returns>ProbabilityL.</returns>
        [NotNull]
        public ProbabilityL ToProbability()
        {
            return new ProbabilityL(Math.Exp(Value), Label);
        }
    }
}
