using System.Diagnostics;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;

namespace widemeadows.MachineLearning.Classification.Scores.Likelihoods
{
    /// <summary>
    /// Class ProbabilityL.
    /// <para>
    /// Describes the  probability <c>P(l)</c> of the label.
    /// </para>
    /// </summary>
    [DebuggerDisplay("log L({Label})={Value}")]
    public sealed class LogLikelihoodL : Likelihood
    {
        /// <summary>
        /// Gets or sets the  label.
        /// </summary>
        /// <value>The label.</value>
        [NotNull]
        public ILabel Label { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogLikelihoodL" /> class.
        /// </summary>
        /// <param name="likelihood">The probability.</param>
        /// <param name="label">The label.</param>
        public LogLikelihoodL(double likelihood, [NotNull] ILabel label)
            : base(likelihood)
        {
            Label = label;
        }
    }
}
