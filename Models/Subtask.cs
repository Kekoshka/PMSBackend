namespace PMSBackend.Models
{
    public class Subtask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? TaskId { get; set; }
        public int? UserId { get; set; }//ответственный за подзадачу
        public int ProjectStatusId { get; set; }
        public User? User { get; set; }
        public Task Task { get; set; }
        public Status Status{ get; set; }
    }
}
