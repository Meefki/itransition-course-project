import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import { BrowserRouter } from 'react-router-dom';
import i18next from 'i18next';
import LanguageDetector from 'i18next-browser-languagedetector';
import HttpApi from 'i18next-http-backend';
import { initReactI18next } from 'react-i18next';

i18next
.use(LanguageDetector)
.use(HttpApi)
.use(initReactI18next)
.init({
  debug: process.env.REACT_APP_ENV === 'development',
  fallbackLng: 'en',
  defaultNS: ['reviews'],
  detection: {
    order: ['localStorage', 'cookie', 'subdomain'],
    caches: ['localStorage', 'cookie'],
    cookieMinutes: 60,
    cookieDomain: 'reviews'
  },
  backend: {
    loadPath: '/locales/{{lng}}/{{ns}}.json'
  },
  supportedLngs: ['en', 'ru'],
  react: { useSuspense: false }
})

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <BrowserRouter basename={baseUrl}>
      <App />
    </BrowserRouter>
  </React.StrictMode>
);
