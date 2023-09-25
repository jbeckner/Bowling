import React from "react";
import Frame from "../models/Frame";
import "../styles/bowling-panel.css";

type BowlingState = {
  panes?: Frame[] | null;
  disabled: boolean;
  gameOver: boolean;
};

export class BowlingPanel extends React.Component<{}, BowlingState> {
  state: BowlingState = {
    panes: null,
    disabled: true,
    gameOver: false,
  };

  async componentDidMount() {
    await this.clearCache();
    this.setState({ disabled: false });
  }

  async clearCache() {
    this.setState({ panes: null });
    await fetch("bowling/clearcache");
  }

  async playAgain() {
    await this.clearCache();
    this.setState({ gameOver: false, disabled: false });
  }

  async bowlBall() {
    this.setState({ disabled: true });
    const response = await fetch(`bowling`);
    const data = await response.json();
    this.setState({
      panes: data,
    });

    this.checkForEndOfGame();
  }

  checkForEndOfGame() {
    if (this.state.panes?.filter((a) => a.frameFinished).length === 10) {
      //game is over
      this.setState({ disabled: true, gameOver: true });
    } else {
      this.setState({ disabled: false });
    }
  }

  renderFirstRoll(frameData: Frame | null, index: number) {
    return (
      <span style={{ paddingLeft: 8, paddingTop: 5 }}>
        {frameData?.firstRoll != null && (!frameData.hadStrike || index == 10)
          ? frameData.firstRoll == 10
            ? "X"
            : frameData.firstRoll
          : ""}
      </span>
    );
  }

  renderSecondRoll(frameData: Frame | null, index: number) {
    return (
      <span className="second-score">
        {frameData?.hadStrike && index != 10 && "X"}
        {frameData?.secondRoll &&
          (frameData.hadSpare
            ? "/"
            : frameData.secondRoll ?? frameData.secondRoll)}
      </span>
    );
  }

  renderThirdRollLastFrame(frameData: Frame | null) {
    return (
      <span className="tenth-third">
        {frameData?.tenthFrameThirdRoll != null
          ? frameData.tenthFrameThirdRoll === 10
            ? "X"
            : frameData.tenthFrameThirdRoll
          : ""}
      </span>
    );
  }

  renderFrames() {
    var frames = [];
    for (let i = 1; i <= 10; i++) {
      //Get Frame data if it exists as the panels are rerendered
      let frameData =
        this.state.panes != null && this.state.panes.length >= i - 1
          ? this.state.panes[i - 1]
          : null;

      //Build out the frames for render
      frames.push(
        <div key={i} className="panel">
          <div style={{ width: 100, textAlign: "center" }}>{i}</div>
          <div className="frame-cell">
            <div style={{ display: "flex" }}>
              {this.renderFirstRoll(frameData, i)}
              <div style={{ flexGrow: 1 }} />
              {this.renderSecondRoll(frameData, i)}
              {i == 10 && <>{this.renderThirdRollLastFrame(frameData)}</>}
            </div>
            <div style={{ textAlign: "center", paddingTop: 15 }}>
              <span style={{}}>
                {frameData?.frameFinished && frameData?.currentTotal}
              </span>
            </div>
          </div>
        </div>
      );
    }

    return frames;
  }

  render() {
    return (
      <div>
        <div style={{ display: "flex" }}>
          <h1>Bowling Challenge</h1>
          <div style={{ flexGrow: 1 }} />

          {!this.state.gameOver && (
            <>
              <button
                className={"btn btn-primary"}
                disabled={this.state.disabled}
                onClick={() => this.clearCache()}
              >
                New Game
              </button>
              <button
                className={"btn btn-primary"}
                disabled={this.state.disabled}
                onClick={() => this.bowlBall()}
              >
                Bowl Ball
              </button>
            </>
          )}
        </div>
        <div style={{ display: "flex", justifyContent: "center" }}>
          {this.renderFrames()}
        </div>
        {this.state.panes && this.state.gameOver && (
          <div className="gameover" style={{ marginTop: 25 }}>
            <h3>Game Over!</h3>
            <h3>{`Final Score: ${
              this.state.panes[this.state.panes.length - 1].currentTotal
            }`}</h3>
            <div>
              <button
                className={"btn btn-secondary"}
                onClick={() => this.playAgain()}
              >
                Play Again?
              </button>
            </div>
          </div>
        )}
      </div>
    );
  }
}
