import { useContext, useEffect, useMemo, useState } from "react";
import { ReviewingService } from '../../Services/ReviewingService'
import { FilterOptionsContext } from '../../Contexts/FilterOptionsContext';
import { SortOptionsContext } from '../../Contexts/SortOptionsContext';
import ReviewCard from "./ReviewCard";
import { MDBCheckbox, MDBTable, MDBTableBody, MDBTableHead } from "mdb-react-ui-kit";
import ReviewTr from "./ReviewTr";

function ReviewList({ table = false }) {

    const pageSize = 10; // TODO: might get it from params or env
    const reviewingService = useMemo(() => new ReviewingService(), []);
    const [dataLoading, setDataLoading] = useState(true);
    const [currentPage, setCurrentPage] = useState(0);
    const [reviewsDesc, setReviewsDesc] = useState([]);
    const [scrollTop, setScrollTop] = useState(0);
    const [reviewsCount, setReviewsCount] = useState(0);
    const { filterOptions } = useContext(FilterOptionsContext);
    const { sortOptions, setSortOptions } = useContext(SortOptionsContext);
    const defaultSortOptions = [
        { name: "publishedDate", value: "desc" }
    ]

    /* eslint-disable */
    useEffect(() => {
        setDataLoading(true);
        let needUseDefaultSort = false;
        const sortPropNames = Object.getOwnPropertyNames(sortOptions ?? {})
        if (sortPropNames.length === 0)
            needUseDefaultSort = true;
        needUseDefaultSort && setSortOptions(defaultSortOptions);

        reviewingService.getShortReviewsDescriptions(pageSize, currentPage, needUseDefaultSort ? defaultSortOptions : sortOptions, filterOptions?.filter((option) => option.name !== "tags"), filterOptions?.tags)
            .then((reviews) => {
                setReviewsDesc((current) => [...current, ...(reviews
                    .filter((review) => !current.map((curr) => curr.id).includes(review.id)))]);
                setDataLoading(false);
            })
    }, [filterOptions, sortOptions, currentPage, reviewingService]);

    useEffect(() => {
        if (!((reviewsDesc?.length ?? 0) > 0))
            return;

        reviewingService
            .getCount(filterOptions?.filter(opt => opt.name !== 'tags'), filterOptions?.tags)
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
                            {!table && reviewsDesc.map((reviewDesc) => <div key={reviewDesc.id} className="w-100"><ReviewCard reviewDesc={reviewDesc}/></div>)}
                            {table && 
                            <MDBTable>
                                <MDBTableHead>
                                    <tr>
                                        <th><MDBCheckbox id="check-al" /></th>
                                        <th>Review Name:</th>
                                        <th>Status:</th>
                                        <th>Publish Date:</th>
                                        <th>Likes:</th>
                                    </tr>
                                </MDBTableHead>
                                <MDBTableBody>
                                    {reviewsDesc.map((reviewDesc) => {
                                        return <ReviewTr key={reviewDesc.id} reviewDesc={reviewDesc}/>
                                    })}
                                </MDBTableBody>
                            </MDBTable>}
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