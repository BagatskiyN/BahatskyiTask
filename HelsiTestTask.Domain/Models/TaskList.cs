namespace HelsiTestTask.Domain.Models
{
    public class TaskList
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Name { get; set; }

        public string OwnerId { get; set; }

        public List<string> Tasks { get; set; }

        public List<string> SharedWith { get; set; }
    }
}