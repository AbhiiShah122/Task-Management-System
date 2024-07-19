using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Data.Context;
using TaskManagementSystem.Repositories.Interfaces;

namespace TaskManagementSystem.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly TaskManagementContext _context;

        public RoleRepository(TaskManagementContext context)
        {
            _context = context;
        }

        public string? GetRoleById(int roleId)
        {
            var role = _context.Roles.Find(roleId);
            return role?.RoleName.ToString();
        }

        public int? GetRoleId(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            return user?.RoleId ?? 0;
        }
    }
}
