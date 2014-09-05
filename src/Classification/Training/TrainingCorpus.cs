using System;
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
    public class TrainingCorpus : ITrainingCorpus, IDictionary
    {
        /// <summary>
        /// The document collection
        /// </summary>
        private readonly ConcurrentBag<IDocument> _documents = new ConcurrentBag<IDocument>();

        /// <summary>
        /// The vocabulary size
        /// </summary>
        private long _vocabularySize;

        /// <summary>
        /// Gets the size of the dictionary, i.e. the total count of
        /// all possible (distinct) observations.
        /// </summary>
        /// <value>The size.</value>
        public long VocabularySize { [Pure] get { return _vocabularySize; } }

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
        /// Initializes a new instance of the <see cref="TrainingCorpus"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        public TrainingCorpus([NotNull] ILabel label)
        {
            Label = label;
        }

        /// <summary>
        /// Determines whether the dictionary contains the specified observation.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <returns><c>true</c> if the dictionary contains the specified observation; otherwise, <c>false</c>.</returns>
        public bool Contains(IObservation observation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the number of times the <paramref name="observation" /> can be found in the document.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <returns>System.Double.</returns>
        public double GetFrequency(IObservation observation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the sequence.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <returns>IDocument{`1}.</returns>
        [NotNull]
        public IDocument AddSequence([NotNull] IObservationSequence sequence)
        {
            var document = new Document(this, sequence);
            _documents.Add(document);
            IncrementVocabularyCountBy(document.Length);
            return document;
        }

        /// <summary>
        /// Increments the vocabulary count by the given <paramref name="length"/>.
        /// </summary>
        /// <param name="length">The length.</param>
        private void IncrementVocabularyCountBy(int length)
        {
            Interlocked.Add(ref _vocabularySize, length);
            
            /*
            long value;
            do
            {
                value = Interlocked.CompareExchange(ref _vocabularySize, 0, 0);
            } while (value != Interlocked.CompareExchange(ref _vocabularySize, value + length, value));
            */
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
