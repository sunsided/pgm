using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Labels
{
    /// <summary>
    /// Class LabelBase.
    /// </summary>
    [DebuggerDisplay("Label Base")]
    public abstract class LabelBase : ILabel
    {
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        bool IEquatable<ILabel>.Equals([CanBeNull] ILabel other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            // ReSharper disable once RedundantCast
            // Force generic comparer.
            return Equals((object) other);
        }
    }
}
