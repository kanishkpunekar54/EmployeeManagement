using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Data
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<EmployeeDepartment> EmployeeDepartments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Many-to-Many Relationship
            modelBuilder.Entity<EmployeeDepartment>()
                .HasKey(ed => new { ed.EmployeeId, ed.DepartmentId });

            // Configure Self-Referencing Relationship (Managers)
            modelBuilder.Entity<Manager>()
                .HasKey(m => new { m.ManagerId, m.EmployeeId });

            modelBuilder.Entity<Manager>()
                .HasOne(m => m.ManagerEmployee)
                .WithMany(e => e.Managers)
                .HasForeignKey(m => m.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Manager>()
                .HasOne(m => m.Employee)
                .WithMany()
                .HasForeignKey(m => m.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}