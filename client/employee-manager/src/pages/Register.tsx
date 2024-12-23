import { useFormik } from 'formik';
import * as Yup from 'yup';
import { 
    Box, 
    Button, 
    Container, 
    Paper, 
    TextField, 
    Typography 
} from '@mui/material';
import { useNavigate, Link } from 'react-router-dom';
import { authService } from '../services/auth.service';
import { toast } from 'react-toastify';

/**
 * Register component for employee management
 * @component
 */


export const Register = () => {
    const navigate = useNavigate();

    const formik = useFormik({
        initialValues: {
            email: '',
            password: '',
            confirmPassword: '',
            fullName: ''
        },
        validationSchema: Yup.object({
            email: Yup.string()
                .email('Invalid email address')
                .required('Email is required'),
            password: Yup.string()
                .min(8, 'Password must be at least 8 characters')
                .matches(/[A-Z]/, 'Password must contain at least one capital letter')
                .required('Password is required'),
            confirmPassword: Yup.string()
                .oneOf([Yup.ref('password')], 'Passwords must match')
                .required('Please confirm your password'),
            fullName: Yup.string()
                .required('Full name is required')
                .min(2, 'Name must be at least 2 characters')
        }),
        onSubmit: async (values) => {
            try {
                await authService.register({
                    email: values.email,
                    password: values.password,
                    fullName: values.fullName
                });
                toast.success('Registration successful! Please login.');
                navigate('/login');
            } catch (error: any) {
                toast.error(error.message || 'Registration failed. Please try again.');
            }
        }

    });

    return (
        <Container component="main" maxWidth="sm">
            <Box sx={{ 
                marginTop: 8, 
                display: 'flex', 
                flexDirection: 'column', 
                alignItems: 'center' 
            }}>
                <Paper elevation={3} sx={{ p: 4, width: '100%' }}>
                    <Typography component="h1" variant="h5" align="center" gutterBottom>
                        Sign Up
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
                            label="Email Address"
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
                        <TextField
                            fullWidth
                            margin="normal"
                            label="Confirm Password"
                            name="confirmPassword"
                            type="password"
                            value={formik.values.confirmPassword}
                            onChange={formik.handleChange}
                            error={formik.touched.confirmPassword && Boolean(formik.errors.confirmPassword)}
                            helperText={formik.touched.confirmPassword && formik.errors.confirmPassword}
                        />
                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            sx={{ mt: 3, mb: 2 }}
                        >
                            Sign Up
                        </Button>
                        <Box textAlign="center">
                            <Link to="/login" style={{ textDecoration: 'none' }}>
                                <Typography color="primary">
                                    Already have an account? Sign In
                                </Typography>
                            </Link>
                        </Box>
                    </form>
                </Paper>
            </Box>
        </Container>
    );
};