using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification
{
    /// <summary>
    /// Class LinqExtensions.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Multiplies the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="initial">The initial.</param>
        /// <returns>System.Double.</returns>
        [DebuggerStepThrough]
        public static double Mul([NotNull] this IEnumerable<double> values, double initial = 1.0D)
        {
            return values.Aggregate(initial, (x, current) => x*current);
        }

        /// <summary>
        /// Multiplies the specified values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">The values.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="initial">The initial.</param>
        /// <returns>System.Double.</returns>
        [DebuggerStepThrough]
        public static double Mul<T>([NotNull] this IEnumerable<T> values, [NotNull] Func<T, double> selector, double initial = 1.0D)
        {
            return values.Select(selector).Aggregate(initial, (x, current) => x*current);
        }

        /// <summary>
        /// Multiplies the specified values in log-space.
        /// <para>
        /// exploits the fact that <c>a*b = log(a)+log(b)</c>.
        /// </para>
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>System.Double.</returns>
        [DebuggerStepThrough]
        public static double LogMul([NotNull] this IEnumerable<double> values)
        {
            var logarithm = values.Sum(value => Math.Log(value));
            return Math.Exp(logarithm);
        }

        /// <summary>
        /// Multiplies the specified values in log-space.
        /// <para>
        /// exploits the fact that <c>a*b = log(a)+log(b)</c>.
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">The values.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>System.Double.</returns>
        [DebuggerStepThrough]
        public static double LogMul<T>([NotNull] this IEnumerable<T> values, [NotNull] Func<T, double> selector)
        {
            var logarithm = values.Select(selector).Sum(value => Math.Log(value));
            return Math.Exp(logarithm);
        }
    }
}
