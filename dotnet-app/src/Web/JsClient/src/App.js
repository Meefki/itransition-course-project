import "@fortawesome/fontawesome-free/css/all.min.css";
import { Routes } from './Modules/Routes/AppRoute';
import { UserManagerContext, config } from './Contexts/UserManagerContext';
import { UserManager } from 'oidc-client';
import { useState, useEffect, useRef } from "react";

function App() {
  const userManager = useRef(new UserManager(config));
  const [pageLoadingStage, setPageLoadingStage] = useState(true);
  const [isAuthorized, setIsAuthorized] = useState(false);

 useEffect(() => {
  setPageLoadingStage(true);

  userManager.current.getUser().then((user) => {

    let isAuth = false;
    if (user) 
      isAuth = true;
    setIsAuthorized(isAuth);
    setPageLoadingStage(false);
  })
 }, [userManager])

  return (
    <UserManagerContext.Provider value={userManager.current}>
      {!pageLoadingStage && <Routes isAuth={isAuthorized.current}/>}
    </UserManagerContext.Provider>
  );
}

export default App;
