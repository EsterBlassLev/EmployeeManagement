/**
 * Imports the API client instance from the './api' module.
 * @type {import('./api').api}
 */
import { Employee } from '../types/Employee ';
import { api } from './api';

/**
 * Interface for employee data.
 * @typedef {object} Employee
 * */

/**
 * Employee service with methods for CRUD operations and searching employees.
 */
export const employeeService = {
  /**
   * Gets an employee by ID.
   *
   * @param {number} id - The ID of the employee to retrieve.
   * @returns {Promise<Employee>} Promise resolving to the retrieved employee data.
   * @throws {Error} If fetching employee fails or there's a network error.
   */
  async getById(id: number) {
    const response = await api.get<Employee>(`/Employees/GetEmployee/${id}`);
    return response.data;
  },

  /**
   * Gets all employees managed by a specific manager.
   *
   * @returns {Promise<Employee[]>} Promise resolving to an array of employee data.
   * @throws {Error} If fetching employees fails or there's a network error.
   */
  async GetEmployeesByManagerId() {
    const response = await api.get<Employee[]>(`/Employees/GetEmployeesByManagerId`);
    return response.data;
  },

  /**
   * Creates a new employee.
   *
   * @param {Partial<Employee>} employee - Partial employee data for creation.
   * @returns {Promise<Employee>} Promise resolving to the created employee data.
   * @throws {Error} If creating employee fails or there's a network error.
   */
  async create(employee: Partial<Employee>) {
    const response = await api.post<Employee>('/Employees/CreateEmployee', employee);
    return response.data;
  },

  /**
   * Updates an existing employee.
   *
   * @param {number} id - The ID of the employee to update.
   * @param {Partial<Employee>} employee - Partial employee data for update.
   * @returns {Promise<Employee>} Promise resolving to the updated employee data.
   * @throws {Error} If updating employee fails or there's a network error.
   */
  async update(id: number, employee: Partial<Employee>) {
    const response = await api.put<Employee>(`/Employees/UpdateEmployee/${id}`, employee);
    return response.data;
  },

  /**
   * Deletes an employee by ID.
   *
   * @param {number} id - The ID of the employee to delete.
   * @returns {Promise<void>} Promise that resolves when the employee is deleted.
   * @throws {Error} If deleting employee fails or there's a network error.
   */
  async delete(id: number) {
    await api.delete(`/Employees/DeleteEmployee/${id}`);
  },

  /**
   * Searches for employees by name.
   *
   * @param {string} name - The name to search for (partial matches are possible).
   * @returns {Promise<Employee[]>} Promise resolving to an array of matching employee data.
   * @throws {Error} If searching employees fails or there's a network error.
   */
  async searchByName(name: string) {
    const response = await api.get<Employee[]>(`/Employees/SearchEmployees?name=${name}`);
    return response.data;
  }
};