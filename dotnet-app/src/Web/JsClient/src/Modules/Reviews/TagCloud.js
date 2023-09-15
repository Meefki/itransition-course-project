import { TagCloud } from "react-tagcloud";
import { ReviewingService } from "../../Services/ReviewingService";
import { useContext, useEffect, useMemo, useState } from "react";
import { FilterOptionsContext } from "../../Contexts/FilterOptionsContext";
import HrStyle from "../../Assets/Css/hr";

function TagsCloud() {
    const reviewingService = useMemo(() => new ReviewingService(), []);
    const [tags, setTags] = useState([]);

    const { setFilterOptions } = useContext(FilterOptionsContext);

    /* eslint-disable */
    useEffect(() => {
        reviewingService.getMostPopularTags(50)
            .then((tags) => {
                setTags(tags);
            });

        return () => {
            setFilterOptions(current => {
                let filter = [...current]?.filter(f => f.name !== 'tags');
                return filter;
            })
        };
    }, [])
    /* eslint-enable */

    const customRender = (tag, size, color) => {
        return(
            <span 
                key={tag.value}
                title={tag.count}
                role="button" 
                style={{
                    fontSize: size,
                    display: 'inline-block',
                    verticalAlign: 'middle',
                    color
                }} 
                className="mx-1 tag-cloud-tag user-select-none">
                {tag.value}
            </span>
        )
    }

    const handleClick = (t) => {
        setFilterOptions(current => {
            let filter = [...current];
            let removeTags = false;
            const obj = filter?.find((o, i) => {
                if (o.name === 'tags') {
                    if (Array.isArray(o.value) && o.value.length === 1 && o.value[0] === t) removeTags = true;
                    filter[i] = { name: 'tags', value: [t] }
                    return true;
                }
                return false;
            })
            if (!obj) {
                filter.push({ name: 'tags', value: [t] });
            }
            if (removeTags) return filter?.filter(f => f.name !== 'tags');
            return filter;
        });
    }

    return(
        <div className="p-3">
            <TagCloud 
                minSize={14}
                maxSize={35}
                tags={tags}
                renderer={customRender}
                onClick={t => handleClick(t.value)}/>
            <div className="mt-1" style={HrStyle.horizontalHrStyle}/>
        </div>
    )
}

export default TagsCloud;