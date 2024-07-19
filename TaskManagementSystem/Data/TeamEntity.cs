using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManagementSystem.Data
{
    public class TeamEntity
    {
        [Key]
        public int TeamId { get; set; }

        public required string TeamName { get; set; }

        [JsonIgnore]
        public ICollection<UserEntity>? Users { get; set; }
    }
}