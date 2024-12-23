/**
 * Imports the axios library for making HTTP requests.
 * @type {import('axios').AxiosInstance}
 */
import axios from 'axios';

/**
 * Base URL for the API.
 * @type {string}
 */
const API_URL = 'https://localhost:44356/api';

/**
 * Creates an Axios instance with default configuration for API interactions.
 *
 * @type {import('axios').AxiosInstance}
 * @property {string} baseURL - Base URL for the API.
 * @property {object} headers - Default headers for requests.
 * @property {boolean} withCredentials - Whether to include credentials in requests.
 */
export const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json'
  },
  //  timeout: 5000,
  withCredentials: false
});

/**
 * Adds an interceptor to the request chain to add authorization header if a token exists.
 */
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

/**
 * Adds an interceptor to log the starting request.
 */
api.interceptors.request.use(request => {
  console.log('Starting Request:', request);
  return request;
});

/**
 * Adds interceptors to log responses and handle errors.
 */
api.interceptors.response.use(
  response => {
    console.log('Response:', response);
    return response;
  },
  error => {
    console.log('Response Error:', error);
    return Promise.reject(error);
  }
);