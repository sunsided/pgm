using System;

namespace widemeadows.MachineLearning.Classification.Classifiers.Bayes
{
    /// <summary>
    /// Class BayesBase.
    /// </summary>
    public abstract class BayesBase
    {
        /// <summary>
        /// The default Laplace smoothing strength
        /// </summary>
        private const double DefaultLaplaceSmoothing = 0.01D;

        /// <summary>
        /// The _laplace smoothing
        /// </summary>
        private double _laplaceSmoothing = DefaultLaplaceSmoothing;

        /// <summary>
        /// Gets or sets the Laplace smoothing strength.
        /// </summary>
        /// <value>The Laplace smoothing.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">value;The Laplace smoothing strength must be greater than 0.</exception>
        /// <exception cref="System.NotFiniteNumberException">The Laplace smoothing strength must be a finite number.</exception>
        public double LaplaceSmoothing
        {
            get { return _laplaceSmoothing; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value", value, "The Laplace smoothing strength must be greater than 0.");
                if (Double.IsNaN(value) || Double.IsInfinity(value)) throw new NotFiniteNumberException("The Laplace smoothing strength must be a finite number.", value);
                _laplaceSmoothing = value;
            }
        }
    }
}
