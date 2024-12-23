import { useState, useEffect } from 'react';
import {
    Container,
    Paper,
    Typography,
    Button,
    TextField,
    Box,
    CircularProgress
} from '@mui/material';
import { useNavigate, useParams } from 'react-router-dom';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import { employeeService } from '../services/employeeService';
import { toast } from 'react-toastify';

/**
 * EditEmployee component for employee management
 * Provides functionality for CRUD operations on employees the UPDATE of the CRUD
 * @component
 */

export const EditEmployee = () => {
    const navigate = useNavigate();
    const { id } = useParams<{ id: string }>();
    const [loading, setLoading] = useState(true);

    // check the validtions
    const formik = useFormik({
        initialValues: {
            fullName: '',
            email: '',
            password: '' // Optional for editing
        },
        validationSchema: Yup.object({
            email: Yup.string()
                .email('Invalid email address')
                .required('Required'),
            fullName: Yup.string()
                .required('Required'),
            password: Yup.string()
                .test('password', 'Password must be at least 8 characters', function(value) {
                    // can be null
                    if (!value) return true;
                    // if not null check validators
                    return value.length >= 8;
                })
                .test('password', 'Password must contain a capital letter', function(value) {
                    if (!value) return true;
                    return /[A-Z]/.test(value);
                })
        }),
        onSubmit: async (values) => {
            if (!id) return;
            
            try {
                // only changes
                const updateData = {
                    fullName: values.fullName,
                    email: values.email,
                    // if change the password 

                    ...(values.password ? { password: values.password } : {})
                };

                await employeeService.update(Number(id), updateData);
                toast.success('Employee updated successfully');
                navigate('/dashboard');
            } catch (error) {
                // show error 
                if (error instanceof Error) {
                    toast.error(error.message);
                } else {
                    toast.error('Failed to update employee');
                }
            }
        }
    });

    useEffect(() => {
        const fetchEmployee = async () => {
            if (!id) return;

            try {
                const data = await employeeService.getById(Number(id));
                formik.setValues({
                    fullName: data.fullName,
                    email: data.email,
                    password: '' // Don't show existing password
                });
            } catch (error) {
                toast.error('Failed to load employee data');
                navigate('/dashboard');
            } finally {
                setLoading(false);
            }
        };

        fetchEmployee();
    }, [id, navigate]);

    if (loading) {
        return (
            <Box display="flex" justifyContent="center" alignItems="center" minHeight="200px">
                <CircularProgress />
            </Box>
        );
    }

    return (
        <Container maxWidth="sm" sx={{ mt: 4, mb: 4 }}>
            <Paper sx={{ p: 4 }}>
                <Typography component="h1" variant="h5" gutterBottom>
                    Edit Employee
                </Typography>

                <form onSubmit={formik.handleSubmit}>
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
                        label="New Password (Optional)"
                        name="password"
                        type="password"
                        value={formik.values.password}
                        onChange={formik.handleChange}
                        error={formik.touched.password && Boolean(formik.errors.password)}
                        helperText={(formik.touched.password && formik.errors.password) || "Leave empty to keep current password"}
                    />

                    <Box sx={{ mt: 3, display: 'flex', justifyContent: 'flex-end', gap: 2 }}>
                        <Button
                            variant="outlined"
                            onClick={() => navigate('/dashboard')}
                        >
                            Cancel
                        </Button>
                        <Button
                            type="submit"
                            variant="contained"
                            color="primary"
                        >
                            Update
                        </Button>
                    </Box>
                </form>
            </Paper>
        </Container>
    );
};