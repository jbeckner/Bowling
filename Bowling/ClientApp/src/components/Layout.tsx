import React from "react";
import { Container } from "reactstrap";

export class Layout extends React.Component<{ children: React.ReactNode }, {}> {
  render() {
    return (
      <div>
        <Container>{this.props.children}</Container>
      </div>
    );
  }
}
