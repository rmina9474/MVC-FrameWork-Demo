import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import styled from 'styled-components';

// Components
import Header from './components/Header';
import Footer from './components/Footer';

// Pages
import Home from './pages/Home';
import Menu from './pages/Menu';
import Rewards from './pages/Rewards';
import GiftCards from './pages/GiftCards';
import OrderNow from './pages/OrderNow';

const AppContainer = styled.div`
  display: flex;
  flex-direction: column;
  min-height: 100vh;
`;

const Main = styled.main`
  flex: 1;
`;

function App() {
  return (
    <Router>
      <AppContainer>
        <Header />
        <Main>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/menu" element={<Menu />} />
            <Route path="/rewards" element={<Rewards />} />
            <Route path="/gift-cards" element={<GiftCards />} />
            <Route path="/order-now" element={<OrderNow />} />
          </Routes>
        </Main>
        <Footer />
      </AppContainer>
    </Router>
  );
}

export default App; 