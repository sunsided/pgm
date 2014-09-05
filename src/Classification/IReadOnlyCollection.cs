// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
#if !NET_45_OR_GREATER

    /// <summary>
    /// Represents a strongly-typed, read-only collection of elements.
    /// </summary>
    /// <remarks>
    /// Since .NET 4.0 does not feature the <see cref="IReadOnlyCollection{T}"/> type,
    /// this interface will be conditionally compiled instead.
    /// </remarks>
    /// <typeparam name="T">The type of the elements.</typeparam>
    public interface IReadOnlyCollection<out T> : IEnumerable<T>
    {
        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        /// <value>The number of elements in the collection.</value>
        int Count { get; }
    }

#endif
}
