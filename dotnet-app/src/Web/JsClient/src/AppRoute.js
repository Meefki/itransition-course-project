import { Callback } from "./Modules/Callback";
import { Main } from "./Modules/Main";
import { Login } from "./Modules/Identity/Login"
import { Logout } from "./Modules/Identity/Logout"
import { Error } from "./Modules/Error"

const AppRoutes = [
    {
        path: '/',
        element: <Main />
    },
    {
        path: '/callback',
        element: <Callback />
    },
    {
        path: '/error',
        element: <Error />
    },
    {
        path: '/login',
        element: <Login />
    },
    {
        path: '/logout',
        element: <Logout />
    }
];

export default AppRoutes;