using System;
using System.Diagnostics;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;

namespace widemeadows.MachineLearning.Classification.Classifiers.Bayes
{
    /// <summary>
    /// Struct LabeledObservationKey
    /// </summary>
    [DebuggerDisplay("Key for {Label} and {Observation}")]
    internal struct LabeledObservationKey : IEquatable<LabeledObservationKey>
    {
        /// <summary>
        /// The label
        /// </summary>
        [NotNull]
        public readonly ILabel Label;

        /// <summary>
        /// The observation
        /// </summary>
        [NotNull]
        public readonly IObservation Observation;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabeledDocumentObservationKey" /> struct.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="observation">The observation.</param>
        public LabeledObservationKey([NotNull] ILabel label, [NotNull] IObservation observation)
        {
            Label = label;
            Observation = observation;
        }

        /// <summary>
        /// Determines whether the specified <see cref="LabeledDocumentObservationKey" /> is equal to this instance.
        /// </summary>
        /// <param name="other">Another object to compare to.</param>
        /// <returns><see langword="true" /> if the specified <see cref="LabeledDocumentObservationKey" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
        public bool Equals(LabeledObservationKey other)
        {
            return Label.Equals(other.Label) && Observation.Equals(other.Observation);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><see langword="true" /> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
        public override bool Equals([CanBeNull] object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is LabeledObservationKey && Equals((LabeledObservationKey)obj);
        }
        
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Label.GetHashCode()*397) ^ Observation.GetHashCode();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return String.Format("Key for observation [{1}] in [{0}]", Label, Observation);
        }
    }
}
