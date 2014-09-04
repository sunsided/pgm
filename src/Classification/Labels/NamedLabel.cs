using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Labels
{
    /// <summary>
    /// Class NamedLabel.
    /// </summary>
    [DebuggerDisplay("Label {Name}")]
    public class NamedLabel : LabelBase, INamedLabel
    {
        /// <summary>
        /// The string comparison type
        /// </summary>
        protected readonly StringComparison StringComparisonType;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedLabel" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="stringComparisonType">Type of the string comparison.</param>
        /// <exception cref="System.ArgumentNullException">name</exception>
        public NamedLabel([NotNull] string name, StringComparison stringComparisonType = StringComparison.OrdinalIgnoreCase)
        {
            StringComparisonType = stringComparisonType;
            if (name == null) throw new ArgumentNullException("name");
            Name = name;
        }

        /// <summary>
        /// Determines whether the specified <see cref="NamedLabel" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="NamedLabel" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        bool IEquatable<INamedLabel>.Equals([CanBeNull] INamedLabel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return String.Equals(Name, other.Name, StringComparisonType);
        }

        /// <summary>
        /// Determines whether the specified <see cref="NamedLabel" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="NamedLabel" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        protected bool Equals([CanBeNull] NamedLabel other)
        {
            return ((IEquatable<INamedLabel>) this).Equals(other);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals([NotNull] string other)
        {
            return String.Equals(Name, other, StringComparisonType);
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
            return Equals((NamedLabel) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
