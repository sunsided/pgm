using System.Collections.Generic;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Scores
{
    /// <summary>
    /// Class LinqExtensions.
    /// </summary>
    internal static class LinqExtensions
    {
        /// <summary>
        /// Determines the minimum and maximum values of a score enumeration.
        /// </summary>
        /// <typeparam name="TScore">The type of the t score.</typeparam>
        /// <param name="enumeration">The enumeration.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns><c>true</c> if the collection had at least one item, <c>false</c> otherwise.</returns>
        [ContractAnnotation("=>true,min:notnull,max:notnull;=>false,min:null,max:null")]
        public static bool MinMax<TScore>([NotNull] this IEnumerable<TScore> enumeration, [CanBeNull] out TScore min, [CanBeNull] out TScore max)
            where TScore: IScore
        {
            // fetch the enumerator
            var enumerator = enumeration.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                min = max = default(TScore);
                return false;
            }

            // set the initial values and iterate
            min = max = enumerator.Current;
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (item.CompareTo(min) < 0) min = item;
                if (item.CompareTo(max) > 0) max = item;
            }
            return true;
        }
    }
}
