namespace widemeadows.MachineLearning.Classification.Observations
{
    /// <summary>
    /// Class ObservationRegistry.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservationRegistry<T> : Registry<T>
        where T: IObservation
    {
    }
}
