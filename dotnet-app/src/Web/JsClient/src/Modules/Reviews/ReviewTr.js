import { MDBCheckbox } from "mdb-react-ui-kit";
import React, { useState } from "react";

function ReviewTr({reviewDesc, check}) {
    const [isCheched, setIsChecked] = useState(false);
    const onCheck = () => {
        setIsChecked(current => !current);
    }

    return(
        <tr className="align-middle">
            <td><MDBCheckbox checked={isCheched} onChange={() => onCheck()} id={"review-checkbox-" + reviewDesc.id} /></td>
            <td>{reviewDesc.name}</td>
            <td>{reviewDesc.status}</td>
            <td>{reviewDesc.publishedDate}</td>
            <td>{reviewDesc.likes ?? 0}</td>
        </tr>
    )
}

export default ReviewTr;