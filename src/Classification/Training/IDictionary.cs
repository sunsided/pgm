using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Observations;

namespace widemeadows.MachineLearning.Classification.Training
{
    /// <summary>
    /// Interface IDictionary
    /// </summary>
    public interface IDictionary
    {
        /// <summary>
        /// Determines whether the dictionary contains the specified observation.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <returns><c>true</c> if the dictionary contains the specified observation; otherwise, <c>false</c>.</returns>
        bool Contains([NotNull] IObservation observation);

        /// <summary>
        /// Gets the number of times the <paramref name="observation"/> can be found in the document.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <returns>System.Double.</returns>
        double GetFrequency([NotNull] IObservation observation);

        /// <summary>
        /// Gets the size of the dictionary, i.e. the total count of 
        /// all possible (distinct) observations.
        /// </summary>
        /// <value>The size.</value>
        long VocabularySize { get; }
    }
}
