using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Observations
{
    /// <summary>
    /// Class ObservationSequenceExtensions.
    /// </summary>
    public static class ObservationSequenceExtensions
    {
        /// <summary>
        /// Attaches boundary observations to an existing sequence.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <returns>IEnumerable{IObservation}.</returns>
        [NotNull]
        public static IEnumerable<IObservation> WithBoundaries([NotNull] this IEnumerable<IObservation> sequence)
        {
            var e = sequence.GetEnumerator();
            if (!e.MoveNext()) yield break;

            // emit start boundary
            if (e.Current is BoundaryObservation)
            {
                yield return e.Current;
            }
            else
            {
                yield return new BoundaryObservation(BoundaryType.SequenceStart);
            }

            // emit all the symbols!
            IObservation lastObservation;
            do
            {
                lastObservation = e.Current;
                yield return lastObservation;
            }
            while (e.MoveNext());

            // emit stop boundary
            if (lastObservation is BoundaryObservation)
            {
                yield break;
            }
            else
            {
                yield return new BoundaryObservation(BoundaryType.SequenceEnd);
            }
        }

        /// <summary>
        /// Counts the specified sequence.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <returns>System.Int32.</returns>
        public static int Count([NotNull] this IObservationSequence sequence)
        {
            var observationSequence = sequence as ObservationSequence;
            if (observationSequence != null) return observationSequence.Length;
            return ((IEnumerable<IObservation>) sequence).Count();
        }
    }
}
