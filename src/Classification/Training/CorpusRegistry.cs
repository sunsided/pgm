using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;

namespace widemeadows.MachineLearning.Classification.Training
{
    /// <summary>
    /// Class CorpusRegistry. This class cannot be inherited.
    /// </summary>
    public sealed class CorpusRegistry : Registry<ITrainingCorpusAccess>
    {
        /// <summary>
        /// The lazily created empty collection
        /// </summary>
        private static readonly Lazy<CorpusRegistry> LazyEmpty = new Lazy<CorpusRegistry>(() => new CorpusRegistry());

        /// <summary>
        /// Gets the empty collection
        /// </summary>
        /// <value>The empty.</value>
        [NotNull]
        public static IIndexedCollectionAccess<ITrainingCorpusAccess> Empty { get { return LazyEmpty.Value; } }

        /// <summary>
        /// Adds and registers a new corpus.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns>TrainingCorpus.</returns>
        [NotNull]
        public ITrainingCorpus Add([NotNull] ILabel label)
        {
            var corpus = new TrainingCorpus(label);
            return Add(corpus);
        }
    }
}
