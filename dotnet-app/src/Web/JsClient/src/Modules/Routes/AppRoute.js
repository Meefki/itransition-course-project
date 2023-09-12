import { Callback } from "../Identity/Callback";
import { Login } from "../Identity/Login"
import { Logout } from "../Identity/Logout"
import { Error } from "../Shared/Error"
import ThemeLayout from "../Layouts/ThemeLayout";
import WithoutLayout from "../Layouts/WithoutLayout";
import CommonLayout from "../Layouts/CommonLayout";
import { RenderRoutes } from "./RenderRoutes";
import Review from "../Reviews/Review";
import TwoColumnLayout from "../Layouts/TwoColumnLayout";
import ReviewList from "../Reviews/ReviewList";
import ReviewCarousel from "../Reviews/ReviewCarousel";
import ReviewsFilter from "../Reviews/ReviewsFilter";
import ReviewActions from "../Identity/User/ReviewActions";
import UserActions from "../Identity/User/UserActions";
import UserInfo from "../Identity/User/UserInfo";
import { UserManager } from "oidc-client";
import { config } from "../../Contexts/UserManagerContext";

const mgr = new UserManager(config);
const publicProfileSideComponents = async () => {
    const user = await mgr.getUser();
    if (user) {
        if (user?.profile.role?.toLowerCase() === "admin")
            return [<UserInfo owner={false} />, <ReviewActions />, <UserActions />];
    }
    return [<UserInfo owner={false} />]
}

const AppRoutes = [
    {
        layout: <ThemeLayout />,
        routes: [
            {
                name: 'login',
                title: 'Login page',
                path: '/login',
                component: <Login />,
                isPublic: true
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
                isPublic: true
            }
        ]
    },
    {
        layout: <CommonLayout />,
        routes: [
            {
                name: 'main',
                title: 'Main page',
                path: '/',
                component: <TwoColumnLayout 
                    mainComponents={[<ReviewList />]} 
                    sideComponents={[<ReviewCarousel />]} />,
                isPublic: true
            },
            {
                name: 'profile',
                title: 'Profile page',
                path: '/profile/me',
                component: <TwoColumnLayout 
                    mainComponents={[<ReviewsFilter immutableFilters={["authorUserId"]}/>, <ReviewList table={true}/>]}
                    sideComponents={[<UserInfo owner={true} />, <ReviewActions />, <UserActions />]}
                    hideSecondCol={false}/>,
                isPublic: false
            },
            {
                name: 'public-profile',
                title: 'Profile page',
                path: '/profile/:id',
                component: <TwoColumnLayout 
                mainComponents={[<ReviewsFilter immutableFilters={["authorUserId"]} />, <ReviewList table={true}/>]}
                sideComponents={await publicProfileSideComponents()}
                hideSecondCol={false}/>,
                isPublic: true
            },
            {
                name: 'review',
                title: 'Review page',
                path: '/review/:id',
                component: <Review />,
                isPublic: true
            },
        ]
    }
]

export const Routes = RenderRoutes(AppRoutes);