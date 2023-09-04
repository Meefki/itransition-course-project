import React from "react";
import {
    
} from 'mdb-react-ui-kit'

function Header() {
    const imgSize = '50px';

    return(
        <div id="header" className="header pt-2 pb-2 border-bottom-2 shadow">
            <div className="d-flex flex-row">
                <img 
                    style={{width: imgSize, height: imgSize}}
                    src="https://placehold.co/50x50"
                    className="rounded-circle ms-3"/>
                <nav className="w-100 d-flex align-items-center justify-content-between ms-4 me-4">
                    <ul className="m-0 p-0">
                        <li className="d-inline me-5">Categories</li>
                        <li className="d-inline me-5">Categories</li>
                        <li className="d-inline me-5">Categories</li>
                    </ul>
                    <ul className="m-0 p-0">
                        <li className="d-inline">Profile</li>
                    </ul>
                </nav>
            </div>
        </div>
    )
}

export default Header;