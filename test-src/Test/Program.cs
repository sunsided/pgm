using widemeadows.MachineLearning.Classification.Classifiers.Bayes;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Scores.Probabilities;
using widemeadows.MachineLearning.Classification.Training;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var labels = new LabelRegistry();
            var corpora = new CorpusRegistry();

            var corpus = corpora.Add(new TrainingCorpus(labels.Add(new NamedLabel("Pangramm 1"))));
            corpus.AddSequence("The quick brown fox jumped over the lazy dog.".ToObservationSequence());
            corpus.AddSequence("Waxy and quivering, jocks fumble the pizza.".ToObservationSequence());
            corpus.AddSequence("Foxy diva Jennifer Lopez wasn't baking my quiche.".ToObservationSequence());
            corpus.AddSequence("My girl wove six dozen plaid jackets before she quit.".ToObservationSequence());
            corpus.AddSequence("Grumpy wizards make toxic brew for the evil queen and jack.".ToObservationSequence());
            corpus.AddSequence("The wizard quickly jinxed the gnomes before they vaporized.".ToObservationSequence());

            corpus = corpora.Add(new TrainingCorpus(labels.Add(new NamedLabel("Pangramm 2"))));
            corpus.AddSequence("A quick movement of the enemy will jeopardize six gunboats.".ToObservationSequence());
            corpus.AddSequence("The five boxing wizards jump quickly.".ToObservationSequence());
            corpus.AddSequence("Heavy boxes perform waltzes and jigs.".ToObservationSequence());
            corpus.AddSequence("Pack my box with five dozen liquor jugs.".ToObservationSequence());
            corpus.AddSequence("The lazy major was fixing Cupid's broken quiver.".ToObservationSequence());
            corpus.AddSequence("A very big box sailed up then whizzed quickly from Japan.".ToObservationSequence());

            var probabilityResolver = labels.GetEqualDistribution();
            var evidenceCombinerFactory = NaiveEvidenceCombiner.Factory;

            var classifier = new NaiveBayesClassifier(corpora, probabilityResolver, evidenceCombinerFactory);
            var results = classifier.Classify("My toxic grumpy girlfriend quickly jumped over the cheesy pizza".ToObservationSequence());
        }
    }
}
