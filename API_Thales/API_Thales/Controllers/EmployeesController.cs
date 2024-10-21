using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using DataAccess;
using Entities;
namespace API_Thales.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;
        private readonly EmployeeManager _employeeManager;
        public EmployeesController(EmployeeService employeeService, EmployeeManager employeeManager)
        {
            _employeeService = employeeService;
            _employeeManager = employeeManager;
        }

        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeService.GetEmployeesAsync();
            if(employees == null) 
                return NotFound();

            //Annual Salary for each employee
            foreach (var employee in employees)
            {
                employee.annualSalary = _employeeManager.CalculateAnnualSalary(employee.salary);
            }
            return Ok(employees);
        }

        /// <summary>
        /// Get Employee by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeByIdAsync(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            //Annual Salary for one employee
            employee.annualSalary = _employeeManager.CalculateAnnualSalary(employee.salary);
            return Ok(employee);
        }
    }
}


