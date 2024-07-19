using TaskManagementSystem.Data;

namespace TaskManagementSystem.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        string? GetRoleById(int roleId);
        int? GetRoleId(string email, string password);
    }
}
