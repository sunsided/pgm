using System;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;

namespace widemeadows.MachineLearning.Classification.Training
{
    /// <summary>
    /// Class DocumentExtensions.
    /// </summary>
    public static class DocumentExtensions
    {
        /// <summary>
        /// Withes the label.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="label">The label.</param>
        /// <returns>ILabeledDocument.</returns>
        /// <exception cref="System.ArgumentException">The document given was already labeled with another label.;document</exception>
        [NotNull]
        public static ILabeledDocument WithLabel([NotNull] this IDocument document, [NotNull] ILabel label)
        {
            // check if the document was already labeled
            var labeledDocument = document as ILabeledDocument;
            if (labeledDocument == null) return new LabeledDocument(label, document);

            // if the labels differ, this is an error
            if (!Equals(label, labeledDocument.Label)) throw new ArgumentException("The document given was already labeled with another label.", "document");
            return labeledDocument;
        }
    }
}
