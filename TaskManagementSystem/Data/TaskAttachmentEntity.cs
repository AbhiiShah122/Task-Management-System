using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManagementSystem.Data
{
    public class TaskAttachmentEntity
    {
        [Key]
        public int TaskAttachmentId { get; set; }
        public required string FileName { get; set; }
        public required string FilePath { get; set; }
        public int TaskId { get; set; }
        [JsonIgnore]
        public TaskEntity? Task { get; set; }
    }
}