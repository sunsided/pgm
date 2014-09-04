using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Observations
{
    /// <summary>
    /// Class ObservationSequence.
    /// </summary>
    public sealed class ObservationSequence : IObservationSequence
    {
        /// <summary>
        /// The _sequence
        /// </summary>
        private readonly List<IObservation> _sequence;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservationSequence" /> class.
        /// </summary>
        /// <param name="observations">The observations.</param>
        public ObservationSequence([CanBeNull] IEnumerable<IObservation> observations = null)
        {
            _sequence = observations == null
                ? new List<IObservation>()
                : new List<IObservation>(observations);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservationSequence" /> class.
        /// </summary>
        /// <param name="startSymbol">The start symbol.</param>
        /// <param name="observations">The observations.</param>
        /// <param name="endSymbol">The end symbol.</param>
        public ObservationSequence([NotNull] IObservation startSymbol, [NotNull] IEnumerable<IObservation> observations, [NotNull] IObservation endSymbol)
        {
            _sequence = new List<IObservation>();
            _sequence.Add(startSymbol);
            _sequence.AddRange(observations);
            _sequence.Add(endSymbol);
        }

        /// <summary>
        /// Determines whether the sequence contains the specified observation.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <returns><c>true</c> if the sequence contains the specified observation; otherwise, <c>false</c>.</returns>
        public bool Contains([NotNull] IObservation observation)
        {
            return _sequence.Contains(observation);
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return _sequence.Count; }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<IObservation> GetEnumerator()
        {
            return _sequence.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _sequence).GetEnumerator();
        }
    }
}
