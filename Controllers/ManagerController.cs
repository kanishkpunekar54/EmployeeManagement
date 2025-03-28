using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly EmployeeContext _context;

        public ManagerController(EmployeeContext context)
        {
            _context = context;
        }

        // **Get all managers**
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ManagerDTO>>> GetManagers()
        {
            var managers = await _context.Managers
                .Include(m => m.Employee) // Fetch Employee details
                .Select(m => new ManagerDTO
                {
                    ManagerId = m.ManagerId,
                    EmployeeId = m.EmployeeId,
                    EmployeeName = m.Employee.FirstName + " " + m.Employee.LastName
                })
                .ToListAsync();

            return Ok(managers);
        }

        // **Get manager by ID**
        [HttpGet("{id}")]
        public async Task<ActionResult<ManagerDTO>> GetManager(int id)
        {
            var manager = await _context.Managers
                .Include(m => m.Employee)
                .Where(m => m.ManagerId == id)
                .Select(m => new ManagerDTO
                {
                    ManagerId = m.ManagerId,
                    EmployeeId = m.EmployeeId,
                    EmployeeName = m.Employee.FirstName + " " + m.Employee.LastName
                })
                .FirstOrDefaultAsync();

            if (manager == null)
            {
                return NotFound();
            }

            return Ok(manager);
        }

        // **Create a new manager**
        [HttpPost]
        public async Task<ActionResult<ManagerDTO>> CreateManager(ManagerDTO managerDTO)
        {
            var employee = await _context.Employees.FindAsync(managerDTO.EmployeeId);
            if (employee == null)
            {
                return BadRequest("Invalid EmployeeId");
            }

            var manager = new Manager
            {
                EmployeeId = managerDTO.EmployeeId
            };

            _context.Managers.Add(manager);
            await _context.SaveChangesAsync();

            managerDTO.ManagerId = manager.ManagerId;
            managerDTO.EmployeeName = employee.FirstName + " " + employee.LastName;

            return CreatedAtAction(nameof(GetManager), new { id = manager.ManagerId }, managerDTO);
        }

        // **Update a manager**
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateManager(int id, ManagerDTO managerDTO)
        {
            if (id != managerDTO.ManagerId)
            {
                return BadRequest("Manager ID mismatch");
            }

            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(managerDTO.EmployeeId);
            if (employee == null)
            {
                return BadRequest("Invalid EmployeeId");
            }

            manager.EmployeeId = managerDTO.EmployeeId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // **Delete a manager**
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManager(int id)
        {
            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }

            _context.Managers.Remove(manager);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
