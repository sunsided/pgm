using System;
using System.Collections.Concurrent;
using System.Diagnostics;
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
        /// Gets the size of the dictionary, i.e. the total count of
        /// all possible (distinct) observations.
        /// </summary>
        /// <value>The size.</value>
        public long Size { get { throw new NotImplementedException(); } }

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
            return document;
        }
    }
}
