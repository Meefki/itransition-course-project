import { useContext, useEffect, useMemo, useState } from "react";
import HrStyle from "../../Assets/Css/hr";
import { ReviewingService } from "../../Services/ReviewingService";
import { FilterOptionsContext } from "../../Contexts/FilterOptionsContext";

function PopularTags() {
    const [scrollDirection, setScrollDirection] = useState(true);
    const [scrollTop, setScrollTop] = useState(0);
    const [lastScrollTop, setLastScrollTop] = useState(0);
    const headerOffset = -document.getElementById('header')?.clientHeight ?? 0;

    const { setFilterOptions } = useContext(FilterOptionsContext);
    const reviewingService = useMemo(() => new ReviewingService(), []);
    const [tags, setTags] = useState([]);
    const pageSize = 20;

    const handleOnScroll = () => {
        const height = (document.documentElement && document.documentElement.scrollTop) || document.body.scrollTop;
        setScrollTop(height);
    }

    const styles = {
        header: {
            transition: 'top .1s ease', 
            top: scrollDirection ? `0px` : `${document.getElementById('header')?.clientHeight ?? 0}px`,
            position:'sticky', 
            zIndex: '998'
        },
        offset: {
            height: `${-headerOffset}px`,
            top: `${headerOffset}px`,
            position: 'absolute',
        }
    }

    const handleTagClick = (e) => {
        let tbb = [...document.getElementsByClassName("tbb")];

        if ([...e.target.parentNode.classList].find(c => c === "tbb")) {
            Array.isArray(tbb) && tbb?.length !== 0 && tbb.forEach(t => t.classList.remove("tbb"));
            setFilterOptions(current => {
                let filter = [...current].filter(f => f.name !== "tags");
                return filter ?? [];
            });
        }
        else {
            Array.isArray(tbb) && tbb?.length !== 0 && tbb.forEach(t => t.classList.remove("tbb"));
            e.target.parentNode.classList.add("tbb");
            setFilterOptions(current => {
                let filter = [...current];
                const obj = filter?.find((o, i) => {
                    if (o.name === 'tags') {
                        filter[i] = { name: 'tags', value: [e.target.textContent] }
                        return true;
                    }
                    return false;
                })
                if (!obj) {
                    filter.push({ name: 'tags', value: [e.target.textContent] });
                }
                return filter;
            });
        }
    }

    const handleNavClick = (direction) => {
        const rightOffser = 34;
        const steps = 7;
        const d = direction === 'left';
        const items = document.getElementById("scroller-items");
        const tagHeader = document.getElementById("tag-header");
        if (!items || !tagHeader) return;
        let moveOn = (items.clientWidth - tagHeader.clientWidth + rightOffser) / steps;
        const leftOffset = parseFloat(items.style.marginLeft.length < 3 ? '0' : items.style.marginLeft.substring(0, items.style.marginLeft.length - 2));
        if (d) {
            if (-leftOffset < moveOn) {
                items.style.marginLeft = '0px';
                return;
            } 
        } else {
            if (-leftOffset >= moveOn * (steps === 0 ? steps : steps - 1)) {
                items.style.marginLeft = `-${items.clientWidth - tagHeader.clientWidth + rightOffser}px`;
                return;
            } 
            moveOn = -moveOn;
        }
        items.style.marginLeft = `${leftOffset + moveOn}px`;
    }

    /* eslint-disable */
    useEffect(() => {
        setScrollDirection(scrollTop >= lastScrollTop);
        setLastScrollTop(scrollTop <= 0 ? 0 : scrollTop);
    }, [scrollTop])
    /* eslint-enable */

    useEffect(() => {
        window.addEventListener('scroll', handleOnScroll);
        reviewingService.getMostPopularTags(pageSize)
            .then((tags) => {
                setTags(tags);
            })

        return () => {
            window.removeEventListener('scroll', handleOnScroll);
        }
    }, [])

    return (
        <div id="tag-header" className="header d-flex" style={styles.header}>
            <div className="header w-100" style={styles.offset}></div>
            <div className="d-flex justify-content-center flex-column w-100">
                <div>
                    <div style={{height: '16px'}}></div>
                    <div style={{height: '39px', position: 'relative'}}>
                        <div className="zero-scroll py-1 d-flex align-items-center overflow-x-scroll overflow-y-visible">
                            <div id="scroller-items" className="d-flex flex-row">
                                <div className="me-4" style={{width: '34px'}}/>
                                {
                                    tags.map(t =>
                                        <div key={t.Name} className="pb-2 me-4" style={{minWidth: 'max-content'}}>
                                            <div className="d-block text-center" role="button" onClick={(e) => handleTagClick(e)} style={{minWidth: '50px'}}>
                                                {t.Name}
                                            </div>
                                        </div>
                                    )
                                }
                            </div>
                            <div className="ps-5 d-felx align-items-center anr">
                                <button
                                    className="mx-1 p-0"
                                    aria-label="next sections"
                                    tabIndex="0"
                                    style={{
                                        pointerEvents: 'all',
                                        border: 'none',
                                        cursor: 'pointer',
                                        background: 'transparent',
                                        overflow: 'visible'
                                    }}
                                    onClick={() => handleNavClick("right")}>
                                    <div style={{width:"26px", height:"26px"}} className="ar"></div>
                                </button>
                            </div>
                            <div className="pe-5 flex align-items-center anl">
                                <button
                                    className="mx-1 p-0"
                                    aria-label="previous sections"
                                    tabIndex="0"
                                    style={{
                                        pointerEvents: 'all',
                                        border: 'none',
                                        cursor: 'pointer',
                                        background: 'transparent',
                                        overflow: 'visible'
                                    }}
                                    onClick={() => handleNavClick("left")}>
                                    <div style={{width:"26px", height:"26px"}} className="al"></div>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
                <div style={{...HrStyle.horizontalHrStyle, ...{zIndex: '999'}}}/>
            </div>
        </div>
    );
}

export default PopularTags;
