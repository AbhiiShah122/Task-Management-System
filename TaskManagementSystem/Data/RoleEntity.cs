using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskManagementSystem.Data.Helper;

namespace TaskManagementSystem.Data
{
    public class RoleEntity
    {
        [Key]
        public int RoleId { get; set; }
        public required UserRole RoleName { get; set; }
        [JsonIgnore]
        public ICollection<UserEntity>? Users { get; set; }
    }
}
