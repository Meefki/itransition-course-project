import { MDBCheckbox } from "mdb-react-ui-kit";
import React, { useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";

function ReviewTr({reviewDesc, check}) {
    const [isCheched, setIsChecked] = useState(false);
    const onCheck = () => {
        setIsChecked(current => !current);
    }

    const ns = "reviews";
    const { t, i18n } = useTranslation(ns);
    const [pageLoadingStage, setPageLoadingStage] = useState(true);
    const navigate = useNavigate();

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
    <tr className="align-middle">
        <td><MDBCheckbox checked={isCheched} onChange={() => onCheck()} id={"review-checkbox-" + reviewDesc.id} /></td>
        <td><span role="button" className="link-button" onClick={() => navigate("/review/" + reviewDesc?.id)}>{reviewDesc.name}</span></td>
        <td>{reviewDesc.status}</td>
        <td className="text-nowrap">{t('published-date', { date: reviewDesc?.publishedDate ?? ''})}</td>
        <td className="text-end">{reviewDesc.likes ?? 0}</td>
    </tr>
}

export default ReviewTr;