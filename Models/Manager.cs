namespace EmployeeManagement.Models
{
    public class Manager
    {
        public int ManagerId { get; set; }  // Primary Key
        public int EmployeeId { get; set; }  // Foreign Key referencing Employee

        // The employee being managed
        public Employee Employee { get; set; }

        // The actual manager (who is also an Employee)
        public int? ManagerEmployeeId { get; set; }  // Nullable to avoid circular dependency
        public Employee ManagerEmployee { get; set; }
    }
}
