using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;

namespace widemeadows.MachineLearning.Classification.Training
{
    /// <summary>
    /// Class TrainingCorpus. This class cannot be inherited.
    /// </summary>
    [DebuggerDisplay("Corpus for {Label} containing {DocumentCount} documents")]
    public class TrainingCorpus : ITrainingCorpus
    {
        /// <summary>
        /// The distinct observation registry
        /// </summary>
        private readonly IDistinctObservationRegistry _distinctRegistry;

        /// <summary>
        /// The document collection
        /// </summary>
        private readonly ConcurrentBag<IDocument> _documents = new ConcurrentBag<IDocument>();
        
        /// <summary>
        /// The vocabulary size
        /// </summary>
        private long _totalDocumentLength;

        /// <summary>
        /// Gets the total length of the corpus, i.e. the count of
        /// all (non distinct) observations.
        /// </summary>
        /// <value>The size.</value>
        public long TotalDocumentLength { [Pure] get { return Interlocked.CompareExchange(ref _totalDocumentLength, 0, 0); } }
        
        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <value>The document count.</value>
        public long DocumentCount { [Pure] get { return _documents.Count; } }

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>The label.</value>
        public ILabel Label { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrainingCorpus" /> class.
        /// </summary>
        /// <param name="distinctRegistry">The distinct registry.</param>
        /// <param name="label">The label.</param>
        public TrainingCorpus([NotNull] IDistinctObservationRegistry distinctRegistry, [NotNull] ILabel label)
        {
            _distinctRegistry = distinctRegistry;
            Label = label;
        }

        /// <summary>
        /// Adds the sequence.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <returns>IDocument{`1}.</returns>
        [NotNull]
        public IDocument AddSequence([NotNull] IObservationSequence sequence)
        {
            var document = new Document(sequence);
            _documents.Add(document);
            UpdateDistinctObservations(document);
            return document;
        }

        /// <summary>
        /// Updates the distinct observations.
        /// </summary>
        /// <param name="document">The document.</param>
        private void UpdateDistinctObservations([NotNull] IEnumerable<IObservation> document)
        {
            foreach (var observation in document)
            {
                _distinctRegistry.RegisterDistinctObservation(observation);
                Interlocked.Add(ref _totalDocumentLength, 1);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<ILabeledDocument> GetEnumerator()
        {
            var label = Label;
            return _documents.Select(doc => new LabeledDocument(label, doc)).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _documents).GetEnumerator();
        }
    }
}
