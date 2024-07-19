using System.Text.Json.Serialization;

namespace TaskManagementSystem.Data
{
    public class ManagerEntity : UserEntity
    {
        [JsonIgnore]
        public ICollection<EmployeeEntity>? Employees { get; set; }
    }
}
