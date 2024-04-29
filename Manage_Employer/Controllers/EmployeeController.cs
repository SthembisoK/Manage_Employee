using Manage_Employee.Interfaces;
using Manage_Employer.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manage_Employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepo;

        public EmployeeController(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        [HttpGet]

        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var employees = await _employeeRepo.GetEmployees();
                return Ok(employees);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("{id}", Name = "EmployeeById")]
        public async Task<IActionResult> GetEmployees(int id)
        {
            var employee = await _employeeRepo.GetEmployee(id);
            if (employee == null)
                return NotFound();

            return Ok(employee);

        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployees([FromBody] EmployeeDTO employee)
        {
            var createdEmployee = await _employeeRepo.CreateEmployees(employee);

            // Return the created schedule along with a success message
            return Ok(new { message = "Schedule successfully added", createdEmployee });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeDTO employee)
        {
            var dbemployee = await _employeeRepo.GetEmployee(id);
            if (dbemployee == null)
                return NotFound("Please enter valid ID Baba.");

            await _employeeRepo.UpdateEmployee(id, employee);

            var updatedEmployee = await _employeeRepo.GetEmployee(id);

            return Ok(new { message = "Schedule Updated Succesfully", employee = updatedEmployee });

        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var dbemployee = await _employeeRepo.GetEmployee(id);
            if (dbemployee == null)

                return NotFound("Cannot find the employee, Please insert the Correct Id");

            await _employeeRepo.DeleteEmployee(id);

            return Ok("Employee Deleted Successfully");
        }

    }
}
