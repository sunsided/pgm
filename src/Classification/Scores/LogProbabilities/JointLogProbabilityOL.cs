using System;
using System.Diagnostics;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Scores.Probabilities;

namespace widemeadows.MachineLearning.Classification.Scores.LogProbabilities
{
    /// <summary>
    /// Class JointLogProbabilityOL.
    /// <para>
    /// Describes the joint log probability <c>log P(o,l)</c> of 
    /// the observation given the label.
    /// </para>
    /// </summary>
    [DebuggerDisplay("log P({Observation},{Label})={Value}")]
    // ReSharper disable once InconsistentNaming
    public sealed class JointLogProbabilityOL : LogProbability
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
        /// <param name="logProbability">The log probability.</param>
        /// <param name="observation">The observation.</param>
        /// <param name="label">The label.</param>
        public JointLogProbabilityOL(double logProbability, [NotNull] IObservation observation, [NotNull] ILabel label) : base(logProbability)
        {
            Observation = observation;
            Label = label;
        }

        /// <summary>
        /// Converts to the joint probability <c>P(o,l)</c>
        /// </summary>
        /// <returns>JointProbabilityOL.</returns>
        [NotNull]
        public JointProbabilityOL ToJointProbability()
        {
            return new JointProbabilityOL(Math.Exp(Value), Observation, Label);
        }

        /// <summary>
        /// Implements the -.
        /// </summary>
        /// <param name="cplo">The cplo.</param>
        /// <param name="co">The co.</param>
        /// <returns>The result of the operator.</returns>
        public static ConditionalLogProbabilityLO operator -(JointLogProbabilityOL cplo, LogProbabilityO co)
        {
            Debug.Assert(cplo.Observation.Equals(co.Observation), "Both probabilities must refer to the same observation.");

            var p = cplo.Value - co.Value;
            return new ConditionalLogProbabilityLO(p, cplo.Label, co.Observation);
        }
    }
}
