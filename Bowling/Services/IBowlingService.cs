using Bowling.Models;

namespace Bowling.Services
{
    public interface IBowlingService
    {
        /// <summary>
        /// Roll the bowling ball
        /// </summary>
        /// <param name="pinsRemaining">Number of Pins remaining for the frame</param>
        /// <returns>Number of pins 'knocked' down</returns>
        public int BowlBall(int pinsRemaining);

        /// <summary>
        /// Calculates the score of the frame
        /// </summary>
        /// <param name="frames">The frames linked representing the bowling game</param>
        /// <param name="currentNode">Current Node to stop on</param>
        /// <returns>Score of the frame with lookahead logic</returns>
        public int CalculateTotalScore(LinkedList<FrameModel> frames, LinkedListNode<FrameModel>? currentNode = null);
    }
}
