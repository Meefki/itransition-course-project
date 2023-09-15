import { useContext, useEffect, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import { FilterOptionsContext } from "../../Contexts/FilterOptionsContext";
import TagInput from "./TagInput";
import { Tag } from "antd";
import { ReviewingService } from "../../Services/ReviewingService";

const styles = {
    a: {
        cursor: 'default',
        userSelect: 'none'
    },
    tagsArea: {
        height: '14rem'
    }
}

function ReviewsFilter({ immutableFilters = [] }) {
    const { filterOptions, setFilterOptions } = useContext(FilterOptionsContext);
    const immutableFilterOptions = immutableFilters?.filter(ifo => filterOptions?.filter(f => f.name === ifo) ? true : false)?.map((option) => { 
        return { 
            name: option,
            value: filterOptions?.find((o) => o.name === option)
        }
    });

    const reviewingService = useMemo(() => new ReviewingService(), []);

    // translation
    const ns = "reviews";
    const { t, i18n } = useTranslation(ns);
    const [pageLoadingStage, setPageLoadingStage] = useState(true);
    /* eslint-disable */
    useMemo(() => {
        i18n.isInitialized &&
        !i18n.hasLoadedNamespace(ns) && 
            i18n.loadNamespaces(ns)
            .then(() => {
                setPageLoadingStage(false);
            });
        i18n.isInitialized &&
        i18n.hasLoadedNamespace(ns) &&
            setPageLoadingStage(false);
    }, [i18n.isInitialized]);
    /* eslint-enable */
    
    // tags
    const [tags, setTags] = useState([]);
    const [selectedTags, setSelectedTags] = useState([]);
    const setTagsFilter = (selTags) => {
        setSelectedTags(selTags);
        if (!immutableFilterOptions.includes('tags'))
            setFilterOptions(current => {
                let filter = [...current]; 
                const obj = filter?.find((o, i) => {
                    if (o.name === 'tags') {
                        filter[i] = { name: 'tags', value: selTags }
                        return true;
                    }
                    return false;
                })
                if (!obj) {
                    filter.push({ name: 'tags', value: selTags });
                }
                return filter;
            });
    }
    const addTag = (key) => {
        if (!selectedTags.find(tagKey => tagKey === key))
        {
            let selTags = [...selectedTags];
            selTags.push(key);
            selTags.sort();
            setTagsFilter(selTags);
        }
    }

    const removeTag = (key) => {
        const selTags = selectedTags.filter(t => t !== key.tag);
        setTagsFilter(selTags);
    }

    const getTags = (startWith, pageSize = 20, pageNumber = 0) => {
        reviewingService.getTags(startWith, pageSize, pageNumber)
        .then((tags) => {
            setTags(tags ?? []);
        });
    }
    const filterTags = () => {
        return tags?.filter(tag => !selectedTags.includes(tag));
    }

    /* eslint-disable */
    useEffect(() => {
        getTags("");

        return () => {
            setFilterOptions(current => current?.filter(f => f.name !== 'tags'))
        }
    }, [])
    /* eslint-enable */

    useEffect(() => {
        setSelectedTags(filterOptions?.find(f => f.name === 'tags')?.value ?? []);
    }, [filterOptions])

    // render
    return pageLoadingStage ? '' :
        <div className="d-flex flex-column">
            <h5>{t('filter_header')}</h5>
            <div className="d-flex flex-column flex-md-row">
                <div className="card mb-3 col-12 col-sm-6 col-md-4">
                    <TagInput tags={filterTags()} getTags={getTags} addTag={addTag} inputPlaceholder={t('filter_tags_input_placeholder')}/>
                    <div style={styles.tagsArea} className="overflow-auto">
                    {
                        selectedTags.map((tag) => 
                        <span
                            style={styles.a}
                            className="text-decoration-none"
                            key={tag}
                            onClick={() => removeTag({tag})}>
                                <Tag className="m-1" color="#55acee">
                                    {tag}
                                </Tag>
                        </span>)
                    }
                    </div>
                </div>
            </div>
            {/* dropdown with tags */}
            {/* dropdown with existing subjects */}
            {/* publish date */}
            {/*  */}
        </div>
}

export default ReviewsFilter;