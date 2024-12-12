﻿using Handlingform.Models;
using Microsoft.EntityFrameworkCore;

namespace Handlingform.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options
    ) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Department> Departments { get; set; }

    }
}
