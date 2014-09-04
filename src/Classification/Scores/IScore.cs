using System;

namespace widemeadows.MachineLearning.Classification.Scores
{
    /// <summary>
    /// Interface IScore
    /// <para>
    /// Describes the score of a classification or a
    /// subtask. Scores, such as likelihoods, probabilities
    /// or arbitrary ranks are described by their <see cref="Value"/>.
    /// </para>
    /// </summary>
    public interface IScore : IComparable<IScore>
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        double Value { get; }
    }
}
