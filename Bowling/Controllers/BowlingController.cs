﻿using Bowling.Models;
using Bowling.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

namespace Bowling.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BowlingController : ControllerBase
    {
        private readonly IBowlingService _bowlingService;
        private readonly IMemoryCache _cache;
        private LinkedList<FrameModel> _frames;
        private readonly string FrameKey = "frames";
        public BowlingController(IBowlingService bowlingService, IMemoryCache memoryCache)
        {
            _bowlingService = bowlingService;
            _cache = memoryCache;
            _frames = new LinkedList<FrameModel>();
        }

        [HttpGet("[action]")]
        public bool ClearCache()
        {
            this.IMemoryCacheClear(this._cache);
            return true;
        }

        [HttpGet]
        public IEnumerable<FrameModel> Get()
        {
            LinkedList<FrameModel> frameList = this.GetCache(FrameKey);

            FrameModel currentFrame = this.GetCurrentFrame(frameList);


            var result = _bowlingService.BowlBall(10 - (currentFrame?.firstRoll ?? 0));


            bool isTenthFrame = frameList.Count() == 10;

            if(currentFrame.firstRoll == null)
            {
                currentFrame.firstRoll = result;
                
                if(result == 10)
                {
                    currentFrame.hadStrike = true;
                    currentFrame.frameFinished = !isTenthFrame ? true : false;
                }
            }
            else if(currentFrame.secondRoll == null)
            {
                currentFrame.secondRoll = result;

                if (currentFrame.firstRoll + result == 10)
                {
                    currentFrame.hadSpare = true;
                   
                }

                if(isTenthFrame)
                {
                    if((!currentFrame.hadSpare || !currentFrame.hadStrike))
                        currentFrame.frameFinished = true;                        
                }
                else
                {
                    currentFrame.frameFinished = true;
                }

            }
            else
            {
                currentFrame.tenthFrameThirdRoll = result;
                currentFrame.frameFinished = true;
            }
            

            currentFrame.currentTotal = _bowlingService.CalculateTotalScore(frameList);

            LinkedListNode<FrameModel>? curNode = frameList.First;

            while (curNode != null)
            {
                curNode.Value.currentTotal = _bowlingService.CalculateTotalScore(frameList, curNode);

                curNode = curNode.Next;
            }

            

            return frameList.Select((a, index) => new FrameModel
            {
                currentTotal = a.currentTotal,
                firstRoll = a.firstRoll,
                secondRoll = a.secondRoll,
                tenthFrameThirdRoll = a.tenthFrameThirdRoll,
                hadStrike = a.hadStrike,
                hadSpare = a.hadSpare,
                frameFinished = a.frameFinished,
            }) ;
        }

        public void IMemoryCacheClear(IMemoryCache memoryCache)
        {
            PropertyInfo? prop = memoryCache.GetType().GetProperty("EntriesCollection", BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public);
            if (prop is null)
                return;
            object? innerCache = prop.GetValue(memoryCache);
            MethodInfo? clearMethod = innerCache?.GetType().GetMethod("Clear", BindingFlags.Instance | BindingFlags.Public);
            clearMethod?.Invoke(innerCache, null);
        }

        public LinkedList<FrameModel> GetCache(string key)
        {
            if (_cache.TryGetValue(key, out LinkedList<FrameModel> data))
            {
                return data;
            }

            _cache.Set(key, _frames);

            return _frames;
        }

        private FrameModel GetCurrentFrame(LinkedList<FrameModel> frames)
        {
            if(frames.Count() == 0)
            {
                frames.AddFirst(new FrameModel());
            }


            var lastFrame = frames.Last();

            //if(lastFrame.firstRoll == null || (!lastFrame.hadStrike && lastFrame.secondRoll == null) || (frames.Count == 10 && lastFrame.tenthFrameThirdRoll == null))
            if(!lastFrame.frameFinished)
            {
                return lastFrame;
            }
            else
            {
                frames.AddLast(new FrameModel());
                return frames.Last();
            }
        }

    }
}