import React, { useEffect, useState } from "react";
import { MDBContainer } from "mdb-react-ui-kit";
import HrStyle from '../../Assets/Css/hr'

function TwoColumnLayout({sideComponents, mainComponents, hideSecondCol = true}) {
    
    const [scrollTop, setScrollTop] = useState(0);
    const [lastScrollTop, setLastScrollTop] = useState(0);
    const [lastScrollDirection, setLastScrollDirection] = useState(true);
    const [headerHeight, setHeaderHeight] = useState(67);

    const [secondColPos, setSecondColPos] = useState('sticky');
    const [secondColTop, setSecondColTop] = useState(0);
    const [secondColMgTop, setSecondColMgTop] = useState(0);
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
        //const header = document.getElementById('header');
        const doc = document.documentElement;

        if (col)
        if (scrollDirection) {
            if (scrollDirection === lastScrollDirection) {
                if (secondColPos === 'relative' && secondColMgTop + col.clientHeight - doc?.clientHeight <= scrollTop) {
                    setSecondColPos('sticky');
                    setSecondColTop(doc?.clientHeight - col.clientHeight);
                    setSecondColMgTop('0px');
                }
                if (secondColPos === 'sticky' && (col.clientHeight - doc?.clientHeight - error) > scrollTop) {
                    setSecondColPos('relative');
                    setSecondColMgTop(scrollTop);
                    setSecondColTop(0);
                }
            } else {
                if (secondColPos === 'relative') {
                    if (secondColMgTop + col.clientHeight - doc?.clientHeight - error <= scrollTop) {
                        setSecondColPos('sticky');
                        setSecondColTop(doc?.clientHeight - col.clientHeight);
                        setSecondColMgTop(0);
                    }
                } else {
                    setSecondColPos('relative');
                    setSecondColMgTop(scrollTop);
                    setSecondColTop(0);
                }
            }
        } else {
            if (scrollDirection === lastScrollDirection) {
                if (secondColPos === 'relative' && (secondColMgTop + error) >= scrollTop) {
                    setSecondColPos('sticky');
                    setSecondColTop(headerHeight);
                    setSecondColMgTop(0);
                }
            } else {
                if (secondColPos === 'sticky') {
                    setSecondColPos('relative');
                    setSecondColMgTop(scrollTop + doc?.clientHeight - headerHeight - col.clientHeight);
                    setSecondColTop(0);
                } else {
                    if (secondColMgTop >= scrollTop) {
                        setSecondColPos('sticky');
                        setSecondColTop(headerHeight);
                        setSecondColMgTop(0);
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
                <div className={hideSecondCol ? colClasses.hide.first : colClasses.show.first} style={{minHeight: `calc(100vh - ${document.getElementById('header')?.clientHeight}px)`}}>
                    <div className="mt-5 flex-fill">
                        {mainComponents && mainComponents?.map((c, index) => <div key={index}>{c}</div>)}
                    </div>
                    <div className="d-none d-lg-block" style={HrStyle.verticalHrStyle}/>
                </div>
                <div className={hideSecondCol ? colClasses.hide.second : colClasses.show.second}>
                    <div id="second-column" className="" style={document.documentElement.clientWidth < 992 ? {} :
                        {
                            position: secondColPos,
                            marginTop: `${secondColMgTop}px`,
                            top: `${secondColTop}px`,
                            minHeight: `calc(100vh - ${headerHeight}px)`
                        }}>
                        <div className='pt-5'>
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