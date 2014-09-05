using System;
using System.Diagnostics;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Training;

namespace widemeadows.MachineLearning.Classification.Classifiers.Bayes
{
    /// <summary>
    /// Struct DocumentObservationKey
    /// </summary>
    [DebuggerDisplay("Key for {Document} and {Observation}")]
    internal struct LabeledDocumentObservationKey : IEquatable<LabeledDocumentObservationKey>
    {
        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>The label.</value>
        [NotNull]
        public ILabel Label { [Pure] get { return Document.Label; } }

        /// <summary>
        /// The document
        /// </summary>
        [NotNull]
        public readonly ILabeledDocument Document;

        /// <summary>
        /// The observation
        /// </summary>
        [NotNull]
        public readonly IObservation Observation;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabeledDocumentObservationKey"/> struct.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="observation">The observation.</param>
        public LabeledDocumentObservationKey([NotNull] ILabeledDocument document, [NotNull] IObservation observation)
        {
            Document = document;
            Observation = observation;
        }

        /// <summary>
        /// Determines whether the specified <see cref="LabeledDocumentObservationKey" /> is equal to this instance.
        /// </summary>
        /// <param name="other">Another object to compare to.</param>
        /// <returns><see langword="true" /> if the specified <see cref="LabeledDocumentObservationKey" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
        public bool Equals(LabeledDocumentObservationKey other)
        {
            return Document.Equals(other.Document) && Observation.Equals(other.Observation);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><see langword="true" /> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
        public override bool Equals([CanBeNull] object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is LabeledDocumentObservationKey && Equals((LabeledDocumentObservationKey) obj);
        }
        
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Document.GetHashCode()*397) ^ Observation.GetHashCode();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return String.Format("Key for [{0}], observation [{1}]", Document, Observation);
        }
    }
}
