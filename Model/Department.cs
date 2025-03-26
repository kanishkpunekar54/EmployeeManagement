namespace EmployeeManagement.Model
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public List<EmployeeDepartment> EmployeeDepartments { get; set; }


    }
}
