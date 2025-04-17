namespace PMSBackend.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Participation> Participations { get; set; }
    }
}
