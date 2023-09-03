const fetchGet = async (url) => {
    const response = await fetch(url);
    const data = await response.json();
    return data;
}

const getSearch = (params) => {
    const notNullParams = params.filter(param => param.value);

    let searchString = '';
    if (notNullParams && notNullParams.length > 0) {
        searchString = notNullParams.map(param => param.name + '=' + param.value).join('&');
    }

    if (searchString)
        searchString = '?' + searchString;
    else
        searchString = '';

    return searchString
}

export class ReviewingService {

    getTags = async (startWith) => {
        const params = [
            { name: 'startWith', value: startWith }
        ]

        const url = process.env.REACT_APP_REVIEWING_API + '/reviews/tags' + getSearch(params);
        return await fetchGet(url);
    }

    getReview = async (reviewId) => {
        if (!reviewId) return;

        const url = process.env.REACT_APP_REVIEWING_API + '/reviews/' + reviewId;
        return await fetchGet(url);
    }

    getReviewsDescription = async (pageSize, pageNumber) => {
        const params = [
            {name: 'pageSize', value: pageSize },
            {name: 'pageNumber', value: pageNumber }
        ]

        const url = process.env.REACT_APP_REVIEWING_API + '/reviews' + getSearch(params);
        return await fetchGet(url);
    }
}