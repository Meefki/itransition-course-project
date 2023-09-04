import { UserManager, WebStorageStateStore } from "oidc-client";
import { createContext } from "react";

export const config = {
    authority: process.env.REACT_APP_IDENTITY_AUTHORITY,
    client_id: process.env.REACT_APP_IDENTITY_CLIENT_ID,
    redirect_uri: process.env.REACT_APP_IDENTITY_REDIRECT_URI,
    response_type: process.env.REACT_APP_IDENTITY_RESPONSE_TYPE,
    scope: process.env.REACT_APP_IDENTITY_SCOPE,
    post_logout_redirect_uri : process.env.REACT_APP_IDENTITY_POST_LOGOUT_REDIRECT_URI,
    response_mode: process.env.REACT_APP_IDENTITY_RESPONSE_MODE,
    client_secret: process.env.REACT_APP_IDENTITY_CLIENT_SECRET,
    //monitorSession: true,
    automaticSilentRenew: true,
    userStore: new WebStorageStateStore({ store: localStorage })
};

export const UserManagerContext = createContext(new UserManager());