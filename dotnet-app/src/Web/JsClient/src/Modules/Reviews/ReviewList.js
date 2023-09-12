import { useContext, useEffect, useMemo, useState } from "react";
import { ReviewingService } from '../../Services/ReviewingService'
import { FilterOptionsContext } from '../../Contexts/FilterOptionsContext';
import { SortOptionsContext } from '../../Contexts/SortOptionsContext';
import ReviewCard from "./ReviewCard";
import { MDBCheckbox, MDBTable, MDBTableBody, MDBTableHead } from "mdb-react-ui-kit";
import ReviewTr from "./ReviewTr";
// import { UserManagerContext } from "../../Contexts/UserManagerContext";

function ReviewList({ table = false }) {

    const pageSize = 10; // TODO: might get it from params or env
    //const mgr = useContext(UserManagerContext);
    const reviewingService = useMemo(() => new ReviewingService(), []);
    const [isFirstRender, setIsFirstRender] = useState(true);
    const [dataLoading, setDataLoading] = useState(false);
    const [currentPage, setCurrentPage] = useState(0);
    const [reviewsDesc, setReviewsDesc] = useState([]);
    const [scrollTop, setScrollTop] = useState(0);
    const [reviewsCount, setReviewsCount] = useState(0);
    const { filterOptions, valid } = useContext(FilterOptionsContext);
    const [isValid, setIsValid] = useState(false);
    const { sortOptions, /*setSortOptions*/ } = useContext(SortOptionsContext);
    const defaultSortOptions = [
        { name: "publishedDate", value: "desc" }
    ]

    /* eslint-disable */
    useEffect(() => {
        if (isFirstRender)
            return;

        setIsValid(valid);
    }, [valid, isFirstRender]);

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
            })
        }
    }, [filterOptions, sortOptions, isValid]);

    useEffect(() => {
        if (!((reviewsDesc?.length ?? 0) > 0))
            return;

        reviewingService
            .getCount(
                (filterOptions.filter(f => f.name !== 'tags').length !== 0) ? filterOptions.filter(f => f.name !== "tags") : null,
                (filterOptions.filter(f => f.name === 'tags').length !== 0) ? filterOptions.find(f => f.name === "tags")?.value : null)
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
            {/* <div className="d-flex flex-column">
                {filterOptions?.map((f, index) => 
                <div key={index} className="d-flex flex-row justify-content-between">
                    <span>{f?.name}:</span>
                    {Array.isArray(f?.value) ? 
                    <div className="d-flex flex-column">
                        {f?.value?.map((v, index) => <span key={index}>{v}</span>)}
                    </div> :
                    <span>{f?.value}</span>}
                </div>)}
            </div> */}

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
                                        <th className="th-lg">Review Name:</th>
                                        <th className="th-sm">Status:</th>
                                        <th className="th-lg text-nowrap">Publish Date:</th>
                                        <th className="th-sm">Likes:</th>
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