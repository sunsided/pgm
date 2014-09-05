using System.Diagnostics;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;

namespace widemeadows.MachineLearning.Classification.Scores.Probabilities
{
    /// <summary>
    /// Class JointProbabilityOL.
    /// <para>
    /// Describes the joint probability <c>P(o,l)</c> of 
    /// the observation given the label.
    /// </para>
    /// </summary>
    [DebuggerDisplay("P({Observation},{Label})={Value}")]
    // ReSharper disable once InconsistentNaming
    public sealed class JointProbabilityOL : Probability
    {
        /// <summary>
        /// Gets or sets the given label.
        /// </summary>
        /// <value>The given label.</value>
        [NotNull]
        public IObservation Observation { get; private set; }

        /// <summary>
        /// Gets or sets the given label.
        /// </summary>
        /// <value>The given label.</value>
        [NotNull]
        public ILabel Label { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalProbabilityOL" /> class.
        /// </summary>
        /// <param name="probability">The probability.</param>
        /// <param name="observation">The observation.</param>
        /// <param name="label">The label.</param>
        public JointProbabilityOL(double probability, [NotNull] IObservation observation, [NotNull] ILabel label) : base(probability)
        {
            Observation = observation;
            Label = label;
        }

        /// <summary>
        /// Implements the *.
        /// </summary>
        /// <param name="jp">The jp.</param>
        /// <param name="po">The po.</param>
        /// <returns>The result of the operator.</returns>
        public static ConditionalProbabilityLO operator /(JointProbabilityOL jp, ProbabilityO po)
        {
            Debug.Assert(jp.Observation.Equals(po.Observation), "Both probabilities must refer to the same observation.");

            var p = jp.Value / po.Value;
            return new ConditionalProbabilityLO(p, jp.Label, po.Observation);
        }

        /// <summary>
        /// Implements the *.
        /// </summary>
        /// <param name="jp">The jp.</param>
        /// <param name="pl">The pl.</param>
        /// <returns>The result of the operator.</returns>
        public static ConditionalProbabilityOL operator /(JointProbabilityOL jp, ProbabilityL pl)
        {
            Debug.Assert(jp.Label.Equals(pl.Label), "Both probabilities must refer to the same labels.");

            var p = jp.Value / pl.Value;
            return new ConditionalProbabilityOL(p, jp.Observation, pl.Label);
        }
    }
}
