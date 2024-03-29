﻿using System.Linq;
using FluentAssertions;
using widemeadows.MachineLearning.Classification.Classifiers.Bayes;
using widemeadows.MachineLearning.Classification.Labels;
using widemeadows.MachineLearning.Classification.Observations;
using widemeadows.MachineLearning.Classification.Scores;
using widemeadows.MachineLearning.Classification.Scores.Combiners;
using widemeadows.MachineLearning.Classification.Training;

namespace widemeadows.MachineLearning.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            PangramExample();
            DirectionExample();
            SanFranciscoBostonExample();
        }

        /// <summary>
        /// Pangram example
        /// </summary>
        private static void PangramExample()
        {
            var labels = new NamedLabelRegistry();
            var corpora = new CorpusRegistry();

            var corpus = corpora.Add(labels.Add("Pangram 1"));
            corpus.AddSequence("The quick brown fox jumped over the lazy dog".ToObservationSequence());
            corpus.AddSequence("Waxy and quivering, jocks fumble the pizza".ToObservationSequence());
            corpus.AddSequence("Foxy diva Jennifer Lopez wasn't baking my quiche".ToObservationSequence());
            corpus.AddSequence("My girl wove six dozen plaid jackets before she quit".ToObservationSequence());

            var corpus2 = corpora.Add(labels.Add("Pangram 2"));
            corpus2.AddSequence("Grumpy wizards make toxic brew for the evil queen and jack".ToObservationSequence());
            corpus2.AddSequence("The wizard quickly jinxed the gnomes before they vaporized".ToObservationSequence());
            corpus2.AddSequence("A quick movement of the enemy will jeopardize six gunboats".ToObservationSequence());
            corpus2.AddSequence("The five boxing wizards jump quickly".ToObservationSequence());
            
            var corpus3 = corpora.Add(labels.Add("Pangram 3"));
            corpus3.AddSequence("Heavy boxes perform waltzes and jigs".ToObservationSequence());
            corpus3.AddSequence("Pack my box with five dozen liquor jugs".ToObservationSequence());
            corpus3.AddSequence("The lazy major was fixing Cupid's broken quiver".ToObservationSequence());
            corpus3.AddSequence("A very big box sailed up then whizzed quickly from Japan".ToObservationSequence());

            var priorResolver = corpora.GetDistributionFromVocabularySize();
            var evidenceCombinerFactory = EtaEvidenceCombiner.Factory;
            var probabilityCalculator = new LaplaceSmoothingProbabilityCalculator(corpora);

            // fetch a new classifier and train it using the corpora
            var classifier = new NegationNaiveBayesClassifier(priorResolver, evidenceCombinerFactory, probabilityCalculator).TrainedWith(corpora);

            var results0 = classifier.Classify("The quick brown fox jumped over the lazy dog".ToObservationSequence());
            var bestResult0 = results0.BestScore;
            bestResult0.Label.As<NamedLabel>().Name.Should().Be("Pangram 1", "this class is more likely.");

            var results1 = classifier.Classify("My toxic grumpy girlfriend quickly jumped over the cheesy pizza".ToObservationSequence());
            var bestResult1 = results1.BestScore;
            bestResult1.Label.As<NamedLabel>().Name.Should().Be("Pangram 1", "this class is more likely.");

            var results2 = classifier.Classify("The lazy major was fixing Cupid's broken quiver.".ToObservationSequence());
            var bestResult2 = results2.BestScore;
            bestResult2.Label.As<NamedLabel>().Name.Should().Be("Pangram 3", "this class is more likely.");

            var results3 = classifier.Classify("Pack my box with jinxed Lopez jackets quiche.".ToObservationSequence());
            var bestResult3 = results3.BestScore;
            bestResult3.Label.As<NamedLabel>().Name.Should().Be("Pangram 3", "this class is more likely.");
            bestResult3.Value.Should().BeLessThan(0.05, "this sentence is mixed from different corpora.");

            var results4 = classifier.Classify("This sentence isn't even close to appear in these training corpora.".ToObservationSequence());
            results4.BestScore.Value.Should().BeLessThan(0.01, "because this sentence is not in any way trained.");
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

            corpus = corpora.Add(labels.Add("Center ... Half"));
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
            bestResult.Value.Should().BeApproximately(1.0D, 0.04D, "because this is a 100% match.");

            // test the right side
            results = classifier.Classify("right".ToObservationSequence(BoundaryMode.NoBoundaries));
            bestResult = results.BestScore;
            bestResult.Label.As<NamedLabel>().Name.Should().Be("Right Half", "because this is it.");
            bestResult.Value.Should().BeApproximately(1.0D, 0.04D, "because this is a 100% match.");

            // test the center
            results = classifier.Classify("center".ToObservationSequence(BoundaryMode.NoBoundaries));
            results.BestScore.Value.Should().BeApproximately(0.5, 0.01D, "because \"center\" appears perfectly in the center set");
            results.Last().Value.Should().BeApproximately(0.25, 0.01D, "because \"center\" appears in both left and right sets");
        }

        /// <summary>
        /// Direction Example
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private static void SanFranciscoBostonExample()
        {
            var labels = new NamedLabelRegistry();
            var corpora = new CorpusRegistry();

            const int count = 10;

            var corpus = corpora.Add(labels.Add("San Francisco"));
            for (int i = 0; i < count; ++i)
            {
                corpus.AddSequence("San Francisco".ToObservationSequence(BoundaryMode.NoBoundaries));
            }

            corpus = corpora.Add(labels.Add("Boston"));
            for (int i = 0; i < count; ++i)
            {
                corpus.AddSequence("Boston".ToObservationSequence(BoundaryMode.NoBoundaries));
            }
            var priorResolver = labels.GetEqualDistribution();
            var evidenceCombinerFactory = EtaEvidenceCombiner.Factory;
            var probabilityCalculator = new LaplaceSmoothingProbabilityCalculator(corpora);

            // fetch a new classifier and train it using the corpora
            var classifier = new OptimizedNaiveBayesClassifier(priorResolver, evidenceCombinerFactory, probabilityCalculator);
            classifier.Learn(corpora);

            // test the left side
            var results = classifier.Classify("San Francisco San Francisco San Francisco San Francisco Boston Boston Boston Boston Boston".ToObservationSequence(BoundaryMode.NoBoundaries));
            var bestResult = results.BestScore;
            bestResult.Label.As<NamedLabel>().Name.Should().Be("San Francisco", "because this the way Naive Bayes works.");
        }
    }
}
