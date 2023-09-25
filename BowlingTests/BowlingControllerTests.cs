using Bowling.Controllers;
using Bowling.Models;
using Bowling.Services;
using BowlingTests;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Tests
{
    public  class BowlingControllerTests
    {
        private IBowlingService bowlingService;
        private IMemoryCache memoryCache;
        private BowlingController _controller;

        [SetUp]
        public void Setup()
        {
            bowlingService = Helper.GetRequiredService<IBowlingService>() ?? throw new ArgumentNullException(nameof(bowlingService));
            memoryCache = Helper.GetRequiredService<IMemoryCache>() ?? throw new ArgumentNullException(nameof(memoryCache));
            _controller = new BowlingController(bowlingService, memoryCache); 

        }

        [Test]
        public void LogicCheck_TestMultipleBalls_AssertsAreTrue()
        {
            _controller.BowlBall();
            _controller.BowlBall();
            var thirdBall = _controller.BowlBall().ToList();

            Assert.IsNotEmpty(thirdBall);
            //Expecting at least one frame complete
            Assert.True(thirdBall.Count > 1);

            var node = thirdBall.First();

            Assert.True(node.frameFinished);
            Assert.True(node.firstRoll != null);
            Assert.True(node.secondRoll != null || node.hadStrike);

            Assert.True((node.firstRoll + (node.secondRoll != null ? node.secondRoll : 0)) == node.currentTotal);
            
        }
    }
}
