import { fetchGet, getSearch } from "./QueryHelper";

export class IdentityService {

    login = async (credentials, search) => {
        const url = process.env.REACT_APP_IDENTITY_API + '/authenticate/login' + (search ?? '');
        const response = await fetch(url, {
            method: 'POST',
            headers: { 
                'Content-Type' : 'application/json'
            },
            credentials: 'include',
            body: JSON.stringify({
                passwordHash: credentials.passHash,
                email: credentials.email,
                rememberMe: credentials.rememberMe,
            })
        })

        const data = await response.json();
        return data;
    }

    externalLogin = async (scheme, search) => {
        const params = [
            { name: "scheme", value: scheme },
            { name: "returnUrl", value: search }
        ]

        const url = process.env.REACT_APP_IDENTITY + '/external/challenge' + getSearch(params);
        const response = await fetchGet(url);
        return response;
    }

    register = async (credentials, search) => {
        const url = process.env.REACT_APP_IDENTITY_API + '/authenticate/register' + (search ?? '')
        const response = await fetch(url, {
            method: 'POST',
            headers: { 
                'Content-Type' : 'application/json'
            },
            credentials: 'include',
            body: JSON.stringify({
                // name,
                username: credentials.username,
                passwordHash: credentials.passHash,
                email: credentials.email,
            })
        })

        const data = await response.json();
        return data;
    }

    logout = async (search) => {
        const url = process.env.REACT_APP_IDENTITY_API + '/authenticate/logout' + search
        const response = await fetch(url, {
            credentials: 'include'
        });
  
        const data = await response.json();
        return data;
    }
}