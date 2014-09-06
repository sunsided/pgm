using System;
using System.Diagnostics;
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
        [NotNull, DebuggerStepThrough]
        public static IObservationSequence ToObservationSequence([NotNull] this string sentence, BoundaryMode boundaryMode = BoundaryMode.AddBoundaries, StringComparison stringComparisonType = StringComparison.OrdinalIgnoreCase)
        {
            var words = sentence.Split().Select(CreateStringObservationFromWord);

            // add boundaries only if requested
            if (boundaryMode == BoundaryMode.AddBoundaries)
            {
                return new ObservationSequence(words.WithBoundaries());
            }
            
            return new ObservationSequence(words);
        }

        /// <summary>
        /// Creates the string observation from word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>Func{System.StringStringObservation}.</returns>
        [NotNull, DebuggerStepThrough]
        private static StringObservation CreateStringObservationFromWord([NotNull] string word)
        {
            return new StringObservation(word);
        }
    }
}
