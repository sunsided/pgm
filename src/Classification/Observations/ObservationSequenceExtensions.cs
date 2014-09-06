using System.Collections.Generic;
using System.Diagnostics;
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
        [NotNull, DebuggerStepThrough]
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
                yield return e.Current;
            }

            // emit all the symbols!
            var lastObservation = e.Current;
            while (e.MoveNext())
            {
                yield return lastObservation = e.Current;
            }

            // emit stop boundary
            if (!(lastObservation is BoundaryObservation))
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
