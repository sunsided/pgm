using System;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Observations
{
    /// <summary>
    /// Interface IStringObservation
    /// </summary>
    public interface IStringObservation : IObservation, IEquatable<string>, IComparable<string>
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        [NotNull]
        string Value { get; }
    }
}
