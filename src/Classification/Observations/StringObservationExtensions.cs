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
        /// <param name="stringComparisonType">Type of the string comparison.</param>
        /// <returns>IObservationSequence{StringObservation}.</returns>
        [NotNull]
        public static IObservationSequence ToObservationSequence([NotNull] this string sentence, StringComparison stringComparisonType = StringComparison.OrdinalIgnoreCase)
        {
            var words = sentence.Split().Select(word => new StringObservation(word)).WithBoundaries();
            return new ObservationSequence(words);
        }
    }
}
