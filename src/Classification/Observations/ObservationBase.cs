using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Observations
{
    /// <summary>
    /// Class ObservationBase.
    /// </summary>
    [DebuggerDisplay("Observation Base")]
    public abstract class ObservationBase : IObservation
    {
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        bool IEquatable<IObservation>.Equals([CanBeNull] IObservation other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            // ReSharper disable once RedundantCast
            // Force generic comparer.
            return Equals((object) other);
        }
    }
}
