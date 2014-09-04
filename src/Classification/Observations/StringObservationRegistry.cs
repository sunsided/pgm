using System;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Observations
{
    /// <summary>
    /// Class StringObservationRegistry. This class cannot be inherited.
    /// </summary>
    public sealed class StringObservationRegistry : ObservationRegistry<StringObservation>
    {
        /// <summary>
        /// The string comparison type
        /// </summary>
        private readonly StringComparison _stringComparisonType;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringObservationRegistry"/> class.
        /// </summary>
        /// <param name="stringComparisonType">Type of the string comparison.</param>
        public StringObservationRegistry(StringComparison stringComparisonType = StringComparison.OrdinalIgnoreCase)
        {
            _stringComparisonType = stringComparisonType;
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>StringObservation.</returns>
        [NotNull]
        public StringObservation Create([NotNull] string value)
        {
            return new StringObservation(value, _stringComparisonType);
        }
    }
}
