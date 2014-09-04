using System;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Labels
{
    /// <summary>
    /// Class NamedLabelRegistry. This class cannot be inherited.
    /// </summary>
    public sealed class NamedLabelRegistry : LabelRegistry<NamedLabel>
    {
        /// <summary>
        /// The string comparison type
        /// </summary>
        private readonly StringComparison _stringComparisonType;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedLabelRegistry"/> class.
        /// </summary>
        /// <param name="stringComparisonType">Type of the string comparison.</param>
        public NamedLabelRegistry(StringComparison stringComparisonType = StringComparison.OrdinalIgnoreCase)
        {
            _stringComparisonType = stringComparisonType;
        }

        /// <summary>
        /// Creates the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>NamedLabel.</returns>
        [NotNull]
        public NamedLabel Create([NotNull] string name)
        {
            return new NamedLabel(name, _stringComparisonType);
        }
    }
}
