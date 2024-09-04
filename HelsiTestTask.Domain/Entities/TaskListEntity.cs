using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace HelsiTestTask.Domain.Entities
{
    public class TaskListEntity : BaseEntity
    {
        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }

        [BsonElement("ownerId")]
        [BsonRequired]
        public string OwnerId { get; set; }

        [BsonElement("tasks")]
        public List<string> Tasks { get; set; } = new List<string>();

        [BsonElement("sharedWith")]
        public List<string> SharedWith { get; set; } = new List<string>();
    }
}