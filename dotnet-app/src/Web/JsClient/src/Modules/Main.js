import React, { useEffect, useState } from "react";
import { MDBContainer } from "mdb-react-ui-kit";
import ReviewList from "./Reviews/ReviewList";
import ReviewCarousel from './Reviews/ReviewCarousel'
import HrStyle from '../Assets/Css/hr'

export function Main() {
    
    const [scrollTop, setScrollTop] = useState(0);
    const [lastScrollTop, setLastScrollTop] = useState(0);
    const [lastScrollDirection, setLastScrollDirection] = useState(true);

    const handleOnScroll = () => {
        const height = (document.documentElement && document.documentElement.scrollTop) || document.body.scrollTop;
        setScrollTop(height);
    }

    /* eslint-disable */
    useEffect(() => {
        const getFloat = (px) => {
            return parseFloat(px?.replace(/px/, ''));
        }

        const scrollDirection = scrollTop >= lastScrollTop;
        setLastScrollTop(scrollTop <= 0 ? 0 : scrollTop);

        const col = document.getElementById('second-column');
        const header = document.getElementById('header');
        const doc = document.documentElement;

        if (col)
        if (scrollDirection) {
            if (scrollDirection === lastScrollDirection) {
                if (col.style.position === 'relative' && getFloat(col.style.marginTop) + col.clientHeight - doc?.clientHeight <= scrollTop) {
                    col.style.position = 'sticky';
                    col.style.top = `${doc?.clientHeight - col.clientHeight}px`;
                    col.style.marginTop = '0px';
                }
                if (col.style.position === 'sticky' && header?.clientHeight <= scrollTop && (header?.clientHeight + col.clientHeight - doc?.clientHeight) >= scrollTop) {
                    col.style.position = 'relative';
                }
            } else {
                if (col.style.position === 'relative') {
                    if (getFloat(col.style.marginTop) + col.clientHeight >= scrollTop) {
                        col.style.position = 'sticky';
                        col.style.top = `${doc?.clientHeight - col.clientHeight}px`;
                        col.style.marginTop = '0px';
                    }
                } else {
                    col.style.position = 'relative';
                    col.style.marginTop = `${scrollTop}px`;
                    col.style.top = '0px';
                }
            }
        } else {
            if (scrollDirection === lastScrollDirection) {
                if (col.style.position === 'relative' && getFloat(col.style.marginTop) >= scrollTop) {
                    col.style.position = 'sticky';
                    col.style.top = `${header?.clientHeight}px`;
                    col.style.marginTop = '0px';
                }
            } else {
                if (col.style.position === 'sticky') {
                    col.style.position = 'relative';
                    col.style.marginTop = `${scrollTop + doc?.clientHeight - col.clientHeight}px`;
                    col.style.top = '0px';
                } else {
                    if (getFloat(col.style.marginTop) >= scrollTop) {
                        col.style.position = 'sticky';
                        col.style.top = `${header?.clientHeight}px`;
                        col.style.marginTop = '0px';
                    }
                }
            }
        }

        setLastScrollDirection(scrollDirection);
            
    }, [scrollTop])
    /* eslint-enable */

    useEffect(() => {
        window.addEventListener('scroll', handleOnScroll);

        return () => {
            window.removeEventListener('scroll', handleOnScroll);
        }
    }, [])

    return(
        <div>
            <MDBContainer className="d-flex justify-content-start">
                <div className="col-12 col-lg-7 col-xl-8 col-xxl-8 d-flex" style={{minHeight: `calc(100vh - ${lastScrollDirection ? '0px' : '66px'})`}}>
                    <ReviewList/>
                    <div className="d-none d-lg-block" style={HrStyle.verticalHrStyle}/>
                </div>
                <div className="col-lg-5 col-xl-4 col-xxl-4 d-none d-lg-block">
                    <div id="second-column" className="" style={{position: 'sticky', marginTop: '0px'}}>
                        <div className="" style={{minHeight: `calc(100vh - ${lastScrollDirection ? '0px' : '66px'})`}}>
                            <div style={{minHeight: '100vh - 66px'}}>
                                <ReviewCarousel />
                            </div>
                        </div>
                    </div>
                </div>
            </MDBContainer>
        </div>
        
    );
}