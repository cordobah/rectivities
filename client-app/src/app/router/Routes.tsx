import {createBrowserRouter, RouteObject} from "react-router-dom";
import App from "../layout/App";
import ActivityDashboard from "../../features/Activity/Dashboard/ActivityDashboard";
import ActivityForm from "../../features/Activity/form/ActivityForm";
import ActivityDetails from "../../features/Activity/details/ActivityDetails";

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App/>,
        children:[
            {path: 'activities', element: <ActivityDashboard/>},
            {path: 'activities/:id', element: <ActivityDetails/>},
            {path: 'createActivity', element: <ActivityForm key='create'/>},
            {path: 'manage/:id', element: <ActivityForm key='manage'/>},
        ]
    }
]

export const router = createBrowserRouter(routes);