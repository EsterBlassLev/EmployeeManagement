import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import { Navbar } from './components/Layout/Navbar';
import { ProtectedRoute } from './components/ProtectedRoute';
import { Login } from './pages/Login';
import { Register } from './pages/Register';
import { EmployeeList } from './pages/EmployeeList';
import { Dashboard } from './pages/Dashboard';
import 'react-toastify/dist/ReactToastify.css';
import React from 'react';
import { EditEmployee } from './pages/EditEmployee';

function App() {
    return (
        <Router>
            <Navbar />
            <ToastContainer />
            <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route
                    path="/dashboard"
                    element={
                        <ProtectedRoute>
                            <Dashboard />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/employees"
                    element={
                        <ProtectedRoute>
                            <EmployeeList />
                        </ProtectedRoute>
                    }
                />
                  <Route path="/edit-employee/:id" element={
                      <ProtectedRoute>
                    <EditEmployee />
                    </ProtectedRoute>
                } 
                    /> 
                <Route path="/" element={<Login />} />
            </Routes>
        </Router>
    );
}

export default App;
