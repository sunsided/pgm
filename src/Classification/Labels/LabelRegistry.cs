namespace widemeadows.MachineLearning.Classification.Labels
{
    /// <summary>
    /// Class LabelRegistry.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LabelRegistry<T> : Registry<T>
        where T: ILabel
    {
    }

    /// <summary>
    /// Class LabelRegistry.
    /// </summary>
    public sealed class LabelRegistry : LabelRegistry<ILabel>
    {
    }
}
