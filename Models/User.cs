namespace PMSBackend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronumic { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public ICollection<Participation>? Participations { get; set; }
        public ICollection<Task>? Tasks { get; set; }
        public ICollection<Subtask>? Subtasks { get; set; }


        public string GetFIO()
        {
            return $"{this.Surname} {this.Name} {this.Patronumic}";
        }
        public string getFI()
        {
            return $"{this.Surname} {this.Name}";
        }
        public override string ToString()
        {
            return $"{this.Surname} {this.Name} {this.Patronumic}";
        }
    }
}
