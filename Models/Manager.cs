namespace EmployeeManagement.Models
{
    public class Manager
    {
        public int ManagerId { get; set; }
        public int EmployeeId { get; set; }
        public Employee ManagerEmployee { get; set; }
        public Employee Employee { get; set; }
    }
}
