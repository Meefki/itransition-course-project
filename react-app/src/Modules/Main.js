import React, { useContext, useEffect, useState } from "react";
import { UserManagerContext } from "../Contexts/UserManagerContext";
import { ReviewingService } from "../Services/ReviewingService";

export function Main() {

    const mgr = useContext(UserManagerContext);
    const [isAuthorized, setIsAuthorized] = useState(false);
    const [tags, setTags] = useState([]);
    const reviewingService = new ReviewingService();

    useEffect(() => {
        mgr.getUser().then(function (user) {
            if (user)
              setIsAuthorized(true)
            else
              setIsAuthorized(false)
        })
    }, [mgr])

    async function login() {
        mgr.signinRedirect()

        mgr.getUser().then(function (user) {
            if (user)
              setIsAuthorized(true)
            else
              setIsAuthorized(false)
        })
    }

    async function logout() {
        mgr.signoutRedirect();

        mgr.getUser().then(function (user) {
            if (user)
              setIsAuthorized(true)
            else
              setIsAuthorized(false)
        })
    }

    async function getTags() {
        setTags(await reviewingService.getTags());
        // if (isAuthorized) {
        //     mgr.getUser().then(async function (user) {
        //         if (user) {
                    
        //         }
        //     });
        // }
        // for sample for now
    }

    return(
        <div className="container d-flex flex-row">
            <div className="align-items-baseline justify-content-start">
                <button className="btn btn-primary m-3" onClick={() => getTags()}>Get Tags</button>
                <button className="btn btn-primary m-3" hidden={isAuthorized} onClick={() => login()}>Login</button>
                <button className="btn btn-primary m-3" hidden={!isAuthorized} onClick={() => logout()}>Logout</button>
            </div>

            <div>
                <ul>
                    {
                        (tags ? tags.map(tag =>  <li key={tag}>{tag}</li>) : "")
                    }
                </ul>
            </div>
        </div>
    )
}