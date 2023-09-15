import { Tag } from "antd";
import { MDBCheckbox } from "mdb-react-ui-kit";
import React, { useContext, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";
import { FilterOptionsContext } from "../../Contexts/FilterOptionsContext";

function ReviewTr({reviewDesc, check}) {
    const [isCheched, setIsChecked] = useState(false);
    const onCheck = () => {
        setIsChecked(current => !current);
    }

    const { setFilterOptions, setValid } = useContext(FilterOptionsContext);

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

    const setTagFilter = (tag) => {
        setFilterOptions(current => {
            let filters = Array.isArray(current) ? [...current] : [];
            const obj = filters?.find((o, i) => {
                if (o.name === 'tags') {
                    filters[i] = { name: 'tags', value: [tag] }
                    return true;
                }
                return false;
            })
            if (!obj) {
                filters?.push({ name: 'tags', value: [tag] });
            }
            setValid(true);
            return filters;
        });
    }

    return pageLoadingStage ? '' :
    <tr className="align-middle">
        <td><MDBCheckbox checked={isCheched} onChange={() => onCheck()} id={"review-checkbox-" + reviewDesc.id} /></td>
        <td><span role="button" className="link-button" onClick={() => navigate("/review/" + reviewDesc?.id)}>{reviewDesc.name}</span></td>
        <td>{reviewDesc.status}</td>
        <td className="text-nowrap">{t('published-date', { date: reviewDesc?.publishedDate ?? ''})}</td>
        <td className="text-end">{reviewDesc.likes ?? 0}</td>
        <td className="w-25">
            <div>
                {reviewDesc?.tags?.map(t => <Tag role="button" className="m-1" color="#55acee" key={t} onClick={() => setTagFilter(t)}>{t}</Tag>)}
            </div>
        </td>
    </tr>
}

export default ReviewTr;