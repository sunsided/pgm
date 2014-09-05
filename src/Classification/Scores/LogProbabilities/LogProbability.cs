using System;
using System.Diagnostics;
using System.Globalization;

namespace widemeadows.MachineLearning.Classification.Scores.LogProbabilities
{
    /// <summary>
    /// Class LogProbability.
    /// </summary>
    [DebuggerDisplay("Log-Probability {Value}")]
    public class LogProbability : ScoreBase, ILogProbability
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogProbability" /> class.
        /// </summary>
        /// <param name="logProbability">The probability.</param>
        /// <exception cref="System.NotFiniteNumberException">The probability must be a finite number in the range of 0 &lt;= p &lt;= 1.</exception>
        public LogProbability(double logProbability)
            : base(logProbability)
        {
            if (Double.IsNaN(logProbability) || Double.IsInfinity(logProbability)) throw new NotFiniteNumberException("The log-probability must be a finite number.", logProbability);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "log P={0}", Value);
        }
    }
}
