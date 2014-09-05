using System.Diagnostics;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;

namespace widemeadows.MachineLearning.Classification.Scores.Probabilities
{
    /// <summary>
    /// Class ProbabilityL.
    /// <para>
    /// Describes the  probability <c>P(l)</c> of the label.
    /// </para>
    /// </summary>
    [DebuggerDisplay("P({Label})={Value}")]
    public sealed class ProbabilityL : Probability
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
        /// <param name="probability">The probability.</param>
        /// <param name="label">The label.</param>
        public ProbabilityL(double probability, [NotNull] ILabel label)
            : base(probability)
        {
            Label = label;
        }
    }
}
