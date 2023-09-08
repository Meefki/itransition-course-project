import {fetchGet, getSearch} from "./QueryHelper";

export class UserService {
    getUser = async (userId) => {
        const params = [
            { name: "id", value: userId }
        ]

        const url = process.env.REACT_APP_IDENTITY_API + "/userProfile" + getSearch(params);
        return await fetchGet(url);
    }

    getUserNames = async (ids) => {
        const params = [
            { name: "ids", value: ids }
        ]

        const url = process.env.REACT_APP_IDENTITY_API + "/userProfile/names" + getSearch(params);
        return await fetchGet(url);
    }
}