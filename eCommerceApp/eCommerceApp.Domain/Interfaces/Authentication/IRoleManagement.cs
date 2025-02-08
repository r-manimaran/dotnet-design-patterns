using eCommerceApp.Domain.Entities.Identity;

namespace eCommerceApp.Domain.Interfaces.Authentication;

public interface IRoleManagement
{
    Task<string?> GetUserRole(string email);
    Task<bool> AddUserToRole(AppUser user, string roleName);
}
