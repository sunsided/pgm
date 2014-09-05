using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;

namespace widemeadows.MachineLearning.Classification.Training
{
    /// <summary>
    /// Class CorpusRegistry. This class cannot be inherited.
    /// </summary>
    public sealed class CorpusRegistry : Registry<ITrainingCorpusAccess>, IDictionary, IDistinctObservationRegistry
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
        /// The distinct observation dictionary tracking the counts
        /// </summary>
        private readonly ConcurrentDictionary<IObservation, long> _distinctObservations = new ConcurrentDictionary<IObservation, long>();

        /// <summary>
        /// Adds and registers a new corpus.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns>TrainingCorpus.</returns>
        [NotNull]
        public ITrainingCorpus Add([NotNull] ILabel label)
        {
            var corpus = new TrainingCorpus(this, label);
            return Add(corpus);
        }

        /// <summary>
        /// Gets the size of the dictionary, i.e. the total count of 
        /// all possible (distinct) observations.
        /// </summary>
        /// <value>The size.</value>
        public long VocabularySize
        {
            get { return _distinctObservations.Count; }
        }

        /// <summary>
        /// Gets the size of the dictionary, i.e. the total count of 
        /// all possible (distinct) observations.
        /// </summary>
        /// <value>The size.</value>
        public long TotalDocumentLength
        {
            get { return Entries.Sum(corpus => corpus.TotalDocumentLength); }
        }

        /// <summary>
        /// Registers an observation.
        /// </summary>
        /// <param name="observation">The observation.</param>
        void IDistinctObservationRegistry.RegisterDistinctObservation(IObservation observation)
        {
            _distinctObservations.AddOrUpdate(observation, key => 1, (key, oldCount) => oldCount + 1);
        }

        /// <summary>
        /// Gets all distinct observations.
        /// </summary>
        /// <returns>IEnumerable&lt;IObservation&gt;.</returns>
        public IEnumerable<KeyValuePair<IObservation, long>> GetDistinctObservations()
        {
            return _distinctObservations.AsEnumerable();
        }
    }
}
