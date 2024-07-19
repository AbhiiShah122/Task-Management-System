using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManagementSystem.Data
{
    public class TaskNoteEntity
    {
        [Key]
        public int TaskNoteId { get; set; }
        public required string Note { get; set; }
        public int TaskId { get; set; }
        [JsonIgnore]
        public TaskEntity? Task { get; set; }
    }
}