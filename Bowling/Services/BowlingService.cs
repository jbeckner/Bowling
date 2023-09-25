using Bowling.Models;

namespace Bowling.Services
{    
    public class BowlingService : IBowlingService
    {
        private Random random = new Random();
        public BowlingService()
        {

        }

        public int BowlBall(int pinsRemaining)
        {
            return random.Next(pinsRemaining + 1);
        }

        public int CalculateTotalScore(LinkedList<FrameModel> frames, LinkedListNode<FrameModel>? currentNode = null)
        {
            int totalScore = 0;
            LinkedListNode<FrameModel>? curNode = frames.First;

            while (curNode != null)
            {
                //Don't add score for frame still in progress
                if (!curNode.Value.frameFinished)
                {
                    curNode = curNode.Next;
                    continue;
                }

                int frameTotal = 0;
                FrameModel frame = curNode.Value;

                int lookAheadCount = this.CheckLookAheadScoringNumber(frame);

                frameTotal = (frame?.firstRoll ?? 0) + (frame?.secondRoll ?? 0) + (frame?.tenthFrameThirdRoll ?? 0);

                if (lookAheadCount > 0 && curNode.Next != null)
                {
                    frameTotal += curNode.Next.Value.firstRoll ?? 0;
                    lookAheadCount--;

                    if (lookAheadCount > 0)
                    {
                        if (curNode.Next.Value.secondRoll != null)
                        {
                            frameTotal += curNode.Next.Value.secondRoll ?? 0;
                        }
                        else
                        {
                            frameTotal += curNode.Next.Next != null ? curNode.Next.Next.Value.firstRoll ?? 0 : 0;
                        }
                    }
                }

               
                totalScore += frameTotal;

                if (currentNode != null && curNode == currentNode)
                {
                    //break out
                    break;
                }
                curNode = curNode.Next;
            }

            return totalScore;
        }

        private int CheckLookAheadScoringNumber(FrameModel frame)
        {
            return frame.hadStrike ? 2 : frame.hadSpare ? 1 : 0;
        }

    }
}
