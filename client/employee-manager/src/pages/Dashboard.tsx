import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
    Container,
    Grid,
    Paper,
    Typography,
    Button,
    Box,
    TextField,
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions
} from '@mui/material';
import { DataGrid } from '@mui/x-data-grid';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import { employeeService } from '../services/employeeService';
import { Employee } from '../types/Employee ';
import { toast } from 'react-toastify';

import {
    Add as AddIcon,
    Search as SearchIcon,
    Edit as EditIcon,
    Delete as DeleteIcon
} from '@mui/icons-material';
import { format } from 'date-fns';

/**
 * Dashboard component for employee management
 * Provides functionality for CRUD operations on employees
 * @component
 */

export const Dashboard = () => {

    const navigate = useNavigate();
    const [employees, setEmployees] = useState<Employee[]>([]);
    const [loading, setLoading] = useState(true);
    const [openDialog, setOpenDialog] = useState(false);
    const [searchTerm, setSearchTerm] = useState('');

    /**
 * Handles the searches of an employee
 * @param {string} name - The name of the employee to search
 * @throws {ApiError} When the deletion fails
 * @returns {Promise<void>}
 */

    const sanitizeSearchTerm = (term: string): string => {
        return term.replace(/[<>{}]/g, '');
    };

    const handleSearch = async (value: string) => {
        const sanitizedValue = sanitizeSearchTerm(value);
        setSearchTerm(sanitizedValue);
        if (sanitizedValue.trim()) {
            try {
                const results = await employeeService.searchByName(sanitizedValue);
                setEmployees(results);
            } catch (error) {
                toast.error('Search failed');
            }
        } else {
            loadEmployees();
        }
    };

    const loadEmployees = async () => {
        try {
            console.log('loadEmployees');
            setLoading(true);
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
 * Handles the edition of an employee
 * @param {number} id - The ID of the employee to edite
 * @throws {ApiError} When the deletion fails
 *navigate to edit employee
 */

    const handleEdit = (id: number) => {
        navigate(`/edit-employee/${id}`);
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



    const formik = useFormik({
        initialValues: {
            email: '',
            fullName: '',
            password: ''
        },
        validationSchema: Yup.object({
            email: Yup.string()
                .email('Invalid email address')
                .required('Required'),
            fullName: Yup.string()
                .required('Required'),
            password: Yup.string()
                .min(6, 'Must be at least 6 characters')
                .matches(/[A-Z]/, 'Must contain at least one capital letter')
                .required('Required')
        }),
        onSubmit: async (values, { resetForm }) => {
            try {
                await employeeService.create(values);
                toast.success('Employee created successfully');
                resetForm();
                setOpenDialog(false);
                loadEmployees();
            } catch (error) {
                toast.error('Failed to create employee');
            }
        }
    });

    // show table 
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
        }, {
            field: 'actions',
            headerName: 'Actions',
            width: 200,
            renderCell: (params: any) => (
                <Box>
                    <Button
                        color="primary"
                        onClick={() => handleEdit(params.row.id)}
                        startIcon={<EditIcon />}
                        sx={{ mr: 1 }}
                    >
                        Edit
                    </Button>
                    <Button
                        color="error"
                        onClick={() => handleDelete(params.row.id)}
                        startIcon={<DeleteIcon />}
                    >
                        Delete
                    </Button>
                </Box>
            )
        }
    ];

    return (
        <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Paper sx={{ p: 2, display: 'flex', flexDirection: 'column' }}>
                        <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
                            <Typography component="h2" variant="h6" color="primary" gutterBottom>
                                Employee Management Dashboard
                            </Typography>
                            <Button
                                variant="contained"
                                startIcon={<AddIcon />}
                                onClick={() => setOpenDialog(true)}
                            >
                                Add Employee
                            </Button>
                        </Box>

                        <Box mb={2}>
                            <TextField
                                fullWidth
                                label="Search Employees"
                                value={searchTerm}
                                onChange={(e) => handleSearch(e.target.value)}
                                InputProps={{
                                    startAdornment: <SearchIcon color="action" />
                                }}
                            />
                        </Box>

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
                    </Paper>
                </Grid>
            </Grid>

            <Dialog open={openDialog} onClose={() => setOpenDialog(false)}>
                <DialogTitle>Add New Employee</DialogTitle>
                <form onSubmit={formik.handleSubmit}>
                    <DialogContent>
                        <TextField
                            fullWidth
                            margin="normal"
                            label="Full Name"
                            name="fullName"
                            value={formik.values.fullName}
                            onChange={formik.handleChange}
                            error={formik.touched.fullName && Boolean(formik.errors.fullName)}
                            helperText={formik.touched.fullName && formik.errors.fullName}
                        />
                        <TextField
                            fullWidth
                            margin="normal"
                            label="Email"
                            name="email"
                            value={formik.values.email}
                            onChange={formik.handleChange}
                            error={formik.touched.email && Boolean(formik.errors.email)}
                            helperText={formik.touched.email && formik.errors.email}
                        />
                        <TextField
                            fullWidth
                            margin="normal"
                            label="Password"
                            name="password"
                            type="password"
                            value={formik.values.password}
                            onChange={formik.handleChange}
                            error={formik.touched.password && Boolean(formik.errors.password)}
                            helperText={formik.touched.password && formik.errors.password}
                        />
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={() => setOpenDialog(false)}>Cancel</Button>
                        <Button type="submit" variant="contained">Add</Button>
                    </DialogActions>
                </form>
            </Dialog>
        </Container>
    );
};