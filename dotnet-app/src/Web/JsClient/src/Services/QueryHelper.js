export const fetchGet = async (url) => {
    const response = await fetch(url);
    const data = await response.json();
    return data;
}

export const getSearch = (params) => {
    const notNullParams = params.filter(param => param?.value);

    let searchString = '';
    if (notNullParams && notNullParams.length > 0) {
        searchString = notNullParams.map(param => {
            if (Array.isArray(param?.value)) {
                //console.log("param", param, param.value.length);
                return param.value.map((val, index) => {
                    // console.log("val:", val);
                    let str = `${param.name}[${val.name ? val.name : index}]=`;
                    str += val.value ? val.value : val;
                    return str;
                }).join('&');
            }
            else
                return param.name + '=' + param.value
        }).join('&');
    }

    if (searchString)
        searchString = '?' + searchString;
    else
        searchString = '';

    // console.log(searchString);
    return searchString
}