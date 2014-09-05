using System;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Training;

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
        /// The training corpora
        /// </summary>
        [CanBeNull]
        private IIndexedCollectionAccess<ITrainingCorpusAccess> _trainingCorpora;

        /// <summary>
        /// The training corpora
        /// </summary>
        [NotNull]
        protected IIndexedCollectionAccess<ITrainingCorpusAccess> TrainingCorpora
        {
            get
            {
                var corpora = _trainingCorpora;
                return corpora ?? CorpusRegistry.Empty;
            }
        }

        /// <summary>
        /// Learns the posterior probabilities from specified training corpora.
        /// </summary>
        /// <param name="trainingCorpora">The training corpora.</param>
        public virtual void Learn([NotNull] IDictionary trainingCorpora)
        {
            _trainingCorpora = trainingCorpora;
            LearnInternal(trainingCorpora);
        }

        /// <summary>
        /// Learns the posterior probabilities from specified training corpora.
        /// <para>
        /// This method is called internally by <see cref="BayesBase.Learn"/>.
        /// </para>
        /// </summary>
        /// <param name="trainingCorpora">The training corpora.</param>
        protected abstract void LearnInternal([NotNull] IDictionary trainingCorpora);

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
