import React from "react";
import { Navigate, Outlet } from 'react-router-dom';

function ProtectedRoute({ isPublic, isAuthorized }) {
    return (isPublic || isAuthorized) ? <Outlet /> : <Navigate to='/' />
}

export default ProtectedRoute;