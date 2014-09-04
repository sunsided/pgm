using System.Collections.Generic;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Observations;

namespace widemeadows.MachineLearning.Classification.Training
{
    /// <summary>
    /// Interface IDocument
    /// </summary>
    /// <typeparam name="TObservation">The type of the observation.</typeparam>
    public interface IDocument : IObservationSequence
    {
        /// <summary>
        /// Gets the number of times the <paramref name="observation"/> can be found in the document.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <returns>System.Double.</returns>
        double GetFrequency([NotNull] IObservation observation);

        /// <summary>
        /// Gets the empirical probability that the <paramref name="observation"/> can be found in the document.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <returns>System.Double.</returns>
        double GetProbability([NotNull] IObservation observation);

        /// <summary>
        /// Gets the empirical probability that the <paramref name="observation" /> can be found in the document.
        /// <para>
        /// Laplace smoothing is applied with the strength <paramref name="alpha"/>.
        /// </para>
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <param name="alpha">The alpha.</param>
        /// <returns>System.Double.</returns>
        double GetProbability([NotNull] IObservation observation, double alpha);

        /// <summary>
        /// Gets the number of observations in the document.
        /// </summary>
        /// <value>The count.</value>
        /// <returns>The number of elements in the collection. </returns>
        int Length { get; }
    }
}