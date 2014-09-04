using System.Collections.Generic;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Observations
{
    /// <summary>
    /// Interface IObservations
    /// </summary>
    public interface IObservationSequence : IEnumerable<IObservation>
    {
    }
}
