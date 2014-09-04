using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Scores
{
    /// <summary>
    /// Class TrackingScoreCollection.
    /// <para>
    /// Overload of <see cref="ScoreCollection{TScore}"/> that keeps track of
    /// <see cref="Minimum"/> and <see cref="Maximum"/> values.
    /// </para>
    /// </summary>
    /// <typeparam name="TScore">The type of the score.</typeparam>
    public class TrackingScoreCollection<TScore> : ScoreCollection<TScore>
        where TScore : IScore
    {
        /// <summary>
        /// The taunted marker for min and max fields.
        /// <para>
        /// Set to <code>1</code> if the collection has been altered
        /// after min and max values have been cached, <code>0</code> otherwise.
        /// </para>
        /// </summary>
        private int _minMaxTaunted = 0;

        /// <summary>
        /// The minimum score
        /// </summary>
        [CanBeNull]
        private TScore _minimumScore;

        /// <summary>
        /// The maximum score
        /// </summary>
        [CanBeNull]
        private TScore _maximumScore;

        /// <summary>
        /// The min/max update lock
        /// </summary>
        [NotNull]
        private readonly object _minMaxLock = new object();

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        /// <value>The minimum value.</value>
        [CanBeNull]
        public TScore Minimum 
        {
            get
            {
                lock (_minMaxLock)
                {
                    var isTaunted = Interlocked.Exchange(ref _minMaxTaunted, 0);
                    if (isTaunted > 0) ScoreBag.MinMax(out _minimumScore, out _maximumScore);
                    return _minimumScore;
                }
            } 
        }

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        /// <value>The maximum value.</value>
        [CanBeNull]
        public TScore Maximum {
            get
            {
                lock (_minMaxLock)
                {
                    var isTaunted = Interlocked.Exchange(ref _minMaxTaunted, 0);
                    if (isTaunted > 0) ScoreBag.MinMax(out _minimumScore, out _maximumScore);
                    return _maximumScore;
                }
            } 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreCollection{TScore}"/> class.
        /// </summary>
        /// <param name="scores">The scores.</param>
        public TrackingScoreCollection([CanBeNull] IEnumerable<TScore> scores = null)
            : base(scores)
        {
        }

        /// <summary>
        /// Attempts to add an object to the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" />.</param>
        /// <returns>true if the object was added successfully; otherwise, false.</returns>
        public override bool TryAdd([NotNull] TScore item)
        {
            if (!base.TryAdd(item)) return false;
            Interlocked.Exchange(ref _minMaxTaunted, 1);
            return true;
        }

        /// <summary>
        /// Attempts to remove and return an object from the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" />.
        /// </summary>
        /// <param name="item">When this method returns, if the object was removed and returned successfully, <paramref name="item" /> contains the removed object. If no object was available to be removed, the value is unspecified.</param>
        /// <returns>true if an object was removed and returned successfully; otherwise, false.</returns>
        public override bool TryTake(out TScore item)
        {
            if (!base.TryTake(out item)) return false;
            Interlocked.Exchange(ref _minMaxTaunted, 1);
            return true;
        }
    }
}
