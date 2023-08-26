﻿using LoyalHealthAPI.Interfaces;
using LoyalHealthAPI.Services;
using Xunit;
using LoyalHealthAPI.Models.Interfaces;
using Moq;
using System;

namespace LoyalHealthAPI.Tests
{
    [Collection("Sequential")]
    public class ReviewServiceTests
    {
        private readonly IReviewService _classBeingTested;
        private Mock<IMarkovChainTextGenerator> _mockGenerator;

        public ReviewServiceTests()
        {
            _mockGenerator = new Mock<IMarkovChainTextGenerator>();
            _classBeingTested = new ReviewService(_mockGenerator.Object);
        }

        [Fact]

        public void GenerateReview_ReturnsNull_ThrowsException()
        {
            _mockGenerator.Setup(generator => generator.GenerateMarkovString()).Returns("");
            Assert.Throws<Exception>(() => _classBeingTested.GenerateReview());
        }

        [Fact]
        public void GenerateReview_ReturnsRating_InRange()
        {
            _mockGenerator.Setup(generator => generator.GenerateMarkovString()).Returns("Test Review Text");

            var review = _classBeingTested.GenerateReview();
            var actualStarRating = review.StarRating;
            int minExpectedRating = 1;
            int maxExpectedRating = 5;

            Assert.InRange(actualStarRating, minExpectedRating, maxExpectedRating);
        }
    }
}
