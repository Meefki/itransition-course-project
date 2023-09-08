import { useParams } from "react-router-dom";

function Review() {
    const { id } = useParams();

    return(
        <div>Review {id}</div>
    );
}

export default Review;