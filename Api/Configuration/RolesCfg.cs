using Microsoft.AspNetCore.Identity;

using Core;
using Core.Entities;

namespace Api.Configuration
{
    public static class RolesCfg
    {
        public static async Task InsertRolesIfDoesntExist(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<RoleModel>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<UserModel>>();
            string[] rolesList = {
                Constants.ROLE_NAME_ADMIN,
                Constants.ROLE_NAME_DELIVERY_PERSON
            };

            IdentityResult result;
            foreach (string role in rolesList)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                    result = await roleManager.CreateAsync(new RoleModel() { Name = role });
            }
        }
    }
}
