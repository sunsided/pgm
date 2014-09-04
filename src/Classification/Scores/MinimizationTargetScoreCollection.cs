using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Scores
{
    /// <summary>
    /// Class MinimizationTargetScoreCollection.
    /// </summary>
    /// <typeparam name="TScore">The type of the score.</typeparam>
    [DebuggerDisplay("minimizing score; {Count} elements")]
    public class MinimizationTargetScoreCollection<TScore> : TrackingScoreCollection<TScore>, ITargetScoreCollection<TScore> 
        where TScore : IScore
    {
        /// <summary>
        /// Gets the best score or <see langword="null"/> if no scores are given.
        /// </summary>
        /// <value>The best score.</value>
        /// <seealso cref="ITargetScoreCollection{TScore}.Minimum"/>
        [CanBeNull]
        public TScore BestScore { get { return Minimum; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreCollection{TScore}"/> class.
        /// </summary>
        /// <param name="scores">The scores.</param>
        public MinimizationTargetScoreCollection([CanBeNull] IEnumerable<TScore> scores = null)
            : base(scores)
        {
        }

        /// <summary>
        /// Gets the enumerator.
        /// <para>
        /// The collection will be returned in an ordered manner with the best element (i.e.
        /// the element with the smallest score) being returned first.
        /// </para>
        /// </summary>
        /// <returns>IEnumerator{`0}.</returns>
        /// <seealso cref="ITargetScoreCollection{TScore}.BestScore"/>
        /// <seealso cref="ITargetScoreCollection{TScore}.Minimum"/>
        public override IEnumerator<TScore> GetEnumerator()
        {
            return ScoreBag.OrderBy(score => score.Value).GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <param name="ordered">if set to <c>true</c>, the collection will be returned in 
        /// an ordered manner with the best element (i.e. the element with the smallest score) 
        /// being returned first.</param>
        /// <returns>IEnumerator{`0}.</returns>
        /// <seealso cref="ITargetScoreCollection{TScore}.BestScore"/>
        [NotNull]
        public IEnumerator<TScore> GetEnumerator(bool ordered)
        {
            if (ordered) return GetEnumerator();
            return base.GetEnumerator();
        }
    }
}
