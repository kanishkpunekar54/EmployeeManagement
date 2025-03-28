using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDepartmentController : ControllerBase
    {
        private readonly EmployeeContext _context;

        public EmployeeDepartmentController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeDepartment
        [HttpGet(Name = "Get All EmployeeDepartments")]
        public async Task<ActionResult<IEnumerable<EmployeeDepartmentDTO>>> GetEmployeeDepartments()
        {
            var employeeDepartments = await _context.EmployeeDepartments
                .Include(ed => ed.Employee)
                .Include(ed => ed.Department)
                .Select(ed => new EmployeeDepartmentDTO
                {
                    EmployeeId = ed.EmployeeId,
                    EmployeeName = ed.Employee.FirstName + " " + ed.Employee.LastName,
                    DepartmentId = ed.DepartmentId,
                    DepartmentName = ed.Department.DepartmentName
                })
                .ToListAsync();

            return employeeDepartments;
        }

        // GET: api/EmployeeDepartment/{employeeId}/{departmentId}
        [HttpGet("{employeeId}/{departmentId}", Name = "Get EmployeeDepartment By Id")]
        public async Task<ActionResult<EmployeeDepartmentDTO>> GetEmployeeDepartment(int employeeId, int departmentId)
        {
            var employeeDepartment = await _context.EmployeeDepartments
                .Include(ed => ed.Employee)
                .Include(ed => ed.Department)
                .Select(ed => new EmployeeDepartmentDTO
                {
                    EmployeeId = ed.EmployeeId,
                    EmployeeName = ed.Employee.FirstName + " " + ed.Employee.LastName,
                    DepartmentId = ed.DepartmentId,
                    DepartmentName = ed.Department.DepartmentName
                })
                .FirstOrDefaultAsync(ed => ed.EmployeeId == employeeId && ed.DepartmentId == departmentId);

            if (employeeDepartment == null)
            {
                return NotFound();
            }

            return employeeDepartment;
        }

        // POST: api/EmployeeDepartment
        [HttpPost(Name = "Create EmployeeDepartment")]
        public async Task<ActionResult<EmployeeDepartmentDTO>> CreateEmployeeDepartment(EmployeeDepartmentDTO employeeDepartmentDTO)
        {
            var employeeDepartment = new EmployeeDepartment
            {
                EmployeeId = employeeDepartmentDTO.EmployeeId,
                DepartmentId = employeeDepartmentDTO.DepartmentId
            };

            _context.EmployeeDepartments.Add(employeeDepartment);
            await _context.SaveChangesAsync();

            employeeDepartmentDTO.EmployeeName = (await _context.Employees.FindAsync(employeeDepartmentDTO.EmployeeId)).FirstName + " " + (await _context.Employees.FindAsync(employeeDepartmentDTO.EmployeeId)).LastName;
            employeeDepartmentDTO.DepartmentName = (await _context.Departments.FindAsync(employeeDepartmentDTO.DepartmentId)).DepartmentName;

            return CreatedAtAction(nameof(GetEmployeeDepartment), new { employeeId = employeeDepartment.EmployeeId, departmentId = employeeDepartment.DepartmentId }, employeeDepartmentDTO);
        }

        // PUT: api/EmployeeDepartment/{employeeId}/{departmentId}
        [HttpPut("{employeeId}/{departmentId}", Name = "Update EmployeeDepartment")]
        public async Task<IActionResult> UpdateEmployeeDepartment(int employeeId, int departmentId, EmployeeDepartmentDTO employeeDepartmentDTO)
        {
            if (employeeId != employeeDepartmentDTO.EmployeeId || departmentId != employeeDepartmentDTO.DepartmentId)
            {
                return BadRequest();
            }

            var employeeDepartment = new EmployeeDepartment
            {
                EmployeeId = employeeDepartmentDTO.EmployeeId,
                DepartmentId = employeeDepartmentDTO.DepartmentId
            };

            _context.Entry(employeeDepartment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeDepartmentExists(employeeId, departmentId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/EmployeeDepartment/{employeeId}/{departmentId}
        [HttpDelete("{employeeId}/{departmentId}", Name = "Delete EmployeeDepartment")]
        public async Task<IActionResult> DeleteEmployeeDepartment(int employeeId, int departmentId)
        {
            var employeeDepartment = await _context.EmployeeDepartments
                .FirstOrDefaultAsync(ed => ed.EmployeeId == employeeId && ed.DepartmentId == departmentId);
            if (employeeDepartment == null)
            {
                return NotFound();
            }

            _context.EmployeeDepartments.Remove(employeeDepartment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeDepartmentExists(int employeeId, int departmentId)
        {
            return _context.EmployeeDepartments.Any(ed => ed.EmployeeId == employeeId && ed.DepartmentId == departmentId);
        }
    }
}
