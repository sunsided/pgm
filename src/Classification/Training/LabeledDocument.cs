using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;

namespace widemeadows.MachineLearning.Classification.Training
{
    /// <summary>
    /// Class LabeledDocument. This class cannot be inherited.
    /// <para>
    ///  This class acts as a proxy between an existing document and a label.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Document of length {Length} for label {Label} ")]
    public sealed class LabeledDocument : ILabeledDocument
    {
        /// <summary>
        /// The label
        /// </summary>
        [NotNull]
        public ILabel Label { get; private set; }

        /// <summary>
        /// The document
        /// </summary>
        [NotNull]
        private readonly IDocument _document;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabeledDocument" /> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="document">The document.</param>
        public LabeledDocument([NotNull] ILabel label, [NotNull] IDocument document)
        {
            Label = label;
            _document = document;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<IObservation> GetEnumerator()
        {
            return _document.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _document).GetEnumerator();
        }

        /// <summary>
        /// Gets the number of times the <paramref name="observation" /> can be found in the document.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <returns>System.Double.</returns>
        public double GetFrequency(IObservation observation)
        {
            return _document.GetFrequency(observation);
        }

        /// <summary>
        /// Gets the empirical probability that the <paramref name="observation" /> can be found in the document.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <returns>System.Double.</returns>
        public double GetProbability(IObservation observation)
        {
            return _document.GetProbability(observation);
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
            return _document.GetProbability(observation, alpha);
        }

        /// <summary>
        /// Gets the number of observations in the document.
        /// </summary>
        /// <value>The count.</value>
        /// <returns>The number of elements in the collection. </returns>
        public int Length
        {
            get { return _document.Length; }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals([NotNull] ILabel other)
        {
            return Label.Equals(other);
        }

        /// <summary>
        /// Determines whether the specified <see cref="LabeledDocument" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="LabeledDocument" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals([CanBeNull] IDocument other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _document.Equals(other);
        }
        
        /// <summary>
        /// Determines whether the specified <see cref="LabeledDocument" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="LabeledDocument" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        private bool Equals([CanBeNull] LabeledDocument other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _document.Equals(other._document) && Label.Equals(other.Label);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals([CanBeNull] object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is LabeledDocument && Equals((LabeledDocument) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (_document.GetHashCode()*397) ^ Label.GetHashCode();
            }
        }
    }
}
