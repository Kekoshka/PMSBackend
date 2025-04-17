namespace PMSBackend.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? ProjectId { get; set; }
        public int ProjectStatusId { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public Project Project { get; set; }
        public ICollection<User>? Users { get; set; }//Ответственные за задачу
        public ICollection<Subtask>? Subtasks { get; set; }
    }
}
