using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskManagementSystem.Data
{
    public class TaskEntity
    {
        [Key]
        public int TaskId { get; set; }
        public required string Title { get; set; }

        public required string Description { get; set; }
        public required DateTime DueDate { get; set; }
        public DateTime? CompletedOn { get; set; }
        // Improvements: can have multiple Statuses as well.
        public bool IsCompleted { get; set; }
        [ForeignKey("UserId")]
        public required int EmployeeId { get; set; }
        [JsonIgnore]
        public EmployeeEntity? Employee { get; set; }
        public ICollection<TaskAttachmentEntity>? Attachments { get; set; }
        public ICollection<TaskNoteEntity>? Notes { get; set; }
    }
}
