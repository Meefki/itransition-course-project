import React, { useEffect } from "react";
import { IdentityService } from "../../Services/IdentityService"

export function Logout() {
    const identityService = new IdentityService();
    async function logout() {
        var query = window.location.search;
        var logoutIdQuery = query && query.toLowerCase().indexOf('?logoutid=') === 0 && query;

        const data = await identityService.logout(logoutIdQuery);

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
    })

    return (
        <div className="container d-flex justify-content-center align-items-center">
            <div id="logout_iframe"></div>
            <div style={{height: '100vh'}} className="d-flex align-items-center justify-content-center">
              <h3 id="bye"> </h3>
            </div>
        </div>
    )
}