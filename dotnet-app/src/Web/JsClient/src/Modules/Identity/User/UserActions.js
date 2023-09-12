import React, { useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
// import { useNavigate } from "react-router-dom";
import HrStyle from '../../../Assets/Css/hr';

function UserActions() {
    const ns = "user-profile";
    const { t, i18n } = useTranslation(ns);
    const [pageLoadingStage, setPageLoadingStage] = useState(true);
    // const navigate = useNavigate();

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
        <div className="mx-4 mb-4 d-flex flex-column">
            <h5>{t('user_profile_user_actions')}</h5>
            <span className="link-button" type="button" aria-disabled="true">{t('user_profile_user_action_edit')}</span>
            <span className="link-button" type="button" aria-disabled="true">{t('user_profile_user_action_delete')}</span>
            <div style={HrStyle.horizontalHrStyle}/>
        </div>
}

export default UserActions;