using Bowling.Models;
using Bowling.Services;
using BowlingTests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class BowlingServiceTests
    {
        private IBowlingService bowlingService;

        [SetUp]
        public void Setup()
        {
           bowlingService = Helper.GetRequiredService<IBowlingService>() ?? throw new ArgumentNullException(nameof(bowlingService));
        }

        [Test]
        public void BowlBall_BowlsOneBall_ReturnsNumber()
        {
            var result = bowlingService.BowlBall(10);
            Assert.LessOrEqual(result, 10);
        }

        [Test]
        public void CalulateTotalWithOpenFrame_CalulatesOpenFrame_ReturnsTotal()
        {
            LinkedList<FrameModel> frames = new LinkedList<FrameModel>();
            frames.AddFirst(new FrameModel
            {
                firstRoll = 6,
                secondRoll = 2,
                frameFinished = true,
            });

            var totalScore = bowlingService.CalculateTotalScore(frames) ;

            Assert.AreEqual(totalScore, 8);
        }

        [Test]
        public void CalulateTotalWithSpare_CalulatesSpare_ReturnsTotal()
        {
            LinkedList<FrameModel> frames = new LinkedList<FrameModel>();
            frames.AddLast(new FrameModel
            {
                firstRoll = 6,
                secondRoll = 4,
                hadSpare = true,
                frameFinished = true
            });
            frames.AddLast(new FrameModel
            {
                firstRoll = 5,
                secondRoll = 4,
                frameFinished = true
            });
           
            var totalScore = bowlingService.CalculateTotalScore(frames);

            Assert.AreEqual(totalScore, 24);
        }


        [Test]
        public void CalulateTotalWithStrike_CalulatesStrike_ReturnsTotal()
        {
            LinkedList<FrameModel> frames = new LinkedList<FrameModel>();
            frames.AddLast(new FrameModel
            {
                firstRoll = 10,               
                hadStrike = true,
                frameFinished = true
            });
            frames.AddLast(new FrameModel
            {
                firstRoll = 5,
                secondRoll = 4,
                frameFinished = true
            });


            var totalScore = bowlingService.CalculateTotalScore(frames);

            Assert.AreEqual(totalScore, 28);
        }

        [Test]
        public void CalulateTotalLarger_StrikesAndSpare_ReturnsTotal()
        {
            LinkedList<FrameModel> frames = new LinkedList<FrameModel>();
            frames.AddLast(new FrameModel { firstRoll = 10, hadStrike = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 10, hadStrike = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 8, secondRoll = 2, hadSpare = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 6 });


            var totalScore = bowlingService.CalculateTotalScore(frames);

            Assert.AreEqual(totalScore, 64);
        }

        [Test]
        public void CalulateTotalGame_WithTenthFrame_ReturnsTotal()
        {
            LinkedList<FrameModel> frames = new LinkedList<FrameModel>();
            frames.AddLast(new FrameModel { firstRoll = 10, hadStrike = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 10, hadStrike = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 8, secondRoll = 2, hadSpare = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 6, secondRoll = 4, hadSpare = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 10, hadStrike = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 10, hadStrike = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 10, hadStrike = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 10, hadStrike = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 10, hadStrike = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 9, secondRoll = 1, tenthFrameThirdRoll = 8, hadSpare = true, frameFinished = true });


            var totalScore = bowlingService.CalculateTotalScore(frames);

            Assert.AreEqual(totalScore, 241);
        }

        [Test]
        public void CalulateTotalGame_PerfectScore_ReturnsTotal()
        {
            LinkedList<FrameModel> frames = new LinkedList<FrameModel>();
            frames.AddLast(new FrameModel { firstRoll = 10, hadStrike = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 10, hadStrike = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 10, hadStrike = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 10, hadStrike = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 10, hadStrike = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 10, hadStrike = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 10, hadStrike = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 10, hadStrike = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 10, hadStrike = true, frameFinished = true });
            frames.AddLast(new FrameModel { firstRoll = 10, secondRoll = 10, tenthFrameThirdRoll = 10, hadStrike = true, frameFinished = true });


            var totalScore = bowlingService.CalculateTotalScore(frames);

            Assert.AreEqual(totalScore, 300);
        }

    }
}