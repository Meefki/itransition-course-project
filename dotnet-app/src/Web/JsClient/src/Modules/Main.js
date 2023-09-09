import React, { useEffect, useState } from "react";
import { MDBContainer } from "mdb-react-ui-kit";
import ReviewList from "./Reviews/ReviewList";
import ReviewCarousel from './Reviews/ReviewCarousel'
import HrStyle from '../Assets/Css/hr'

export function Main() {
    
    const [scrollTop, setScrollTop] = useState(0);
    const [lastScrollTop, setLastScrollTop] = useState(0);
    const [lastScrollDirection, setLastScrollDirection] = useState(true);
    const [headerHeight, setHeaderHeight] = useState(0);
    const sortOptions = [
        { name: "publishedDate", value: "desc" },
        { name: "name", value: "asc" }
    ]

    const [carouselPos, setCarouselPos] = useState('sticky');
    const [carouselTop, setCarouselTop] = useState(0);
    const [carouselMgTop, setCarouselMgTop] = useState(0);
    const error = 25;

    const handleOnScroll = () => {
        const height = (document.documentElement && document.documentElement.scrollTop) || document.body.scrollTop;
        setScrollTop(height);
    }

    /* eslint-disable */
    useEffect(() => {
        const scrollDirection = scrollTop >= lastScrollTop;
        setLastScrollTop(scrollTop <= 0 ? 0 : scrollTop);

        const col = document.getElementById('second-column');
        const header = document.getElementById('header');
        const doc = document.documentElement;

        if (col)
        if (scrollDirection) {
            if (scrollDirection === lastScrollDirection) {
                if (carouselPos === 'relative' && carouselMgTop + col.clientHeight - doc?.clientHeight <= scrollTop) {
                    setCarouselPos('sticky');
                    setCarouselTop(doc?.clientHeight - col.clientHeight);
                    setCarouselMgTop('0px');
                }
                if (carouselPos === 'sticky' && (col.clientHeight - doc?.clientHeight - error) > scrollTop) {
                    setCarouselPos('relative');
                    setCarouselMgTop(scrollTop); // + (header?.clientHeight/2 ?? 0)
                    setCarouselTop(0);
                }
            } else {
                if (carouselPos === 'relative') {
                    if (carouselMgTop + col.clientHeight - doc?.clientHeight - error <= scrollTop) {
                        setCarouselPos('sticky');
                        setCarouselTop(doc?.clientHeight - col.clientHeight);
                        setCarouselMgTop(0);
                    }
                } else {
                    setCarouselPos('relative');
                    setCarouselMgTop(scrollTop); // + (header?.clientHeight/2 ?? 0)
                    setCarouselTop(0);
                }
            }
        } else {
            if (scrollDirection === lastScrollDirection) {
                if (carouselPos === 'relative' && (carouselMgTop + error) >= scrollTop) {
                    setCarouselPos('sticky');
                    setCarouselTop(header?.clientHeight);
                    setCarouselMgTop(0);
                }
            } else {
                if (carouselPos === 'sticky') {
                    setCarouselPos('relative');
                    setCarouselMgTop(scrollTop + doc?.clientHeight - header?.clientHeight - col.clientHeight);
                    setCarouselTop(0);
                } else {
                    if (carouselMgTop >= scrollTop) {
                        setCarouselPos('sticky');
                        setCarouselTop(header?.clientHeight);
                        setCarouselMgTop(0);
                    }
                }
            }
        }

        setLastScrollDirection(scrollDirection);
            
    }, [scrollTop])
    /* eslint-enable */

    useEffect(() => {
        window.addEventListener('scroll', handleOnScroll);
        setHeaderHeight(document.getElementById('header')?.clientHeight ?? 0);

        return () => {
            window.removeEventListener('scroll', handleOnScroll);
        }
    }, [])

    return(
        <div>
            <MDBContainer className="d-flex justify-content-center">
                <div className="mt-5 col-12 col-lg-7 col-xl-8 col-xxl-8 d-flex" style={{minHeight: `calc(100vh - ${lastScrollDirection ? '0px' : headerHeight})`}}>
                    <ReviewList sort={sortOptions} filters={{}}/>
                    <div className="d-none d-lg-block" style={HrStyle.verticalHrStyle}/>
                </div>
                <div className="col-lg-5 col-xl-4 col-xxl-4 d-none d-lg-block">
                    <div id="second-column" className="" style={
                        {
                            // position: 'sticky', 
                            // marginTop: '0px', 
                            position: carouselPos,
                            marginTop: `${carouselMgTop}px`,
                            top: `${carouselTop}px`,
                            // transition: '.15s linear'
                        }}>
                        <div className="mt-5" style={{minHeight: `calc(100vh - ${lastScrollDirection ? '0px' : headerHeight})`}}>
                            <div style={{minHeight: `calc(100vh - ${lastScrollDirection ? '0px' : headerHeight})`}}>
                                <ReviewCarousel />
                                <ReviewCarousel />
                                <ReviewCarousel />
                            </div>
                        </div>
                    </div>
                </div>
            </MDBContainer>
        </div>
        
    );
}