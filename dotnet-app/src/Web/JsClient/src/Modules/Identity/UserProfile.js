import React, { useMemo, useState } from "react";
import { useTranslation } from "react-i18next";

function UserProfile() {

    const ns = 'user-profile'
    const { t, i18n} = useTranslation(ns);
    const [pageLoadingStage, setPageLoadingStage] = useState(true);

    /* eslint-disable */
    useMemo(() => {
        i18n.isInitialized &&
        !i18n.hasLoadedNamespace(ns) && 
            i18n.loadNamespaces(ns)
            .then(() => {
                setPageLoadingStage(false);
            });
        i18n.isInitialized &&
        i18n.hasLoadedNamespace(ns) &&
            setPageLoadingStage(false);
    }, [i18n.isInitialized]);
    /* eslint-enable */

    return pageLoadingStage ? '' :
        <div>User page</div>
    
}

export default UserProfile;