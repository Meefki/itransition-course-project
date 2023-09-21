import { useEffect, useMemo, useState } from "react";
import DropdownInput from './DropdownInput';
import { ReviewingService } from "../../Services/ReviewingService";

function SubjectEditor({ subject, setSubject }) {
    const reviewingService = useMemo(() => new ReviewingService(), [])
    const [subjects, setSubjects] = useState([]);
    // const [subject, setSubject] = useState('');

    const getSubjects = (name) => {
        reviewingService.getSubjects(name)
            .then(subjects => {
                setSubjects(subjects ?? []);
            });
    }

    const setSubjectItem = (name) => {
        setSubject(subjects.find(o => o.name === name) ?? {})
    }

    useEffect(() => {
        getSubjects(null);
    }, [])

    return (
    <div>
        <div className="d-flex flex-column">
            <div className="d-flex flex-column mb-3">
                <DropdownInput items={subjects} getItems={getSubjects} addItem={setSubjectItem}/>
            </div>
            {subject &&
            <div className="d-flex flex-column">
                <span className="mb-2">{subject?.name}</span>
                <span className="mb-2">{subject?.groupName}</span>
                {/* <span>{subject?.grade}</span> */}
                <button className="btn btn-danger mb-2" onClick={() => setSubject(null)}>Clear</button>
            </div>}
        </div>
    </div>
    ) 
}

export default SubjectEditor;