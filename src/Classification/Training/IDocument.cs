using System;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Observations;

namespace widemeadows.MachineLearning.Classification.Training
{
    /// <summary>
    /// Interface IDocument
    /// </summary>
    public interface IDocument : IObservationSequence, IEquatable<IDocument>
    {
        /// <summary>
        /// Gets the number of times the <paramref name="observation"/> can be found in the document.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <returns>System.Double.</returns>
        double GetFrequency([NotNull] IObservation observation);

        /// <summary>
        /// Gets the number of observations in the document.
        /// </summary>
        /// <value>The count.</value>
        /// <returns>The number of elements in the collection. </returns>
        int Length { get; }
    }
}