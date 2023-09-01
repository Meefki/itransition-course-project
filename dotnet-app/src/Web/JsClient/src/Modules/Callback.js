import React, { useContext, useEffect } from "react"
import { UserManagerContext } from "../Contexts/UserManagerContext";

export function Callback() {

    const mgr = useContext(UserManagerContext);

    async function result() {
        if (!window.location.search)
            window.location = "/home";

        mgr.signinRedirectCallback().then(function() {
            window.location = "/home"
        }).catch(function(e) {
            console.error(e);
        })
    }

    useEffect(() => {
        result()
    })

    return (
        <div style={{height: '100vh'}} className="d-flex align-items-center justify-content-center">
            <h3>Waiting for redirection...</h3>
        </div>
    );
}