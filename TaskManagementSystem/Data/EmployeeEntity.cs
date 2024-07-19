using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskManagementSystem.Data
{
    public class EmployeeEntity : UserEntity
    {
        [ForeignKey("UserId")]
        public int? ManagerId { get; set; }
        [JsonIgnore]
        public ManagerEntity? Manager { get; set; }
        [JsonIgnore]
        public ICollection<TaskEntity>? Tasks { get; set; }
    }
}
