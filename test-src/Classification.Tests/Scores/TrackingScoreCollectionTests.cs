using FluentAssertions;
using JetBrains.Annotations;
using NUnit.Framework;
using widemeadows.MachineLearning.Classification.Scores;

namespace widemeadows.MachineLearning.Classification.Tests.Scores
{
    /// <summary>
    /// Class TrackingScoreCollectionTests.
    /// </summary>
    [TestFixture]
    public abstract class TrackingScoreCollectionTests<T>
        where T:IScore
    {
        /// <summary>
        /// The _collection
        /// </summary>
        [NotNull]
        protected ITargetScoreCollection<T> Collection { get; private set; }

        /// <summary>
        /// Sets up the test fixture.
        /// </summary>
        [TestFixtureSetUp]
        public void Setup()
        {
            Collection = GenerateCollection();
        }

        /// <summary>
        /// Generates this instance.
        /// </summary>
        /// <returns>ITargetScoreCollection{IProbability}.</returns>
        [NotNull]
        protected abstract ITargetScoreCollection<T> GenerateCollection();

        /// <summary>
        /// Generates the score.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>`0.</returns>
        [NotNull]
        protected abstract T GenerateScore(double value);

        /// <summary>
        /// Asserts that adding of values updates the minimum and maximum value fields.
        /// </summary>
        [Test]
        public void AddingElementsTracksMinimumAndMaximumValues()
        {
            Collection.TryAdd(GenerateScore(0.1D));
            Collection.TryAdd(GenerateScore(0.0D));

            Collection.Count.Should().Be(2, "because we added two probabilities");

            Collection.Minimum.Should().NotBeNull("because we added four values");
            Collection.Maximum.Should().NotBeNull("because we added four values");
            Collection.Minimum.Value.Should().Be(0D, "because the smallest probability added was zero");
            Collection.Maximum.Value.Should().Be(0.1D, "because the greatest probability added was 0.1D");

            Collection.TryAdd(GenerateScore(0.2D));
            Collection.TryAdd(GenerateScore(0.1D));

            Collection.Count.Should().Be(4, "because we added four probabilities");
            Collection.Minimum.Should().NotBeNull("because we added four values");
            Collection.Maximum.Should().NotBeNull("because we added four values");
            Collection.Minimum.Value.Should().Be(0D, "because the smallest probability added was zero");
            Collection.Maximum.Value.Should().Be(0.2D, "because the greatest probability added was 0.2D");
        }
    }
}
