using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Observations
{
    /// <summary>
    /// Class StringObservation.
    /// </summary>
    [DebuggerDisplay("String {Value}")]
    public class StringObservation : ObservationBase, IStringObservation
    {
        /// <summary>
        /// The string comparison type
        /// </summary>
        private readonly StringComparison _stringComparisonType;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringObservation" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="stringComparisonType">Type of the string comparison.</param>
        /// <exception cref="System.ArgumentNullException">value</exception>
        [DebuggerStepThrough]
        public StringObservation([NotNull] string value, StringComparison stringComparisonType = StringComparison.OrdinalIgnoreCase)
        {
            _stringComparisonType = stringComparisonType;
            if (value == null) throw new ArgumentNullException("value");
            Value = value;
        }

        /// <summary>
        /// Determines whether the specified <see cref="StringObservation" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="StringObservation" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        protected bool Equals([CanBeNull] StringObservation other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return String.Equals(Value, other.Value, _stringComparisonType);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals([CanBeNull] string other)
        {
            return String.Equals(Value, other, _stringComparisonType);
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other" /> parameter.Zero This object is equal to <paramref name="other" />. Greater than zero This object is greater than <paramref name="other" />.</returns>
        public int CompareTo([CanBeNull] string other)
        {
            return String.Compare(Value, other, _stringComparisonType);
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
            if (obj.GetType() != GetType()) return false;
            return Equals((StringObservation) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Value.ToLowerInvariant().GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Value;
        }
    }
}
