using System.Linq;
using FluentAssertions;
using widemeadows.MachineLearning.Classification.Classifiers.Bayes;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Scores;
using widemeadows.MachineLearning.Classification.Scores.Probabilities.Combiners;
using widemeadows.MachineLearning.Classification.Training;

namespace widemeadows.MachineLearning.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            PangramExample();
            DirectionExample();
        }

        /// <summary>
        /// Pangram example
        /// </summary>
        private static void PangramExample()
        {
            var labels = new NamedLabelRegistry();
            var corpora = new CorpusRegistry();

            var corpus = corpora.Add(labels.Add("Pangram 1"));
            corpus.AddSequence("The quick brown fox jumped over the lazy dog.".ToObservationSequence());
            corpus.AddSequence("Waxy and quivering, jocks fumble the pizza.".ToObservationSequence());
            corpus.AddSequence("Foxy diva Jennifer Lopez wasn't baking my quiche.".ToObservationSequence());
            corpus.AddSequence("My girl wove six dozen plaid jackets before she quit.".ToObservationSequence());
            corpus.AddSequence("Grumpy wizards make toxic brew for the evil queen and jack.".ToObservationSequence());
            corpus.AddSequence("The wizard quickly jinxed the gnomes before they vaporized.".ToObservationSequence());

            corpus = corpora.Add(labels.Add("Pangram 2"));
            corpus.AddSequence("A quick movement of the enemy will jeopardize six gunboats.".ToObservationSequence());
            corpus.AddSequence("The five boxing wizards jump quickly.".ToObservationSequence());
            corpus.AddSequence("Heavy boxes perform waltzes and jigs.".ToObservationSequence());
            corpus.AddSequence("Pack my box with five dozen liquor jugs.".ToObservationSequence());
            corpus.AddSequence("The lazy major was fixing Cupid's broken quiver.".ToObservationSequence());
            corpus.AddSequence("A very big box sailed up then whizzed quickly from Japan.".ToObservationSequence());

            var priorResolver = labels.GetEqualDistribution();
            var evidenceCombinerFactory = EtaEvidenceCombiner.Factory;
            var probabilityCalculator = new LaplaceSmoothingProbabilityCalculator(corpora);

            // fetch a new classifier and train it using the corpora
            var classifier = new OptimizedNaiveBayesClassifier(priorResolver, evidenceCombinerFactory, probabilityCalculator).TrainedWith(corpora);

            var results = classifier.Classify("My toxic grumpy girlfriend quickly jumped over the cheesy pizza".ToObservationSequence());
            var bestResult = results.BestScore;
            bestResult.Label.As<NamedLabel>().Name.Should().Be("Pangram 1", "this class is more likely.");

            var results2 = classifier.Classify("The lazy major was fixing Cupid's broken quiver.".ToObservationSequence());
            var bestResult2 = results2.BestScore;
            bestResult2.Label.As<NamedLabel>().Name.Should().Be("Pangram 2", "this class is more likely.");

            var results3 = classifier.Classify("Pack my box with jinxed Lopez jackets quiche.".ToObservationSequence());
            var bestResult3 = results3.BestScore;
            bestResult3.Label.As<NamedLabel>().Name.Should().Be("Pangram 2", "this class is more likely.");

            var results4 = classifier.Classify("This sentence isn't even close to appear in these training corpora.".ToObservationSequence());
            results4.First().Value.Should().BeLessThan(0.5, "because this sentence is not in any way trained.");
            results4.Last().Value.Should().BeLessThan(0.5, "because this sentence is not in any way trained.");
        }


        /// <summary>
        /// Direction Example
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private static void DirectionExample()
        {
            var labels = new NamedLabelRegistry();
            var corpora = new CorpusRegistry();

            var corpus = corpora.Add(labels.Add("Left Half"));
            corpus.AddSequence("left".ToObservationSequence(BoundaryMode.NoBoundaries));
            corpus.AddSequence("center".ToObservationSequence(BoundaryMode.NoBoundaries));

            corpus = corpora.Add(labels.Add("Right Half"));
            corpus.AddSequence("right".ToObservationSequence(BoundaryMode.NoBoundaries));
            corpus.AddSequence("center".ToObservationSequence(BoundaryMode.NoBoundaries));

            var priorResolver = labels.GetEqualDistribution();
            var evidenceCombinerFactory = EtaEvidenceCombiner.Factory;
            var probabilityCalculator = new LaplaceSmoothingProbabilityCalculator(corpora);

            // fetch a new classifier and train it using the corpora
            var classifier = new OptimizedNaiveBayesClassifier(priorResolver, evidenceCombinerFactory, probabilityCalculator);
            classifier.Learn(corpora);

            // test the left side
            var results = classifier.Classify("left".ToObservationSequence(BoundaryMode.NoBoundaries));
            var bestResult = results.BestScore;
            bestResult.Label.As<NamedLabel>().Name.Should().Be("Left Half", "because this is it.");
            bestResult.Value.Should().BeApproximately(1.0D, 0.01D, "because this is a 100% match.");

            // test the right side
            results = classifier.Classify("right".ToObservationSequence(BoundaryMode.NoBoundaries));
            bestResult = results.BestScore;
            bestResult.Label.As<NamedLabel>().Name.Should().Be("Right Half", "because this is it.");
            bestResult.Value.Should().BeApproximately(1.0D, 0.01D, "because this is a 100% match.");

            // test the center
            results = classifier.Classify("center".ToObservationSequence(BoundaryMode.NoBoundaries));
            bestResult = results.BestScore;
            foreach (var result in results)
            {
                result.Value.Should().Be(0.5, "because \"center\" appears in both sets");
            }
        }
    }
}
