import { 
    MDBCol, MDBRow, MDBBadge
} from "mdb-react-ui-kit";
import { useNavigate } from "react-router-dom";
import { horizontalHrStyle } from "../../Assets/Css/hr";
import { useTranslation } from "react-i18next";
import { useMemo, useState } from "react";

function ReviewCard({ reviewDesc }) {

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

    return pageLoadingStage ? "" :
        <div className="user-select-none mt-2">
            <MDBRow>
                <div className="d-flex justify-content-start">
                    <span className="me-1 link-primary" role="button" onClick={() => navigate("/profile/" + reviewDesc?.authorUserId)}>{reviewDesc?.userName ?? ''}</span>
                    <span>&#x2022;</span>
                    <span className="mx-1">{reviewDesc?.likesCount ?? 0}</span>
                    <span>&#x2022;</span>
                    <span className="mx-1">{reviewDesc?.subjectGrade ?? 0}</span>
                    {reviewDesc?.publishedDate && <span>&#x2022;</span>}
                    {reviewDesc?.publishedDate &&<span className="mx-1">{t('published-date', { date: reviewDesc?.publishedDate ?? ''})}</span>}
                </div>
            </MDBRow>
            <MDBRow>
                <MDBCol size="8" sm="9" md="9" lg="9">
                    <div onClick={() => navigate("/review/" + reviewDesc?.id)} role="button">
                        <h5>{reviewDesc?.name}</h5>
                        <p>{reviewDesc?.shortDesc}</p>
                    </div>
                    <div>
                        {reviewDesc?.tags?.map((tag) => <MDBBadge className="me-2" key={tag}>{tag}</MDBBadge>)}
                    </div>
                </MDBCol>
                <MDBCol size="4" sm="3" md="3" lg="3" className="d-flex justify-content-end d-none d-sm-flex"> 
                    <img
                        src={reviewDesc?.imageUrl ?? "https://placehold.co/200x200"}
                        role="button"
                        width="112px" 
                        height="112px" 
                        alt=''
                        className="me-0 me-lg-4"
                        onClick={() => navigate("/review/" + reviewDesc?.id)}/>
                </MDBCol>
            </MDBRow>
            <div style={horizontalHrStyle} className="mt-2"/>
        </div>
}

export default ReviewCard;