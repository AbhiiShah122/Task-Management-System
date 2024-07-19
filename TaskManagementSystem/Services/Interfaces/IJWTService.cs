using TaskManagementSystem.Data;
using TaskManagementSystem.Model.Response;

namespace TaskManagementSystem.Services.Interfaces
{
    public interface IJWTService
    {
        string GenerateToken(string email, string password);
    }
}
