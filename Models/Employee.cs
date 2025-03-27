namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime HireDate {  get; set; }
        public decimal Salary { get; set; }
        public List<EmployeeDepartment> EmployeeDepartments { get; set; }
        public List<Manager> Managers { get; set; }


    }
}
