using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Scores
{
    /// <summary>
    /// Class ScoreCollection.
    /// </summary>
    /// <typeparam name="TScore">The type of the t score.</typeparam>
    public class ScoreCollection<TScore> : IScoreCollection<TScore>
        where TScore: IScore
    {
        /// <summary>
        /// The score bag
        /// </summary>
        protected readonly IProducerConsumerCollection<TScore> ScoreBag;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreCollection{TScore}"/> class.
        /// </summary>
        /// <param name="scores">The scores.</param>
        public ScoreCollection([CanBeNull] IEnumerable<TScore> scores = null)
        {
            ScoreBag = (scores == null)
                ? new ConcurrentBag<TScore>() 
                : new ConcurrentBag<TScore>(scores);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public virtual IEnumerator<TScore> GetEnumerator()
        {
            return ScoreBag.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) ScoreBag).GetEnumerator();
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(Array array, int index)
        {
            ScoreBag.CopyTo(array, index);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection" />.
        /// </summary>
        /// <value>The count.</value>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection" />.</returns>
        public int Count
        {
            get { return ScoreBag.Count; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
        /// </summary>
        /// <value>The synchronize root.</value>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</returns>
        public object SyncRoot
        {
            get { return ScoreBag.SyncRoot; }
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).
        /// </summary>
        /// <value><c>true</c> if this instance is synchronized; otherwise, <c>false</c>.</value>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.</returns>
        public bool IsSynchronized
        {
            get { return ScoreBag.IsSynchronized; }
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        public void CopyTo([NotNull] TScore[] array, int index)
        {
            ScoreBag.CopyTo(array, index);
        }

        /// <summary>
        /// Attempts to add an object to the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" />.</param>
        /// <returns>true if the object was added successfully; otherwise, false.</returns>
        public virtual bool TryAdd([NotNull] TScore item)
        {
            return ScoreBag.TryAdd(item);
        }

        /// <summary>
        /// Attempts to remove and return an object from the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" />.
        /// </summary>
        /// <param name="item">When this method returns, if the object was removed and returned successfully, <paramref name="item" /> contains the removed object. If no object was available to be removed, the value is unspecified.</param>
        /// <returns>true if an object was removed and returned successfully; otherwise, false.</returns>
        public virtual bool TryTake([CanBeNull] out TScore item)
        {
            return ScoreBag.TryTake(out item);
        }

        /// <summary>
        /// Copies the elements contained in the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" /> to a new array.
        /// </summary>
        /// <returns>A new array containing the elements copied from the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" />.</returns>
        [NotNull]
        public TScore[] ToArray()
        {
            return ScoreBag.ToArray();
        }
    }
}
