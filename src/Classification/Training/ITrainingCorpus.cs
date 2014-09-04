using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;

namespace widemeadows.MachineLearning.Classification.Training
{
    /// <summary>
    /// Interface ITrainingCorpus
    /// </summary>
    public interface ITrainingCorpus
    {
        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>The label.</value>
        [NotNull]
        ILabel Label { get; }
    }
}
