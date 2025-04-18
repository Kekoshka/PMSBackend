using Microsoft.EntityFrameworkCore;
using PMSBackend.Models;

namespace PMSBackend.Context
{
    public class ApplicationContext :DbContext
    {
        public DbSet<Participation> Participations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Subtask> Subtasks { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base (options)
        {
            Database.EnsureCreated();
        }
    }
}
