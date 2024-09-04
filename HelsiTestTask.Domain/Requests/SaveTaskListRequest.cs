using System.ComponentModel.DataAnnotations;

namespace HelsiTestTask.Domain.Requests
{
    public class SaveTaskListRequest
    {
        [StringLength(255, MinimumLength = 1)]
        public string Name { get; set; }

        public List<string> Tasks { get; set; }

        public List<string> SharedWith { get; set; }
    }
}
