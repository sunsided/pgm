using System;
using System.Diagnostics;
using System.Globalization;

namespace widemeadows.MachineLearning.Classification.Scores.Probabilities
{
    /// <summary>
    /// Class Probability.
    /// </summary>
    [DebuggerDisplay("Probability {Value}")]
    public class Probability : ScoreBase, IProbability
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Probability" /> class.
        /// </summary>
        /// <param name="probability">The probability.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">probability;The probability must be in the range of 0 &lt;= p &lt;= 1.</exception>
        /// <exception cref="System.NotFiniteNumberException">The probability must be a finite number in the range of 0 &lt;= p &lt;= 1.</exception>
        public Probability(double probability)
            : base(probability)
        {
            if (probability < 0 || probability > 1) throw new ArgumentOutOfRangeException("probability", probability, "The probability must be in the range of 0 <= p <= 1.");
            if (Double.IsNaN(probability) || Double.IsInfinity(probability)) throw new NotFiniteNumberException("The probability must be a finite number in the range of 0 <= p <= 1.", probability);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "P={0}", Value);
        }
    }
}
