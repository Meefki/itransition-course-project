import React, { useEffect } from "react";

export function Logout() {

    async function logout() {
        var query = window.location.search;
        var logoutIdQuery = query && query.toLowerCase().indexOf('?logoutid=') === 0 && query;

        const response = await fetch(process.env.REACT_APP_IDENTITY_AUTHENTICATE_URL + '/logout' + logoutIdQuery, {
          credentials: 'include'
        });

        const data = await response.json();

        if (data.signOutIFrameUrl) {
          var iframe = document.createElement('iframe');
          iframe.width = 0;
          iframe.height = 0;
          iframe.class = 'signout';
          iframe.src = data.signOutIFrameUrl;
          document.getElementById('logout_iframe').appendChild(iframe);
        }

        if (data.postLogoutRedirectUri) {
          window.location = data.postLogoutRedirectUri;
        } else {
          document.getElementById('bye').innerText = 'You can close this window. Bye!';
        }
    }

    useEffect(() => {
        logout()
    }, [])

    return (
        <div className="container d-flex justify-content-center align-items-center">
            <div id="logout_iframe"></div>
            <div style={{height: '100vh'}} className="d-flex align-items-center justify-content-center">
              <h3 id="bye"> </h3>
            </div>
        </div>
    )
}