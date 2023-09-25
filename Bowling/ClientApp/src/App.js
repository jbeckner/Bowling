import { Component } from 'react';
import { Route } from 'react-router';
import { BowlingPanel } from './components/BowlingPanel';
import { Layout } from './components/Layout';

import './custom.css';

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route path='/' component={BowlingPanel} />
            </Layout>
        );
    }
}
