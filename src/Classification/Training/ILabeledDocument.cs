using System;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Labels;

namespace widemeadows.MachineLearning.Classification.Training
{
    /// <summary>
    /// Class LabeledDocument. This class cannot be inherited.
    /// <para>
    ///  This class acts as a proxy between an existing document and a label.
    /// </para>
    /// </summary>
    public interface ILabeledDocument : IDocument, IEquatable<ILabel>
    {
        /// <summary>
        /// The label
        /// </summary>
        [NotNull]
        ILabel Label { get; }
    }
}