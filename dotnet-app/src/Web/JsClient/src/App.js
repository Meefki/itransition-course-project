import './App.css';
import 'mdb-react-ui-kit/dist/css/mdb.min.css';
import "@fortawesome/fontawesome-free/css/all.min.css";
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoute';
import { UserManagerContext, config } from './Contexts/UserManagerContext';
import { UserManager } from 'oidc-client';

function App() {
  return (
    <UserManagerContext.Provider value={new UserManager(config)}>
      <Routes>
        {AppRoutes.map((route, index) => {
          const { element, ...rest } = route;
          return <Route key={index} {...rest} element={element} />;
        })}
      </Routes>
    </UserManagerContext.Provider>
  );
}

export default App;
