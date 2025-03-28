namespace EmployeeManagement.Models
{
    public class ManagerDTO
    {
        public int ManagerId { get; set; }
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }//fetched from employee table
    }
}
