import React, { useEffect, useLayoutEffect, useState } from "react";
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
    const error = 20;

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

    function setSecondColParams(position, top, marginTop) {
        setSecondColPos(position);
        setSecondColTop(top);
        setSecondColMgTop(marginTop);
    }

    /* eslint-disable */
    useLayoutEffect(() => {
        const scrollDirection = scrollTop >= lastScrollTop;
        setLastScrollTop(scrollTop <= 0 ? 0 : scrollTop);

        const col = document.getElementById('second-column');
        const doc = document.documentElement;

        if (col) {
            if (col.clientHeight > doc.clientHeight) {
                col.style.transition = 'none';
                if (scrollDirection) {
                    if (scrollDirection === lastScrollDirection) {
                        if (secondColPos === 'relative' && secondColMgTop + col.clientHeight - doc?.clientHeight <= scrollTop) {
                            setSecondColParams('sticky', doc?.clientHeight - col.clientHeight, 0);
                        }
                        if (secondColPos === 'sticky' && (col.clientHeight - doc?.clientHeight - error) > scrollTop) {
                            setSecondColParams('relative', scrollTop, 0);
                        }
                    } else {
                        if (secondColPos === 'relative') {
                            if (secondColMgTop + col.clientHeight - doc?.clientHeight - error <= scrollTop) {
                                setSecondColParams('sticky', doc?.clientHeight - col.clientHeight, 0);
                            }
                        } else {
                            setSecondColParams('relative', scrollTop, 0);
                        }
                    }
                } else {
                    if (scrollDirection === lastScrollDirection) {
                        if (secondColPos === 'relative' && (secondColMgTop + error) <= scrollTop) {
                            setSecondColParams('sticky', headerHeight, 0);
                        }
                    } else {
                        if (secondColPos === 'sticky') {
                            setSecondColParams('relative', scrollTop + doc?.clientHeight - headerHeight - col.clientHeight, 0);
                        } else {
                            if (secondColMgTop >= scrollTop) {
                                setSecondColParams('sticky', headerHeight, 0);
                            }
                        }
                    }
                }
            } else {
                col.style.transition = 'top .1s ease';
                setSecondColPos('sticky');
                if (scrollDirection) {
                    setSecondColTop(0);
                } else {
                    setSecondColTop(headerHeight);
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
                    <div className="px-0 px-md-4 flex-fill w-100">
                        {mainComponents && mainComponents?.map((c, index) => React.cloneElement(c, {key: index}))}
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
                        <div>
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