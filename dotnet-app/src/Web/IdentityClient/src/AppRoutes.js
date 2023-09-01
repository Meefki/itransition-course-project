import { Error } from "./components/Erorr";
import { Login } from "./components/Login";
import { Logout } from "./components/Logout";

const AppRoutes = [
    {
        path: '/login',
        element: <Login />
    },
    {
        path: '/error',
        element: <Error />
    },
    {
        path: '/logout',
        element: <Logout />
    }
];

export default AppRoutes;
