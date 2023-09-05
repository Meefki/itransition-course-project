import "@fortawesome/fontawesome-free/css/all.min.css";
import { Routes } from './Modules/Routes/AppRoute';
import { UserManagerContext, config } from './Contexts/UserManagerContext';
import { UserManager } from 'oidc-client';
import { useMemo } from "react";

function App() {
  const userManager = useMemo(() => new UserManager(config), [config]);

  const RoutesLoader = () => {
    const { user } = userManager.getUser();
    const isAuth = user !== null;

    return(
      <Routes isAuth={isAuth}/>
    )
  }

  return (
    <UserManagerContext.Provider value={userManager}>
        <RoutesLoader />
    </UserManagerContext.Provider>
  );
}

export default App;
