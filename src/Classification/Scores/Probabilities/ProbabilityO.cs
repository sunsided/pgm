using System.Diagnostics;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;

namespace widemeadows.MachineLearning.Classification.Scores.Probabilities
{
    /// <summary>
    /// Class ProbabilityO.
    /// <para>
    /// Describes the probability <c>P(o)</c> of the observation.
    /// </para>
    /// </summary>
    [DebuggerDisplay("P({Observation})={Value}")]
    // ReSharper disable once InconsistentNaming
    public sealed class ProbabilityO : Probability
    {
        /// <summary>
        /// Gets the observation.
        /// </summary>
        /// <value>The observation.</value>
        [NotNull]
        public IObservation Observation { get; private set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalProbabilityOL" /> class.
        /// </summary>
        /// <param name="probability">The probability.</param>
        /// <param name="observation">The observation.</param>
        /// <param name="label">The label.</param>
        public ProbabilityO(double probability, [NotNull] IObservation observation)
            : base(probability)
        {
            Observation = observation;
        }
    }
}
