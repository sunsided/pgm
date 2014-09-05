using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Observations;

namespace widemeadows.MachineLearning.Classification.Training
{
    /// <summary>
    /// Class Document. This class cannot be inherited.
    /// </summary>
    [DebuggerDisplay("Document, length {Length}")]
    public sealed class Document : IDocument
    {
        /// <summary>
        /// Gets or sets the dictionary.
        /// </summary>
        /// <value>The dictionary.</value>
        [NotNull]
        private IDictionary Dictionary { get; set; }

        /// <summary>
        /// Gets or sets the observations.
        /// </summary>
        /// <value>The observations.</value>
        [NotNull]
        private IObservationSequence Observations { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="observations">The observations.</param>
        public Document([NotNull] IDictionary dictionary, [NotNull] IObservationSequence observations)
        {
            Dictionary = dictionary;
            Observations = observations;
        }

        /// <summary>
        /// Gets the number of times the <paramref name="observation"/> can be found in the document.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <returns>System.Double.</returns>
        public double GetFrequency([NotNull] IObservation observation)
        {
            return Observations.Count(o => o.Equals(observation));
        }

        /// <summary>
        /// Gets the empirical probability that the <paramref name="observation"/> can be found in the document.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <returns>System.Double.</returns>
        public double GetProbability([NotNull] IObservation observation)
        {
            return GetFrequency(observation) / Length;
        }

        /// <summary>
        /// Gets the empirical probability that the <paramref name="observation" /> can be found in the document.
        /// <para>
        /// Laplace smoothing is applied with the strength <paramref name="alpha" />.
        /// </para>
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <param name="alpha">The alpha.</param>
        /// <returns>System.Double.</returns>
        public double GetProbability(IObservation observation, double alpha)
        {
            var frequency = GetFrequency(observation);
            var vocabularySize = Dictionary.VocabularySize;
            return (frequency + alpha) / (alpha * vocabularySize + Length);
        }

        /// <summary>
        /// Determines whether the sequence contains the specified observation.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <returns><c>true</c> if the sequence contains the specified observation; otherwise, <c>false</c>.</returns>
        public bool Contains([NotNull] IObservation observation)
        {
            return Observations.Contains(observation);
        }
        
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<IObservation> GetEnumerator()
        {
            return Observations.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) Observations).GetEnumerator();
        }

        /// <summary>
        /// Gets the number of observations in the document.
        /// </summary>
        /// <value>The count.</value>
        /// <returns>The number of elements in the collection. </returns>
        public int Length
        {
            get { return Observations.Count(); }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals([CanBeNull] IDocument other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        private bool Equals([CanBeNull] Document other)
        {
            return Observations.Equals(other.Observations);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.</param>
        /// <returns><see langword="true" /> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
        public override bool Equals([CanBeNull] object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Document && Equals((Document) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Observations.GetHashCode();
        }
    }
}
