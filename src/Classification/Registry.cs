﻿using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification
{
    /// <summary>
    /// Class Registry. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">The type</typeparam>
    public class Registry<T> : IIndexedCollectionAccess<T>
    {
        /// <summary>
        /// The entries
        /// <para>
        /// Overriding classes SHOULD NOT add to or remove from this
        /// list directly. Access should only be used to iterate.
        /// </para>
        /// </summary>
        protected readonly List<T> Entries;

        /// <summary>
        /// Initializes a new instance of the <see cref="Registry{T}" /> class.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">capacity;capacity is less than 0</exception>
        public Registry(int capacity = 0)
        {
            if (capacity < 0) throw new ArgumentOutOfRangeException("capacity", capacity, "capacity is less than 0");
            Entries = new List<T>(capacity);
        }
        
        /// <summary>
        /// Adds the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>T.</returns>
        [NotNull]
        public T Add([NotNull] T entry)
        {
            Entries.Add(entry);
            return entry;
        }

        /// <summary>
        /// Adds the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>T.</returns>
        [NotNull]
        public TDerived Add<TDerived>([NotNull] TDerived entry)
            where TDerived: T
        {
            Entries.Add(entry);
            return entry;
        }
        
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return Entries.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get { return Entries.Count; } }

        /// <summary>
        /// Gets at.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>T.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index;The index was out of range.</exception>
        [NotNull]
        public T GetAt(int index)
        {
            if (index < 0 || index >= Entries.Count) throw new ArgumentOutOfRangeException("index", "The index was out of range.");
            return Entries[index];
        }

        /// <summary>
        /// Gets the <see cref="T" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>T.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index;The index was out of range.</exception>
        public T this[int index]
        {
            get { return GetAt(index); }
        }
    }
}
