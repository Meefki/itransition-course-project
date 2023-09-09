import { useEffect, useMemo, useState } from "react";
import { ReviewingService } from '../../Services/ReviewingService'
import ReviewCard from "./ReviewCard";

function ReviewList({ sort, filters, isSelection = false }) {

    const pageSize = 10; // TODO: might get it from params or env
    const reviewingService = useMemo(() => new ReviewingService(), []);
    const [dataLoading, setDataLoading] = useState(true);
    const [currentPage, setCurrentPage] = useState(0);
    const [reviewsDesc, setReviewsDesc] = useState([]);
    const [scrollTop, setScrollTop] = useState(0);
    const [reviewsCount, setReviewsCount] = useState(0);
    const [filterOptions, setFilterOptions] = useState(null);
    
    /* eslint-disable */
    useEffect(() => {
        setDataLoading(true);
        const filterOpts = Object.getOwnPropertyNames(filters)?.length === 0 ? null :
            Object.getOwnPropertyNames(filters)?.map((key) => {
                return { name: key, value: filters[key] }
            })
        setFilterOptions(filterOpts);

        reviewingService.getShortReviewsDescriptions(pageSize, currentPage, sort, filterOpts?.filter((option) => option.name !== "tags"), filterOpts?.tags)
            .then((reviews) => {
                setReviewsDesc((current) => [...current, ...(reviews
                    .filter((review) => !current.map((curr) => curr.id).includes(review.id))
                    .sort((left, right) => left.publishedDate - right.publishedDate))]);
                setDataLoading(false);
            })
    }, [currentPage, reviewingService]);

    useEffect(() => {
        if (!((reviewsDesc?.length ?? 0) > 0))
            return;

        const filretOpts = filterOptions ? filterOptions : 
        Object.getOwnPropertyNames(filters)?.length === 0 ? null :
            Object.getOwnPropertyNames(filters)?.map((key) => {
                return { name: key, value: filters[key] }
            })

        reviewingService
            .getCount(filretOpts?.filter(opt => opt.name !== 'tags'), filretOpts?.tags)
            .then(count => {
                setReviewsCount(count);
            });
    }, [reviewsDesc, reviewingService])

    useEffect(() => {
        const clientHeight = document.documentElement.clientHeight;
        const scrollHeight = (document.documentElement && document.documentElement.scrollHeight) || document.body.scrollHeight;
        const scrolledToBottom = (Math.ceil(scrollTop + clientHeight) >= (scrollHeight / reviewsDesc?.length * (reviewsDesc?.length - pageSize/2)));
        if (scrolledToBottom && !dataLoading && (reviewsCount > reviewsDesc?.length))
            loadMore();
    }, [scrollTop]);
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

    return(
        <div className="w-100">
            <div id="review-list" className="d-flex flex-row">
                <div className="pe-0 pe-lg-5 w-100">
                    {reviewsDesc && reviewsDesc.length > 0 ? (
                        <div className="d-flex align-items-start flex-column">
                            {!isSelection && reviewsDesc.map((reviewDesc) => <div key={reviewDesc.id} className="w-100"><ReviewCard reviewDesc={reviewDesc}/></div>)}
                            {isSelection && reviewsDesc.map((reviewDesc) => {
                                return( 
                                    <div key={reviewDesc.id} className="w-100">
                                        <ReviewCard reviewDesc={reviewDesc}/>
                                    </div>
                                )
                            })}
                        </div>
                    ) :
                    (
                        dataLoading ? <div className="w-100 text-center">Loading...</div> : <div className="w-100 text-center">No reviews yet</div>
                    )}
                </div>
            </div>
        </div>
    )
}

export default ReviewList;