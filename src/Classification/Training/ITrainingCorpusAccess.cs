using System.Collections.Generic;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;

namespace widemeadows.MachineLearning.Classification.Training
{
    /// <summary>
    /// Interface ITrainingCorpus
    /// </summary>
    public interface ITrainingCorpusAccess : IEnumerable<ILabeledDocument>
    {
        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>The label.</value>
        [NotNull]
        ILabel Label { get; }

        /// <summary>
        /// Gets the size of the dictionary, i.e. the total count of
        /// all possible (distinct) observations.
        /// </summary>
        /// <value>The size.</value>
        long VocabularySize { [Pure] get; }

        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <value>The document count.</value>
        long DocumentCount { [Pure] get; }
    }
}
