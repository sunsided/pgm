using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Training;

namespace widemeadows.MachineLearning.Classification.Scores
{
    public interface IProbabilityCalculator
    {
        /// <summary>
        /// Gets the empirical probability that an <see cref="IObservation" /> can be found in an <see cref="IDocument" />.
        /// </summary>
        /// <param name="frequencyInDocument">The frequency (i.e. number of occurrences) of the observation in the document.</param>
        /// <param name="documentLength">Length of the document.</param>
        /// <returns>System.Double.</returns>
        double GetProbability(double frequencyInDocument, double documentLength);

        /// <summary>
        /// Gets the empirical probability that an <see cref="IObservation" /> can be found in an <see cref="IDocument" />.
        /// </summary>
        /// <param name="frequencyInDocument">The frequency (i.e. number of occurrences) of the observation in the document.</param>
        /// <param name="documentLength">Length of the document.</param>
        /// <param name="alpha">The Laplace smoothing factor.</param>
        /// <returns>System.Double.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">alpha;The Laplace smoothing coefficient must be greater than or equal to zero</exception>
        /// <exception cref="System.NotFiniteNumberException">The Laplace smoothing coefficient must be a finite number</exception>
        double GetProbability(double frequencyInDocument, double documentLength, double alpha);
    }
}