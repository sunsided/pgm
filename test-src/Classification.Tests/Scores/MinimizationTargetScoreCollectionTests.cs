using FluentAssertions;
using NUnit.Framework;
using widemeadows.MachineLearning.Classification.Scores;

namespace widemeadows.MachineLearning.Classification.Tests.Scores
{
    /// <summary>
    /// Class MinimizationTargetScoreCollectionTests.
    /// </summary>
    [TestFixture]
    public class MinimizationTargetScoreCollectionTests : TrackingScoreCollectionTests<IProbability>
    {
        /// <summary>
        /// Generates the collection.
        /// </summary>
        /// <returns>ITargetScoreCollection{IProbability}.</returns>
        protected override ITargetScoreCollection<IProbability> GenerateCollection()
        {
            return new MinimizationTargetScoreCollection<IProbability>();
        }

        /// <summary>
        /// Generates the score.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IProbability.</returns>
        protected override IProbability GenerateScore(double value)
        {
            return new Probability(value);
        }

        /// <summary>
        /// Asserts that the best value is the maximum value added.
        /// </summary>
        [Test]
        public void TheBestValueIsTheMinimum()
        {
            var maxScore = GenerateScore(0.1D);
            var minScore = GenerateScore(0.05D);

            Collection.TryAdd(maxScore);
            Collection.TryAdd(minScore);

            Collection.BestScore.Should().Be(minScore, "because the minimization collection should select the minimum value");

            minScore = GenerateScore(0.0D);
            Collection.TryAdd(minScore);

            Collection.BestScore.Should().Be(minScore, "because the minimization collection should update the minimum value");
        }
    }
}
