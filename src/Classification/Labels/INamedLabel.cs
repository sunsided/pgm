using System;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Labels
{
    /// <summary>
    /// Interface INamedLabel
    /// </summary>
    public interface INamedLabel : ILabel, IEquatable<INamedLabel>, IEquatable<string>
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        [NotNull]
        string Name { get; }
    }
}
