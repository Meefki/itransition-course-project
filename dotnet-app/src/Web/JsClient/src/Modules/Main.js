import React, { useContext, useEffect, useState, useMemo } from "react";
import { UserManagerContext } from "../Contexts/UserManagerContext";
import { ReviewingService } from "../Services/ReviewingService";
import { UserInteraction } from "../Services/UserInteraction";
import { useTranslation } from 'react-i18next';

export function Main() {

    const ns = 'reviews';
    const { t, i18n } = useTranslation(ns);
    const [pageLoadingStage, setPageLoadingStage] = useState(true);

    const mgr = useContext(UserManagerContext);
    const [isAuthorized, setIsAuthorized] = useState(false);
    const [tags, setTags] = useState([]);
    const reviewingService = new ReviewingService();
    const userInteraction = new UserInteraction(useContext(UserManagerContext));
    const [isActive, setIsAvtive] = useState(true);

    useEffect(() => {
        userInteraction.isAuthorized().then((isAuth) => {
            setIsAuthorized(isAuth);
        });
    })

    /* eslint-disable */
    useEffect(() => {
        i18n.isInitialized &&
        !i18n.hasLoadedNamespace(ns) && 
            i18n.loadNamespaces(ns)
            .then(() => {
                setPageLoadingStage(false);
            });

        i18n.isInitialized &&
        i18n.hasLoadedNamespace(ns) &&
            setPageLoadingStage(false);
    }, [i18n.isInitialized])
    /* eslint-enable */

    async function getTags() {
        setTags(await reviewingService.getTags());
    }

    return pageLoadingStage ? '' :
        <div className="container d-flex flex-row">
            <div className="align-items-baseline justify-content-start">
                <button className="btn btn-primary m-3" onClick={() => getTags()}>Get Tags</button>
            </div>

            <div>
                <ul>
                    {
                        (tags ? tags.map(tag =>  <li key={tag}>{tag}</li>) : "")
                    }
                </ul>
            </div>
        </div>
}