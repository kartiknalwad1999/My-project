using EmployeeServiceDepartmentService.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EmployeeServiceDepartmentService.data_access
{
    public class EmpDepartmentDbContext : DbContext
    {
        public EmpDepartmentDbContext(DbContextOptions<EmpDepartmentDbContext> options) : base(options) { }

        // Employee-related tables
        public DbSet<Employees> Employees { get; set; }
        public DbSet<JobContracts> JobContracts { get; set; }
        public DbSet<Attachments> Attachments { get; set; }

        // Department-related tables
        public DbSet<Departments> Departments { get; set; }
        public DbSet<DepartmentRoles> DepartmentRoles { get; set; }

       
    }

}
