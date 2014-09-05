﻿using System.Collections.Generic;
using JetBrains.Annotations;
using widemeadows.MachineLearning.Classification.Observations;

namespace widemeadows.MachineLearning.Classification.Training
{
    /// <summary>
    /// Interface IDictionary
    /// </summary>
    public interface IDictionary
    {
        /// <summary>
        /// Gets the size of the dictionary, i.e. the total count of 
        /// all possible (distinct) observations.
        /// </summary>
        /// <value>The size.</value>
        long VocabularySize { get; }

        /// <summary>
        /// Gets all distinct observations and their total counts.
        /// </summary>
        /// <returns>IEnumerable&lt;IObservation&gt;.</returns>
        [NotNull]
        IEnumerable<KeyValuePair<IObservation, long>> GetDistinctObservations();
    }
}
