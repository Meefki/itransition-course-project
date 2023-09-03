import React, { useContext, useEffect, useState } from "react";
import { UserManagerContext } from "../Contexts/UserManagerContext";
import { ReviewingService } from "../Services/ReviewingService";
import { UserInteraction } from "../Services/UserInteraction";

export function Main() {

    const mgr = useContext(UserManagerContext);
    const [isAuthorized, setIsAuthorized] = useState(false);
    const [tags, setTags] = useState([]);
    const reviewingService = new ReviewingService();
    const userInteraction = new UserInteraction(useContext(UserManagerContext));
    const [isActive, setIsAvtive] = useState(true);

    useEffect(() => {
        mgr.getUser().then(function (user) {
            if (user)
              setIsAuthorized(true)
            else
              setIsAuthorized(false)
        })
    }, [mgr])

    async function login() {
        await userInteraction.login()
        setIsAvtive(false)
    }

    async function logout() {
        await userInteraction.logout()
        setIsAvtive(false)
    }

    async function getTags() {
        setTags(await reviewingService.getTags());
    }

    return(
        <div className="container d-flex flex-row">
            <div className="align-items-baseline justify-content-start">
                <button className="btn btn-primary m-3" onClick={() => getTags()}>Get Tags</button>
                <button className="btn btn-primary m-3" disabled={!isActive} hidden={isAuthorized} onClick={() => login()}>Login</button>
                <button className="btn btn-primary m-3" disabled={!isActive} hidden={!isAuthorized} onClick={() => logout()}>Logout</button>
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