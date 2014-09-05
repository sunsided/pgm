using System;
using System.Diagnostics;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Scores.Probabilities;

namespace widemeadows.MachineLearning.Classification.Scores.LogProbabilities
{
    /// <summary>
    /// Class ConditionalProbabilityOL.
    /// <para>
    /// Describes the conditional probability <c>P(o|l)</c> of 
    /// the observation given the label.
    /// </para>
    /// </summary>
    [DebuggerDisplay("log P({Observation}|{GivenLabel})={Value}")]
    // ReSharper disable once InconsistentNaming
    public sealed class ConditionalLogProbabilityOL : LogProbability
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
        /// <param name="logProbability">The log probability.</param>
        /// <param name="observation">The observation.</param>
        /// <param name="given">The label.</param>
        public ConditionalLogProbabilityOL(double logProbability, [NotNull] IObservation observation, [NotNull] ILabel given) 
            : base(logProbability)
        {
            Observation = observation;
            GivenLabel = given;
        }

        /// <summary>
        /// Implements the +.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        [NotNull]
        public static ConditionalLogProbabilityOL operator +([NotNull] ConditionalLogProbabilityOL a, [NotNull] ConditionalLogProbabilityOL b)
        {
            Debug.Assert(Equals(a.Observation, b.Observation), "The observations of both classes must match");
            Debug.Assert(Equals(a.GivenLabel, b.GivenLabel), "The labels of both classes must match");

            return new ConditionalLogProbabilityOL(a.Value + b.Value, a.Observation, a.GivenLabel);
        }

        /// <summary>
        /// Converts this log P(o|l) to a probability P(o|l)
        /// </summary>
        /// <returns>ConditionalProbabilityOL.</returns>
        [NotNull]
        public ConditionalProbabilityOL ToProbability()
        {
            return new ConditionalProbabilityOL(Math.Exp(Value), Observation, GivenLabel);
        }

        /// <summary>
        /// Implements the +.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        [NotNull]
        public static ConditionalLogProbabilityOL operator +([NotNull] ConditionalLogProbabilityOL a, double b)
        {
            if (a == null) throw new ArgumentNullException("a");
            return new ConditionalLogProbabilityOL(a.Value + b, a.Observation, a.GivenLabel);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="ConditionalLogProbabilityOL"/> to <see cref="ConditionalProbabilityOL"/>.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>The result of the conversion.</returns>
        [NotNull]
        public static explicit operator ConditionalProbabilityOL([NotNull] ConditionalLogProbabilityOL p)
        {
            if (p == null) throw new ArgumentNullException("p");
            return p.ToProbability();
        }

        /// <summary>
        /// Implements the +.
        /// </summary>
        /// <param name="clpol">The clpol.</param>
        /// <param name="lpl">The LPL.</param>
        /// <returns>The result of the operator.</returns>
        public static JointLogProbabilityOL operator +(ConditionalLogProbabilityOL clpol, LogProbabilityL lpl)
        {
            Debug.Assert(clpol.GivenLabel.Equals(lpl.Label), "Both probabilities must refer to the same label.");

            var logp = clpol.Value + lpl.Value;
            return new JointLogProbabilityOL(logp, clpol.Observation, lpl.Label);
        }
    }
}
