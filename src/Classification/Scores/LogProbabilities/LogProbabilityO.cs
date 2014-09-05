using System.Diagnostics;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Scores.Probabilities;

namespace widemeadows.MachineLearning.Classification.Scores.LogProbabilities
{
    /// <summary>
    /// Class LogProbabilityO.
    /// <para>
    /// Describes the log probability <c>log P(o)</c> of the observation.
    /// </para>
    /// </summary>
    [DebuggerDisplay("log P({Observation})={Value}")]
    public sealed class LogProbabilityO : LogProbability
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
        /// <param name="logProbability">The log probability.</param>
        /// <param name="observation">The observation.</param>
        public LogProbabilityO(double logProbability, [NotNull] IObservation observation)
            : base(logProbability)
        {
            Observation = observation;
        }
    }
}
