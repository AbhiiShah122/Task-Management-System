using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;

namespace TaskManagementSystem.Data
{
    public abstract class UserEntity
    {
        [Key]
        public int UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int? TeamId { get; set; }

        [JsonIgnore]
        public TeamEntity? Team { get; set; }
        public int RoleId { get; set; }

        [JsonIgnore]
        public RoleEntity? Role { get; set; }
    }

}
