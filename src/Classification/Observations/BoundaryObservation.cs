using System;
using System.ComponentModel;
using System.Diagnostics;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Observations
{
    /// <summary>
    /// Class BoundaryObservation. This class cannot be inherited.
    /// </summary>
    [DebuggerDisplay("Boundary {Type}")]
    public sealed class BoundaryObservation : ObservationBase
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public BoundaryType Type { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundaryObservation" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">The enumeration had an invalid value.</exception>
        public BoundaryObservation(BoundaryType type)
        {
            if (!Enum.IsDefined(typeof(BoundaryType), type)) throw new InvalidEnumArgumentException("The enumeration had an invalid value.");
            Type = type;
        }

        /// <summary>
        /// Determines whether the specified <see cref="BoundaryObservation" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="BoundaryObservation" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        private bool Equals([CanBeNull] BoundaryObservation other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Type == other.Type;
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
            return obj is BoundaryObservation && Equals((BoundaryObservation) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return (int) Type;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">The enumeration had an invalid value.</exception>
        public override string ToString()
        {
            if (Type == BoundaryType.SequenceStart)
            {
                return "\u22A5"; // ⊥
            }
            if (Type == BoundaryType.SequenceEnd)
            {
                return "\u22A4"; // ⊤
            }
            
            throw new InvalidEnumArgumentException("The enumeration had an invalid value.");
        }
    }
}
