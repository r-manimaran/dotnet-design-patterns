using eCommerceApp.Domain.Entities.Identity;
using eCommerceApp.Domain.Interfaces.Authentication;
using Microsoft.AspNetCore.Identity;

namespace eCommerceApp.Infrastructure.Repositories.Authentication;

public class RoleManagement(UserManager<AppUser> userManager) : IRoleManagement
{
    public async Task<bool> AddUserToRole(AppUser user, string roleName)
    {
        return (await userManager.AddToRoleAsync(user, roleName)).Succeeded;
    }

    public async Task<string?> GetUserRole(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        return (await userManager.GetRolesAsync(user!)).FirstOrDefault();
    }
}
