using Bowling.Models;

namespace Bowling.Services
{
    public interface IBowlingService
    {
        public int BowlBall(int pinsRemaining);
        public int CalculateTotalScore(LinkedList<FrameModel> frames, LinkedListNode<FrameModel>? currentNode = null);
    }
}
