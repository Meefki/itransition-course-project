import "@fortawesome/fontawesome-free/css/all.min.css";
import { Routes } from './Modules/Routes/AppRoute';
import { UserManagerContext, config } from './Contexts/UserManagerContext';
import { UserManager } from 'oidc-client';
import { useEffect, useState, useMemo } from "react";

function App() {
  const userManager = useMemo(() => new UserManager(config), [config]);
  const [isAuthorized, setIsAuthorized] = useState(false);

  useEffect(() => {
    setIsAuthorized(userManager.getUser() ? true : false);
    userManager.clearStaleState(userManager.userStore);
  }, [userManager]);

  return (
    <UserManagerContext.Provider value={userManager}>
      <Routes isAuthorized={isAuthorized}/>
    </UserManagerContext.Provider>
  );
}

export default App;
