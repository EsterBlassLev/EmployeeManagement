/**
 * Imports the API client instance from the './api' module.
 * @type {import('./api').api}
 */
import { LoginDto } from '../types/LoginDto ';
import { RegisterDto } from '../types/RegisterDto ';
import { api } from './api';

/**
 * Interface for login credentials.
 * @typedef {object} LoginDto
 * @property {string} username - Username for login.
 * @property {string} password - Password for login.
 */

/**
 * Interface for user registration data.
 * @typedef {object} RegisterDto
 * @property {string} username - Username for registration.
 * @property {string} password - Password for registration.
 * @property {string} email - Email address for registration.
 */

/**
 * Authentication service with methods for login, registration, and logout.
 */
export const authService = {
  /**
   * Logs in a user with the provided credentials.
   *
   * @param {LoginDto} credentials - Username and password for login.
   * @returns {Promise<any>} Promise resolving to the login response data.
   * @throws {Error} If login fails or there's a network error.
   */
  async login(credentials: LoginDto) {
    const response = await api.post('/auth/login', credentials);
    if (response.data.token) {
      localStorage.setItem('token', response.data.token);
    }
    return response.data;
  },

  /**
   * Registers a new user with the provided data.
   *
   * @param {RegisterDto} data - User data for registration.
   * @returns {Promise<any>} Promise resolving to the registration response data.
   * @throws {Error} If registration fails or there's a network error.
   */
  async register(data: RegisterDto) {
    try {
      const response = await api.post('/auth/register', data);
      return response.data;
    } catch (error: any) {
      if (error.response) {
        throw new Error(error.response.data || 'Registration failed');
      } else if (error.request) {
        throw new Error('No response from server');
      } else {
        throw new Error('Error setting up the request');
      }
    }
  },

  /**
   * Logs out the current user by removing the authentication token from local storage.
   */
  logout() {
    localStorage.removeItem('token');
  },

  /**
   * Retrieves the currently logged-in user information from the localStorage token.
   *
   * @returns {object|null} The decoded user object from the token or null if not logged in.
   */
  getCurrentUser() {
    const token = localStorage.getItem('token');
    return token ? JSON.parse(atob(token.split('.')[1])) : null;
  }
};