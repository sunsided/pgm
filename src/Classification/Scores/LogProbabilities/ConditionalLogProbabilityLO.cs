using System.Diagnostics;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Scores.Probabilities;

namespace widemeadows.MachineLearning.Classification.Scores.LogProbabilities
{
    /// <summary>
    /// Class ConditionalProbabilityLO.
    /// <para>
    /// Describes the conditional probability <c>P(l|o)</c> of 
    /// the observation given the label.
    /// </para>
    /// </summary>
    [DebuggerDisplay("log P({Label}|{GivenObservation})={Value}")]
    // ReSharper disable once InconsistentNaming
    public sealed class ConditionalLogProbabilityLO : LogProbability
    {
        /// <summary>
        /// Gets or sets the given observation.
        /// </summary>
        /// <value>The given observation.</value>
        [NotNull]
        public IObservation GivenObservation { get; private set; }

        /// <summary>
        /// Gets or sets the  label.
        /// </summary>
        /// <value>The label.</value>
        [NotNull]
        public ILabel Label { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalProbabilityOL" /> class.
        /// </summary>
        /// <param name="logProbability">The log probability.</param>
        /// <param name="label">The label.</param>
        /// <param name="given">The given.</param>
        public ConditionalLogProbabilityLO(double logProbability, [NotNull] ILabel label, [NotNull] IObservation given)
            : base(logProbability)
        {
            GivenObservation = given;
            Label = label;
        }

        /// <summary>
        /// Implements the *.
        /// </summary>
        /// <param name="cplo">The cplo.</param>
        /// <param name="co">The co.</param>
        /// <returns>The result of the operator.</returns>
        public static JointLogProbabilityOL operator +(ConditionalLogProbabilityLO cplo, LogProbabilityO co)
        {
            Debug.Assert(cplo.GivenObservation.Equals(co.Observation), "Both probabilities must refer to the same observation.");

            var p = cplo.Value + co.Value;
            return new JointLogProbabilityOL(p, co.Observation, cplo.Label);
        }
    }
}
