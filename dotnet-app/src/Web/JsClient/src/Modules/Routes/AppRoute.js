import { Callback } from "../Identity/Callback";
import { Main } from "../Main";
import { Login } from "../Identity/Login"
import { Logout } from "../Identity/Logout"
import { Error } from "../Error"
import ThemeLayout from "../Layouts/ThemeLayout";
import WithoutLayout from "../Layouts/WithoutLayout";
import MainLayout from "../Layouts/MainLayout";
import { RenderRoutes } from "./RenderRoutes";

const AppRoutes = [
    {
        layout: <ThemeLayout />,
        routes: [
            {
                name: 'login',
                title: 'Login page',
                path: '/login',
                component: <Login />,
                isPublic: true,
                hasSiderLink: true
            }
        ]
    },
    {
        layout: <WithoutLayout />,
        routes: [
            {
                name: 'callback',
                title: 'Redirection page',
                path: '/callback',
                component: <Callback />,
                isPublic: true
            },
            {
                name: 'error',
                title: 'Error page',
                path: '/error',
                component: <Error />,
                isPublic: true
            },
            {
                name: 'logout',
                title: 'Logout page',
                path: '/logout',
                component: <Logout />,
                isPublic: true,
                hasSiderLink: true
            }
        ]
    },
    {
        layout: <MainLayout />,
        routes: [
            {
                name: 'main',
                title: 'Main page',
                path: '/',
                component: <Main />,
                isPublic: true,
                hasSiderLink: true
            }
        ]
    }
]

export const Routes = RenderRoutes(AppRoutes);