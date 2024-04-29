using Manage_Employee.Models;
using Manage_Employer.DTO;

namespace Manage_Employee.Interfaces
{
    public interface IEmployeeRepository
    {

        public Task<Employee> CreateEmployees(EmployeeDTO employee);
        public Task<IEnumerable<Employee>> GetEmployees();
        public Task<Employee> GetEmployee(int id);
        public Task UpdateEmployee(int id, EmployeeDTO employee);
        public Task DeleteEmployee(int id);
    }
}
