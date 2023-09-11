import { useContext, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import { FilterOptionsContext } from "../../Contexts/FilterOptionsContext";
// import { useNavigate } from "react-router-dom";

function ReviewsFilter({ immutableFilters = [] }) {
    const { filterOptions, setFilterOptions } = useContext(FilterOptionsContext);
    const immutableFilterOptions = immutableFilters?.map((option) => { 
        return { 
            name: option, 
            value: filterOptions[option]
        }
    });

    const ns = "user-profile";
    const { t, i18n } = useTranslation(ns);
    const [pageLoadingStage, setPageLoadingStage] = useState(true);
    //const navigate = useNavigate();

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
        <div>
            Filter
            {/* dropdown with tags */}
            {/* dropdown with existing subjects */}
            {/* publish date */}
            {/*  */}
        </div>
}

export default ReviewsFilter;