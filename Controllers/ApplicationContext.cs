using Microsoft.EntityFrameworkCore;
using PMSBackend.Common;
using PMSBackend.Models;

namespace PMSBackend.Controllers
{
    public class ApplicationContext :DbContext
    {
        public DbSet<Participation> Participations { get; set; }
        public DbSet<Project> Projects {  get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Subtask> Subtasks { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationContext() 
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.ConnectionString);
        }
    }
}
