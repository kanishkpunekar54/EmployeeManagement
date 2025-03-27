using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _context;

        public EmployeeController(EmployeeContext context)
        {
            _context = context;
        }

        // **GET: api/employee**
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees()
        {
            return await _context.Employees
                .Select(e => new EmployeeDTO
                {
                    EmployeeId = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    HireDate = e.HireDate,
                    Salary = e.Salary
                })
                .ToListAsync();
        }

        // **GET: api/employee/{id}**
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            return new EmployeeDTO
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Phone = employee.Phone,
                HireDate = employee.HireDate,
                Salary = employee.Salary
            };
        }

        // **POST: api/employee (Create Employee)**
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(EmployeeDTO employeeDto)
        {
            var employee = new Employee
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Email = employeeDto.Email,
                Phone = employeeDto.Phone,
                HireDate = employeeDto.HireDate,
                Salary = employeeDto.Salary
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employeeDto);
        }

        // **PUT: api/employee/{id} (Update Employee)**
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeDTO employeeDto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            employee.FirstName = employeeDto.FirstName;
            employee.LastName = employeeDto.LastName;
            employee.Email = employeeDto.Email;
            employee.Phone = employeeDto.Phone;
            employee.HireDate = employeeDto.HireDate;
            employee.Salary = employeeDto.Salary;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> SearchEmployees([FromQuery] int? empId, [FromQuery] string? name)
        {
            var query = _context.Employees.AsQueryable();

            if (empId.HasValue)
                query = query.Where(e => e.EmployeeId == empId.Value);

            if (!string.IsNullOrEmpty(name))
                query = query.Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name));

            var result = await query.Select(e => new EmployeeDTO
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                HireDate = e.HireDate,
                Salary = e.Salary
            }).ToListAsync();

            return Ok(result);
        }


        // **DELETE: api/employee/{id} (Delete Employee)**
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
