import { Navigate, useLocation } from 'react-router-dom';
import { authService } from '../services/auth.service';
import React, { ReactNode } from 'react';

export const ProtectedRoute = ({ children }: { children: ReactNode }) => {
    const user = authService.getCurrentUser();
    const location = useLocation();

    if (!user) {
        return <Navigate to="/login" state={{ from: location }} replace />;
    }

    return (
        <>{children}</> 
    );
};