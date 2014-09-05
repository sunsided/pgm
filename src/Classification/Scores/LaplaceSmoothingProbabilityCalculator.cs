using System;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Training;

namespace widemeadows.MachineLearning.Classification.Scores
{
    /// <summary>
    /// Class ProbabilityCalculator. This class cannot be inherited.
    /// </summary>
    public class LaplaceSmoothingProbabilityCalculator : IProbabilityCalculator
    {
        /// <summary>
        /// The dictionary
        /// </summary>
        [NotNull] 
        private readonly IDictionary _dictionary;

        /// <summary>
        /// Gets the size of the vocabulary.
        /// </summary>
        /// <value>The size of the vocabulary.</value>
        private double VocabularySize { get { return _dictionary.VocabularySize; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="LaplaceSmoothingProbabilityCalculator"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public LaplaceSmoothingProbabilityCalculator([NotNull] IDictionary dictionary)
        {
            _dictionary = dictionary;
        }

        /// <summary>
        /// Gets the empirical probability that an <see cref="IObservation" /> can be found in an <see cref="IDocument" />.
        /// </summary>
        /// <param name="frequencyInDocument">The frequency (i.e. number of occurrences) of the observation in the document.</param>
        /// <param name="documentLength">Length of the document.</param>
        /// <returns>System.Double.</returns>
        public double GetProbability(double frequencyInDocument, double documentLength)
        {
            return frequencyInDocument/documentLength;
        }

        /// <summary>
        /// Gets the empirical probability that an <see cref="IObservation" /> can be found in an <see cref="IDocument" />.
        /// </summary>
        /// <param name="frequencyInDocument">The frequency (i.e. number of occurrences) of the observation in the document.</param>
        /// <param name="documentLength">Length of the document.</param>
        /// <param name="alpha">The Laplace smoothing factor.</param>
        /// <returns>System.Double.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">alpha;The Laplace smoothing coefficient must be greater than or equal to zero</exception>
        /// <exception cref="System.NotFiniteNumberException">The Laplace smoothing coefficient must be a finite number</exception>
        public double GetProbability(double frequencyInDocument, double documentLength, double alpha)
        {
            if (alpha < 0) throw new ArgumentOutOfRangeException("alpha", alpha, "The Laplace smoothing coefficient must be greater than or equal to zero");
            if (Double.IsNaN(alpha) || Double.IsInfinity(alpha)) throw new NotFiniteNumberException("The Laplace smoothing coefficient must be a finite number", alpha);

            var vocabularySize = VocabularySize;
            return (frequencyInDocument + alpha) / (alpha * vocabularySize + documentLength);
        }
    }
}
