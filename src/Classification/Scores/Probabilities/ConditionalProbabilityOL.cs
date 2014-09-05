using System;
using System.Diagnostics;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Scores.LogProbabilities;

namespace widemeadows.MachineLearning.Classification.Scores.Probabilities
{
    /// <summary>
    /// Class ConditionalProbabilityOL.
    /// <para>
    /// Describes the conditional probability <c>P(o|l)</c> of 
    /// the observation given the label.
    /// </para>
    /// </summary>
    [DebuggerDisplay("P({Observation}|{GivenLabel})={Value}")]
    // ReSharper disable once InconsistentNaming
    public sealed class ConditionalProbabilityOL : Probability
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
        public ILabel GivenLabel { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalProbabilityOL" /> class.
        /// </summary>
        /// <param name="probability">The probability.</param>
        /// <param name="observation">The observation.</param>
        /// <param name="given">The label.</param>
        public ConditionalProbabilityOL(double probability, [NotNull] IObservation observation, [NotNull] ILabel given) : base(probability)
        {
            Observation = observation;
            GivenLabel = given;
        }

        /// <summary>
        /// Converts this P(o|l) to a probability log P(o|l)
        /// </summary>
        /// <returns>ConditionalProbabilityOL.</returns>
        [NotNull]
        public ConditionalLogProbabilityOL ToLogProbability()
        {
            return new ConditionalLogProbabilityOL(Math.Log(Value), Observation, GivenLabel);
        }

        /// <summary>
        /// Implements the *.
        /// </summary>
        /// <param name="cpol">The cpol.</param>
        /// <param name="cl">The cl.</param>
        /// <returns>The result of the operator.</returns>
        public static JointProbabilityOL operator *(ConditionalProbabilityOL cpol, ProbabilityL cl)
        {
            Debug.Assert(cpol.GivenLabel.Equals(cl.Label), "Both probabilities must refer to the same label.");

            var p = cpol.Value*cl.Value;
            return new JointProbabilityOL(p, cpol.Observation, cl.Label);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="ConditionalProbabilityOL"/> to <see cref="ConditionalLogProbabilityOL"/>.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>The result of the conversion.</returns>
        [NotNull]
        public static explicit operator ConditionalLogProbabilityOL([NotNull] ConditionalProbabilityOL p)
        {
            return p.ToLogProbability();
        }
    }
}
