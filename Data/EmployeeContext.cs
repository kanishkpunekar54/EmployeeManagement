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
            // **Many-to-Many Relationship Between Employees & Departments**
            modelBuilder.Entity<EmployeeDepartment>()
                .HasKey(ed => new { ed.EmployeeId, ed.DepartmentId });

            // **Self-Referencing Relationship (Managers)**
            modelBuilder.Entity<Manager>()
                .HasKey(m => m.ManagerId);

            modelBuilder.Entity<Manager>()
                .HasOne(m => m.Employee)  // Employee being managed
                .WithMany()
                .HasForeignKey(m => m.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Manager>()
                .HasOne(m => m.ManagerEmployee)  // The actual manager
                .WithMany(e => e.Managers)  // A manager can have multiple employees reporting to them
                .HasForeignKey(m => m.ManagerEmployeeId)  // Use `ManagerEmployeeId`
                .OnDelete(DeleteBehavior.Restrict);

            // Specify the store type for the Salary property
            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasColumnType("decimal(18,2)");
        }
    }
}
