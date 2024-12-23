import React, { useState, useEffect } from 'react';
import { DataGrid } from '@mui/x-data-grid';
import { Button, Container, TextField, Dialog, DialogTitle, DialogContent, DialogActions, Typography } from '@mui/material';
import { employeeService } from '../services/employeeService';
import { Employee } from '../types/Employee ';
import { toast } from 'react-toastify';
import { format } from 'date-fns';

/**
 * EmployeeList component for employee management
 * Provides functionality for CRUD operations on employees - READ
 * @component
 */

export const EmployeeList = () => {
    const [employees, setEmployees] = useState<Employee[]>([]);
    const [loading, setLoading] = useState(true);
    const [searchTerm, setSearchTerm] = useState('');
    const [selectedEmployee, setSelectedEmployee] = useState<Employee>(employees[0]);
    const [openDialog, setOpenDialog] = useState(false);

    const loadEmployees = async () => {
        try {
            const data = await employeeService.GetEmployeesByManagerId();
            setEmployees(data);
        } catch (error) {
            toast.error('Failed to load employees');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadEmployees();
    }, []);

    /**
 * Handles the search of an employee
 * @param {string} name - The NAME of the employee to search
 * @returns {Promise<void>}
 */

    const sanitizeSearchTerm = (term: string): string => {
        return term.replace(/[<>{}]/g, '');
      };

    const handleSearch = async (event: React.ChangeEvent<HTMLInputElement>) => {
        const value = event.target.value;
        const sanitizedValue = sanitizeSearchTerm(value);
        setSearchTerm(sanitizedValue);
        if (sanitizedValue) {
            const results = await employeeService.searchByName(sanitizedValue);
            setEmployees(results);
        } else {
            loadEmployees();
        }
    };

/**
 * Handles the deletion of an employee
 * @param {number} id - The ID of the employee to delete
 * @throws {ApiError} When the deletion fails
 * @returns {Promise<void>}
 */

    const handleDelete = async (id: number) => {
        if (window.confirm('Are you sure you want to delete this employee?')) {
            try {
                await employeeService.delete(id);
                toast.success('Employee deleted successfully');
                loadEmployees();
            } catch (error) {
                toast.error('Failed to delete employee');
            }
        }
    };

//show table
    const columns = [
        { field: 'fullName', headerName: 'Full Name', width: 200 },
        { field: 'email', headerName: 'Email', width: 200 },
        {
            field: 'createdAt',
            headerName: 'Created Date',
            width: 200,
            renderCell: (params: any) => {
                const isoDate = params.row.createdAt; // Assuming 'createdAt' holds the ISO date
                const formattedDate = format(new Date(isoDate), 'dd/MM/yyyy');
                return <Typography>{formattedDate}</Typography>;
            }
        },
            {
            field: 'actions',
            headerName: 'Actions',
            width: 200,
            renderCell: (params: any) => (
                <>
                    <Button onClick={() => {
                        setSelectedEmployee(params.row);
                        setOpenDialog(true);
                    }}>
                        View
                    </Button>
                    <Button onClick={() => handleDelete(params.row.id)} color="error">
                        Delete
                    </Button>
                </>
            )
        }
    ];
    return (
        <Container maxWidth="lg" sx={{ mt: 4 }}>
            <TextField
                fullWidth
                label="Search Employees"
                value={searchTerm}
                onChange={handleSearch}
                margin="normal"
            />
            <DataGrid
                rows={employees}
                columns={columns}
                loading={loading}
                autoHeight
                initialState={{
                    pagination: {
                        paginationModel: {
                            pageSize: 5
                        },
                    },
                }}
                pageSizeOptions={[5]}
                disableRowSelectionOnClick
            />
            <Dialog open={openDialog} onClose={() => setOpenDialog(false)}>
                <DialogTitle>Employee Details</DialogTitle>
                <DialogContent>
   {selectedEmployee && (
       <>
           <p>Full Name: {selectedEmployee.fullName}</p>
           <p>Email: {selectedEmployee.email}</p>
           <p>Created Date: {selectedEmployee.createdAt ? 

               new Intl.DateTimeFormat('he-IL', {
                   day: '2-digit',
                   month: '2-digit',
                   year: 'numeric',
                   hour: '2-digit',
                   minute: '2-digit'
               }).format(new Date(selectedEmployee.createdAt)) : ''}
           </p>
           <p>GUID: {selectedEmployee.guid}</p>
       </>
   )}
</DialogContent>
                <DialogActions>
                    <Button onClick={() => setOpenDialog(false)}>Close</Button>
                </DialogActions>
            </Dialog>
        </Container>
    );
};