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
    monitorSession: true,
    automaticSilentRenew: true,
    loadUserInfo: true,
    revokeAccessTokenOnSignout: true,
    userStore: new WebStorageStateStore({ store: localStorage })
};

let google = {...config};
google.client_id = '4910687195-p69q4sjr9u6i50bj86ha9n0n21lvoqln.apps.googleusercontent.com';
google.client_secret = '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';
google.authority = 'https://accounts.google.com';
google.scope = 'sub email name';
export const googleConfig = google;

let facebook = {...config};
facebook.client_id = '681063086965837';
facebook.client_secret = 'b1cacc485924dc3ca24d76f903f6cff9';
facebook.authority = 'https://www.facebook.com';
facebook.scope = 'sub email name';
export const facebookConfig = facebook;

export const UserManagerContext = createContext(new UserManager());