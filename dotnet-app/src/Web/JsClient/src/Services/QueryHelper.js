export const fetchGet = async (url, headers = {}) => {
    const response = await fetch(url, { method: 'GET', headers: headers });
    const data = await response.json();
    return data;
}

export const getSearch = (params) => {
    const searchParams = new URLSearchParams();

    const notNullParams = 
    params
    .filter(param => param?.value)
    .filter(param => !(JSON.stringify(param?.value) === '{}'));

    let searchString = '';
    if (notNullParams && notNullParams.length > 0) {
        const searchStr = notNullParams.map(param => {
            if (Array.isArray(param?.value)) {
                // console.log("param", param, param.value.length);
                const params = param.value.map((val, index) => {
                    // console.log("val:", val);
                    let str = `${param.name}[${val.name ? val.name : index}]=`;
                    const valueStr = Object.getOwnPropertyNames(val).includes('value') ? val.value : val;
                    if (valueStr)
                        str += valueStr;
                    else
                        str = '';
                    // console.log(str);
                    return str;
                }).filter(s => s ? true : false);
                return (params && params.length !== 0) ? params.join('&') : '';
            }
            else
                return param.name + '=' + param.value
        });
        searchString = searchStr.filter(s => s ? true : false) ? searchStr.filter(s => s ? true : false).join('&') : '';
    }

    if (searchString)
        searchString = '?' + searchString;
    else
        searchString = '';

    // console.log(searchString);
    return searchString
}