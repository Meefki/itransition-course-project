import axios from "axios";

export const fetchGet = async (url, headers = {}) => {
    const response = await axios.get(url, { headers });
    const data = await response.data;
    return data;
}

export const fetchPost = async (url, body = {}, headers = {}) => {
    const response = await axios.post(url, body, { headers });
    const data = response.data;
    return data;
}

export const getSearch = (params) => {
    let searchParams = new URLSearchParams();

    const notNullParams = 
        params
        .filter(param => param?.value)
        .filter(param => !(JSON.stringify(param?.value) === '{}'));

    if (notNullParams && notNullParams.length > 0) {
        notNullParams.forEach(param => {
            if (Array.isArray(param?.value)) {
                param.value.forEach((val, index) => {
                    const name = `${param.name}[${val.name ? val.name : index}]`;
                    const value = Object.getOwnPropertyNames(val).includes('value') ? val.value : val;
                    searchParams.append(name, value);
                });
            }
            else {
                searchParams.append(param.name, param.value);
            }
        });
    }

    let searchString = '';
    if (searchParams.toString())
        searchString = '?' + searchParams.toString();
    else
        searchString = '';

    return searchString
}