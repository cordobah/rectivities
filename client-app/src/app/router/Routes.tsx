import {createBrowserRouter, Navigate, RouteObject} from "react-router-dom";
import App from "../layout/App";
import ActivityDashboard from "../../features/Activity/Dashboard/ActivityDashboard";
import ActivityForm from "../../features/Activity/form/ActivityForm";
import ActivityDetails from "../../features/Activity/details/ActivityDetails";
import TestErrors from "../../features/errors/TestError";
import NotFound from "../../features/errors/NotFound";
import ServerError from "../../features/errors/ServerError";
import LoginForm from "../../features/users/LoginForm";
import ProfilePage from "../../features/profile/ProfilePage";
import RequireAuth from "./RequireAuth";

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App/>,
        children: [
            {
                element: <RequireAuth/>, children: [
                    {path: 'activities', element: <ActivityDashboard/>},
                    {path: 'activities/:id', element: <ActivityDetails/>},
                    {path: 'createActivity', element: <ActivityForm key='create'/>},
                    {path: 'manage/:id', element: <ActivityForm key='manage'/>},
                    {path: 'profiles/:username', element: <ProfilePage/>},
                    {path: 'login', element: <LoginForm/>},
                    {path: 'errors', element: <TestErrors/>},
                ]
            },
            {path: 'not-found', element: <NotFound/>},
            {path: 'server-error', element: <ServerError/>},
            {path: '*', element: <Navigate replace to='/not-found'/>},
        ]
    }
]

export const router = createBrowserRouter(routes);