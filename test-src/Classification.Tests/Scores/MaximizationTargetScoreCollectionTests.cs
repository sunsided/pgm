﻿using FluentAssertions;
using NUnit.Framework;
using widemeadows.MachineLearning.Classification.Scores;

namespace widemeadows.MachineLearning.Classification.Tests.Scores
{
    /// <summary>
    /// Class MaximizationTargetScoreCollectionTests.
    /// </summary>
    [TestFixture]
    public class MaximizationTargetScoreCollectionTests : TrackingScoreCollectionTests<IProbability>
    {
        /// <summary>
        /// Generates the collection.
        /// </summary>
        /// <returns>ITargetScoreCollection{IProbability}.</returns>
        protected override ITargetScoreCollection<IProbability> GenerateCollection()
        {
            return new MaximizationTargetScoreCollection<IProbability>();
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
        public void TheBestValueIsTheMaximum()
        {
            var maxScore = GenerateScore(0.1D);
            var minScore = GenerateScore(0.0D);

            Collection.TryAdd(maxScore);
            Collection.TryAdd(minScore);

            Collection.BestScore.Should().Be(maxScore, "because the maximization collection should select the maximum value");

            maxScore = GenerateScore(0.2D);
            Collection.TryAdd(maxScore);

            Collection.BestScore.Should().Be(maxScore, "because the maximization collection should update the maximum value");
        }
    }
}
