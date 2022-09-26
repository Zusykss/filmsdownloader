import React from 'react';
import './App.css';
import { Route, Routes } from 'react-router-dom';
import HomeLayout from './containers/homeLayout';
import MoviesPage from './components/MoviesPage';
import SerialsPage from './components/SerialsPage';
import HomePage from './components/home';

function App() {
  return (
    <Routes>
      <Route path="/" element={<HomeLayout/>}>
      <Route index element={<HomePage/>}></Route>
      <Route path="movies" element={<MoviesPage />}></Route>
      <Route path="serials" element={<SerialsPage />}></Route>
      </Route>
    </Routes>
  );
}

export default App;
