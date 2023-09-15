import { useContext, useEffect, useMemo, useState } from "react";
import { ReviewingService } from '../../Services/ReviewingService'
import { FilterOptionsContext } from '../../Contexts/FilterOptionsContext';
import { SortOptionsContext } from '../../Contexts/SortOptionsContext';
import ReviewCard from "./ReviewCard";
import { MDBCheckbox, MDBTable, MDBTableBody, MDBTableHead } from "mdb-react-ui-kit";
import ReviewTr from "./ReviewTr";
import { useTranslation } from "react-i18next";

function ReviewList({ table = false }) {

    const pageSize = 10; // TODO: might get it from params or env
    const reviewingService = useMemo(() => new ReviewingService(), []);
    const [isFirstRender, setIsFirstRender] = useState(true);
    const [dataLoading, setDataLoading] = useState(false);
    const [currentPage, setCurrentPage] = useState(0);
    const [reviewsDesc, setReviewsDesc] = useState([]);
    const [scrollTop, setScrollTop] = useState(0);
    const [reviewsCount, setReviewsCount] = useState(0);
    const { filterOptions, valid } = useContext(FilterOptionsContext);
    const [isValid, setIsValid] = useState(false);
    const { sortOptions } = useContext(SortOptionsContext);
    const defaultSortOptions = [
        { name: "publishedDate", value: "desc" }
    ]
    const ns = "reviews";
    const { t, i18n } = useTranslation(ns);
    const [pageLoadingStage, setPageLoadingStage] = useState(true);

    /* eslint-disable */
    useEffect(() => {
        if (isFirstRender)
            return;

        setIsValid(valid);
    }, [valid, isFirstRender]);

    const getCount = () => {
        reviewingService
            .getCount(
                (filterOptions.filter(f => f.name !== 'tags').length !== 0) ? filterOptions.filter(f => f.name !== "tags") : null,
                (filterOptions.filter(f => f.name === 'tags').length !== 0) ? filterOptions.find(f => f.name === "tags")?.value : null)
            .then(count => {
                setReviewsCount(count);
            });
    };

    useEffect(() => {
        if (isFirstRender) {
            return;
        }

        if (Array.isArray(filterOptions) && isValid) {
            setDataLoading(true);
            reviewingService.getShortReviewsDescriptions(
                pageSize, 
                currentPage, 
                (sortOptions && sortOptions.length !== 0) ? sortOptions : defaultSortOptions, 
                (filterOptions.filter(f => f.name !== 'tags').length !== 0) ? filterOptions.filter(f => f.name !== "tags") : null, 
                (filterOptions.filter(f => f.name === 'tags').length !== 0) ? filterOptions.find(f => f.name === "tags")?.value : null,
                !table)
            .then((reviews) => {
                setReviewsDesc((current) => [...current, ...(reviews
                    .filter((review) => !current.map((curr) => curr.id).includes(review.id)))]);
                setDataLoading(false);
                getCount();
            })
        }

    }, [currentPage]);

    useEffect(() => {
        if (isFirstRender) {
            setIsFirstRender(false);
            return;
        }

        if (Array.isArray(filterOptions) && isValid) {
            setDataLoading(true);
            setCurrentPage(0);

            reviewingService.getShortReviewsDescriptions(
                pageSize, 
                0, 
                (sortOptions && sortOptions.length !== 0) ? sortOptions : defaultSortOptions, 
                (filterOptions.filter(f => f.name !== 'tags').length !== 0) ? filterOptions.filter(f => f.name !== "tags") : null,
                (filterOptions.filter(f => f.name === 'tags').length !== 0) ? filterOptions.find(f => f.name === "tags")?.value : null,
                !table)
            .then((reviews) => {
                setReviewsDesc(reviews);
                setDataLoading(false);
                getCount();
            })
        }
    }, [filterOptions, sortOptions, isValid]);

    useEffect(() => {
        const clientHeight = document.documentElement.clientHeight;
        const scrollHeight = (document.documentElement && document.documentElement.scrollHeight) || document.body.scrollHeight;
        const scrolledToBottom = (Math.ceil(scrollTop + clientHeight) >= (scrollHeight / reviewsDesc?.length * (reviewsDesc?.length - pageSize/2)));
        if (scrolledToBottom && !dataLoading && (reviewsCount > reviewsDesc?.length))
            loadMore();
    }, [scrollTop]);

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

    const loadMore = () => {
        const page = Math.floor(reviewsDesc?.length / pageSize)
        setCurrentPage(page);
    }

    const handleOnScroll = () => {
        const height = (document.documentElement && document.documentElement.scrollTop) || document.body.scrollTop;
        setScrollTop(height);
    }

    useEffect(() => {
        window.addEventListener('scroll', handleOnScroll);

        return () => {
            window.removeEventListener('scroll', handleOnScroll);
        }
    }, [])

    return pageLoadingStage ? '' :
    <div id="review-list" className="pt-3">
        <div className="w-100"> 
            {reviewsDesc && reviewsDesc.length > 0 ? (
                !table ?
                <div className="d-flex align-items-start flex-column">
                    {reviewsDesc.map((reviewDesc) => <div key={reviewDesc.id} className="w-100"><ReviewCard reviewDesc={reviewDesc}/></div>)}
                </div> :
                <div className="w-100">
                    <MDBTable responsive className='caption-top'>
                        <caption>{t('review_list_count')}: {reviewsDesc?.length ?? 0}/{reviewsCount ?? 0}</caption>
                        <MDBTableHead>
                            {/* 
                            <MDBIcon className='me-1' fas icon='caret-up' /> 
                            <MDBIcon className='me-1' fas icon='caret-down' />
                            */}
                            <tr>
                                <th><MDBCheckbox id="check-all" /></th>
                                <th style={{minWidth: '250px'}}>{t('review_list_review_name')}</th>
                                <th className="th-sm">{t('review_list_status')}</th>
                                <th className="th-lg text-nowrap">{t('review_list_publish_date')}</th>
                                <th className="th-sm">{t('review_list_likes')}</th>
                                <th className="th-sm">{t('review_list_tags')}</th>
                            </tr>
                        </MDBTableHead>
                        <MDBTableBody>
                            {reviewsDesc.map((reviewDesc) => {
                                return <ReviewTr key={reviewDesc.id} reviewDesc={reviewDesc}/>
                            })}
                        </MDBTableBody>
                    </MDBTable>
                </div>
            ) :
            (
                dataLoading ? <div className="w-100 text-center">Loading...</div> : <div className="w-100 text-center">No reviews yet</div>
            )}
        </div>
    </div>
}

export default ReviewList;