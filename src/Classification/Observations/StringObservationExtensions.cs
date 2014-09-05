using System;
using System.Linq;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Observations
{
    /// <summary>
    /// Class StringObservationExtensions.
    /// </summary>
    public static class StringObservationExtensions
    {
        /// <summary>
        /// To the observation sequence.
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        /// <param name="boundaryMode">The boundary mode.</param>
        /// <param name="stringComparisonType">Type of the string comparison.</param>
        /// <returns>IObservationSequence{StringObservation}.</returns>
        [NotNull]
        public static IObservationSequence ToObservationSequence([NotNull] this string sentence, BoundaryMode boundaryMode = BoundaryMode.AddBoundaries, StringComparison stringComparisonType = StringComparison.OrdinalIgnoreCase)
        {
            var words = sentence.Split().Select(word => new StringObservation(word));

            // add boundaries only if requested
            if (boundaryMode == BoundaryMode.AddBoundaries)
            {
                return new ObservationSequence(words.WithBoundaries());
            }
            
            return new ObservationSequence(words);
        }
    }
}
