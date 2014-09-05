using System.Collections.Generic;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification
{
    /// <summary>
    /// Interface ICollectionAccess
    /// </summary>
    public interface IIndexedCollectionAccess<out T> : IReadOnlyCollection<T>
    {
        /// <summary>
        /// Gets the <see cref="T"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>T.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index;The index was out of range.</exception>
        [NotNull]
        T this[int index] { get; }
    }
}
