import React from 'react';
import { Navigate } from 'react-router-dom';

export default function PrivateRoute({ children, allowed = [], role }) {
  if (!role) return <Navigate to="/login" replace />;
  if (allowed.length && !allowed.includes(role)) return <Navigate to="/" replace />;
  return children;
}
