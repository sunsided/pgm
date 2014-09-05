using System;
using System.Diagnostics;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;

namespace widemeadows.MachineLearning.Classification.Scores.Probabilities
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
    }
}
