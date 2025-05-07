import React from 'react';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import HomePage from './pages/InicialPage';
import TransactionsPage from './pages/LancamentoPage';
import ConsolidadoDiarioPage from './pages/ConsolidadoDiarioPage';
import './styles/styles.css';

const App: React.FC = () => {
  return (
    <Router>
      <div className="App">
        <Switch>
          <Route path="/" exact component={HomePage} />
          <Route path="/lancamentos" component={TransactionsPage} />
          <Route path="/consolidado-diario" component={ConsolidadoDiarioPage} />
        </Switch>
      </div>
    </Router>
  );
};

export default App;