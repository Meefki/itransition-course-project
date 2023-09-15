import { useEffect, useMemo, useState } from "react";
import { ReviewingService } from "../../Services/ReviewingService";
import { useNavigate } from "react-router-dom";
import HrStyle from "../../Assets/Css/hr";
import { UserService } from "../../Services/UserService";
import { useTranslation } from "react-i18next";

function ReviewCarousel() {
    const pageSize = 10;
    const pageNumber = 0;

    const ns = "reviews";
    const { t, i18n } = useTranslation(ns);
    const [pageLoadingStage, setPageLoadingStage] = useState(true);

    const navigate = useNavigate();
    const [items, setItems] = useState([]);
    const reviewingService = useMemo(() => new ReviewingService(), []);
    const userService = useMemo(() => new UserService(), []);
    const [activeSlide, setActiveSlide] = useState(0);
    const [timer, setTimer] = useState('');
    
    useEffect(() => {
        const sortOptions = [
            { name: "likes", value: "desc" },
            { name: "publishedDate", value: "desc" }
        ]

        reviewingService.getShortReviewsDescriptions(pageSize, pageNumber, sortOptions, null, null, true)
            .then((reviews) => {
                setItems(reviews);
            })
    }, [reviewingService, userService]);

    /* eslint-disable */
    useEffect(() => {
        clearTimeout(timer);
        setTimer(setTimeout(() => setActiveSlide(current => (current + 1) % 10), 6000));
    }, [activeSlide]);

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
    }, [i18n, i18n.isInitialized]);
    /* eslint-enable */

    return (items?.length && items?.length > 0) && !pageLoadingStage ?
        <div className="pt-3 carousel slide carousel-fade d-flex flex-column align-items-center" mdb-data-ride="carousel">
            <h3 className="text-center">{t('most-popular-reviews')}</h3>
            <div className="carousel-inner">
                {
                    items.map((item, index) => {
                        return(
                            <div className={`px-5 carousel-item ${index === activeSlide ? 'active' : ''}`} key={index}>
                                <div style={{height: "300px"}} className="px-3 d-flex flex-column align-items-center justify-content-between">
                                    <div onClick={() => navigate(`/review/${item.id}`)} className="px-3 d-flex flex-column align-items-center justify-content-center">
                                        <img className="mb-2" role="button" height={112} width={112} src={item.imageUrl} alt={item.imageUrl}/>
                                        <h4 className="text-center" role="button">{item.name}</h4>
                                    </div>
                                    <div>
                                        <span className="me-1 link-primary" role="button" onClick={() => navigate("/profile/" + item?.authorUserId)}>{item?.userName ?? ''}</span>
                                        <span>&#x2022;</span>
                                        <span className="mx-1">{item?.likesCount ?? 0}</span>
                                        <span>&#x2022;</span>
                                        <span className="mx-1">{item?.subjectGrade ?? 0}</span>
                                        {item?.publishedDate && <span>&#x2022;</span>}
                                        {item?.publishedDate && <span className="mx-1">{t('published-date', { date: item?.publishedDate ?? ''})}</span>}
                                    </div>
                                </div>
                            </div>
                        );
                    })
                }
            </div>
            <div style={HrStyle.horizontalHrStyle}/>
            <button className="carousel-control-prev" type="button" onClick={() => setActiveSlide(current => current !== 0 ? current - 1 : 9)}>
                <span className="carousel-control-prev-icon carousel-dark-custom" aria-hidden="true"></span>
            </button>
            <button className="carousel-control-next" type="button" onClick={() => setActiveSlide(current => (current + 1) % 10)}>
                <span className="carousel-control-next-icon carousel-dark-custom" aria-hidden="true"></span>
            </button>
        </div> : <div className="w-100 text-center">Loading...</div>
}

export default ReviewCarousel;