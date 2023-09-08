import { useEffect, useMemo, useState } from "react";
import { ReviewingService } from '../../Services/ReviewingService'
import ReviewCard from "./ReviewCard";
import HrStyle from '../../Assets/Css/hr'

function ReviewList({ filters }) {

    const pageSize = 10; // TODO: might get it from params or env
    const reviewingService = useMemo(() => new ReviewingService(), []);
    const [dataLoading, setDataLoading] = useState(true);
    const [currentPage, setCurrentPage] = useState(0);
    const [reviewsDesc, setReviewsDesc] = useState([]);
    const [scrollTop, setScrollTop] = useState(0);
    
    useEffect(() => {
        setDataLoading(true);
        const sortOptions = [
            { name: "publishedDate", value: "desc" },
            { name: "name", value: "asc" }
        ]
        reviewingService.getShortReviewsDescriptions(pageSize, currentPage, sortOptions)
            .then((reviews) => {
                setReviewsDesc((current) => [...current, ...(reviews
                    .filter((review) => !current.map((curr) => curr.id).includes(review.id))
                    .sort((left, right) => left.publishedDate - right.publishedDate))]);
                setDataLoading(false);
            })
    }, [currentPage, reviewingService]);
    const [reviewsCount, setReviewsCount] = useState(0);

    const loadMore = () => {
        const page = Math.floor(reviewsDesc.length / pageSize)
        setCurrentPage(page);
    }

    useEffect(() => {
        reviewingService.getCount().then(count => {
            setReviewsCount(count);
        });
    }, [reviewsDesc, reviewingService])

    const handleOnScroll = () => {
        const height = (document.documentElement && document.documentElement.scrollTop) || document.body.scrollTop;
        setScrollTop(height);
    }

    /* eslint-disable */
    useEffect(() => {
        const clientHeight = document.documentElement.clientHeight;
        const scrollHeight = (document.documentElement && document.documentElement.scrollHeight) || document.body.scrollHeight;
        const scrolledToBottom = (Math.ceil(scrollTop + clientHeight) >= (scrollHeight / reviewsDesc?.length * (reviewsDesc?.length - pageSize/2)));
        if (scrolledToBottom && !dataLoading && (reviewsCount > reviewsDesc?.length))
            loadMore();
    }, [scrollTop]);
    /* eslint-enable */

    useEffect(() => {
        window.addEventListener('scroll', handleOnScroll);

        return () => {
            window.removeEventListener('scroll', handleOnScroll);
        }
    }, [])

    return(
        <div>
            <div id="review-list" className="d-flex flex-row">
                <div className="pe-5">
                    {reviewsDesc && reviewsDesc.length > 0 ? (
                        <div className="d-flex align-items-start flex-column">
                            {reviewsDesc.map((reviewDesc) => <div key={reviewDesc.id} className="w-100"><ReviewCard reviewDesc={reviewDesc}>{JSON.stringify(reviewDesc)}</ReviewCard></div>)}
                        </div>
                    ) :
                    (
                        dataLoading ? <div>Loading...</div> : <div>No reviews yet</div>
                    )}
                </div>
                {(!dataLoading || reviewsDesc?.length !== 0) && <div className="d-none d-lg-block d-xl-block" style={HrStyle.verticalHrStyle}/>}  
            </div>
        </div>
    )
}

export default ReviewList;