namespace Bowling.Models
{
    public class FrameModel
    {
        public int? firstRoll { get; set; }
        public int? secondRoll { get; set; }
        public int? tenthFrameThirdRoll { get; set; }
        public bool hadStrike { get; set; }
        public bool hadSpare { get; set; }
        public int? currentTotal { get; set; }
        public bool frameFinished { get; set; }
    }
}
