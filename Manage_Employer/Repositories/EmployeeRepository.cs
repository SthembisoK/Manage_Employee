using Dapper;
using GetBusDriver.Context;
using Manage_Employee.Interfaces;
using Manage_Employee.Models;
using Manage_Employer.DTO;
using System.Data;
using System;
using System.Text.RegularExpressions;

namespace Manage_Employee.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DBContext _dbContext;

        public EmployeeRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Employee> CreateEmployees(EmployeeDTO employee)
        {
            // Email validation
            if (!IsValidEmail(employee.Email))
            {
                throw new ArgumentException("Invalid email address.");
            }

            var query = "INSERT INTO Employee (Email, FirstName, LastName, Role, Password) VALUES (@Email, @FirstName, @LastName, @Role, @Password)" +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new DynamicParameters();
            parameters.Add("Email", employee.Email, DbType.String);
            parameters.Add("FirstName", employee.FirstName, DbType.String);
            parameters.Add("LastName", employee.LastName, DbType.String);
            parameters.Add("Role", employee.Role, DbType.String);
            parameters.Add("Password", employee.Password, DbType.String);

            using (var connection = _dbContext.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);
                var createdEmployee = new Employee
                {
                    Id = id,
                    Email = employee.Email,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Role = employee.Role,
                    Password=employee.Password

                };
                return createdEmployee;
            }
        }

        // Email validation method
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Use .NET's built-in email validation
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }



        public async Task DeleteEmployee(int id)
        {
            var query = "DELETE FROM Employee WHERE Id = @Id";

            using (var connection = _dbContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            var query = "Select * FROM Employee";
            using (var connection = _dbContext.CreateConnection())
            {
                var employees = await connection.QueryAsync<Employee>(query);
                return employees.ToList();

            }
        }

        public async Task<Employee> GetEmployee(int id)
        {
            var query = "SELECT * FROM Employee WHERE Id=@Id";

            using (var connection = _dbContext.CreateConnection())
            {
                var employee = await connection.QuerySingleOrDefaultAsync<Employee>(query, new { id });

                return employee;
            }
        }

        public async Task UpdateEmployee(int id, EmployeeDTO employee)
        {
            var query = "UPDATE Employee SET Email=@Email, FirstName=@FirstName, LastName=@LastName, Role=@Role, Password=@Password WHERE Id=@Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Email", employee.Email, DbType.String);
            parameters.Add("FirstName", employee.FirstName, DbType.String);
            parameters.Add("LastName", employee.LastName, DbType.String);
            parameters.Add("Role", employee.Role, DbType.String);
            parameters.Add("Password", employee.Password, DbType.String);


            using (var connection = _dbContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);

            }

        }
    }
}
