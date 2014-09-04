using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Training;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var label = new NamedLabel("Pangramm");

            var corpus = new TrainingCorpus(label);
            corpus.AddSequence("The quick brown fox jumped over the lazy dog.".ToObservationSequence());
            corpus.AddSequence("Waxy and quivering, jocks fumble the pizza.".ToObservationSequence());
            corpus.AddSequence("Foxy diva Jennifer Lopez wasn't baking my quiche.".ToObservationSequence());
            corpus.AddSequence("My girl wove six dozen plaid jackets before she quit.".ToObservationSequence());
            corpus.AddSequence("Grumpy wizards make toxic brew for the evil queen and jack.".ToObservationSequence());
            corpus.AddSequence("The wizard quickly jinxed the gnomes before they vaporized.".ToObservationSequence());
        }
    }
}
