export default interface Frame {
  currentTotal: number;
  firstRoll: number | null;
  secondRoll: number | null;
  tenthFrameThirdRoll: number | null;
  hadStrike: boolean;
  hadSpare: boolean;
  frameFinished: boolean;
}
