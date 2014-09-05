using System.Collections.Generic;

namespace widemeadows.MachineLearning.Classification.Labels
{
    /// <summary>
    /// Class LabelRegistry.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LabelRegistry<T> : Registry<T>, IIndexedCollectionAccess<ILabel>
        where T: ILabel
    {
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public new IEnumerator<ILabel> GetEnumerator()
        {
            foreach (var label in this)
            {
                yield return label;
            }
        }

        /// <summary>
        /// Gets the <see cref="ILabel"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>ILabel.</returns>
        public new ILabel this[int index]
        {
            get { return GetAt(index); }
        }
    }

    /// <summary>
    /// Class LabelRegistry.
    /// </summary>
    public sealed class LabelRegistry : LabelRegistry<ILabel>
    {
    }
}
