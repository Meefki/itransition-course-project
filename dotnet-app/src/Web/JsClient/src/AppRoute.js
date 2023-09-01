import { Callback } from "./Modules/Callback";
import { Main } from "./Modules/Main";

const AppRoutes = [
    {
        path: '/',
        element: <Main />
    },
    {
        path: '/callback',
        element: <Callback />
    }
];

export default AppRoutes;