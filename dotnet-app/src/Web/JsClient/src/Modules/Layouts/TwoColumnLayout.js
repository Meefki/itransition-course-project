import React, { useEffect, useState } from "react";
import { MDBContainer } from "mdb-react-ui-kit";
import HrStyle from '../../Assets/Css/hr'

function TwoColumnLayout({mainComponents, sideComponents, hideSecondCol = true}) {
    
    const [scrollTop, setScrollTop] = useState(0);
    const [lastScrollTop, setLastScrollTop] = useState(0);
    const [lastScrollDirection, setLastScrollDirection] = useState(true);
    const [headerHeight, setHeaderHeight] = useState(67);

    const [carouselPos, setCarouselPos] = useState('sticky');
    const [carouselTop, setCarouselTop] = useState(0);
    const [carouselMgTop, setCarouselMgTop] = useState(0);
    const error = 25;

    const colClasses = {
        hide: {
            first: "col-12 col-lg-7 col-xl-8 col-xxl-8 d-flex flex-row",
            second: "col-lg-5 col-xl-4 col-xxl-4 d-none d-lg-block",
            parent: "d-flex justify-content-center"
        },
        show: {
            first: "col-12 col-lg-7 col-xl-8 col-xxl-8 d-flex flex-row",
            second: "col-lg-5 col-xl-4 col-xxl-4 d-block",
            parent: "d-flex justify-content-center flex-column-reverse flex-lg-row"
        }
    }

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
                    setCarouselMgTop(scrollTop);
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
                    setCarouselMgTop(scrollTop);
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
        setHeaderHeight(current => document.getElementById('header')?.clientHeight ?? current);

        return () => {
            window.removeEventListener('scroll', handleOnScroll);
        }
    }, [])

    return(
        <div>
            <MDBContainer className={hideSecondCol ? colClasses.hide.parent : colClasses.show.parent}> 
            {/* "d-flex justify-content-center" */}
                <div className={hideSecondCol ? colClasses.hide.first : colClasses.show.first} style={{minHeight: `calc(100vh - ${document.getElementById('header')?.clientHeight}px)`}}>
                {/* "col-12 col-lg-7 col-xl-8 col-xxl-8 d-flex flex-row" */}
                    <div className="mt-5 flex-fill">
                        {mainComponents && mainComponents?.map((c, index) => <div key={index}>{c}</div>)}
                    </div>
                    <div className="d-none d-lg-block" style={HrStyle.verticalHrStyle}/>
                </div>
                <div className={hideSecondCol ? colClasses.hide.second : colClasses.show.second}>
                {/* "col-lg-5 col-xl-4 col-xxl-4 d-none d-lg-block" */}
                    <div id="second-column" className="" style={document.documentElement.clientWidth < 992 ? {} :
                        {
                            position: carouselPos,
                            marginTop: `${carouselMgTop}px`,
                            top: `${carouselTop}px`,
                            minHeight: `calc(100vh - ${headerHeight}px)`
                        }}>
                        <div className={lastScrollDirection ? 'mt-5' : 'pt-5'}>
                            <div>
                                {sideComponents && sideComponents?.map((c, index) => 
                                    <div key={index}>
                                        {c}
                                    </div>)}
                            </div>
                        </div>
                    </div>
                </div>
            </MDBContainer>
        </div>
    );
}

export default TwoColumnLayout;