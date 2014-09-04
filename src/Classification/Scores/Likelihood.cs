using System;
using System.Diagnostics;
using System.Globalization;

namespace widemeadows.MachineLearning.Classification.Scores
{
    /// <summary>
    /// Class Likelihood.
    /// </summary>
    [DebuggerDisplay("Likelihood {Value}")]
    public class Likelihood : ScoreBase, ILikelihood
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Likelihood"/> class.
        /// </summary>
        /// <param name="likelihood">The likelihood value.</param>
        public Likelihood(double likelihood)
            : base(likelihood)
        {
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "L={0}", Value);
        }
    }
}
