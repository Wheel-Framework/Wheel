using Microsoft.AspNetCore.Identity;
using Wheel.Domain;
using Wheel.Domain.Identity;

namespace Wheel.DataSeeders.Identity
{
    public class IdentityDataSeeder(IBasicRepository<Role, string> roleRepository,
            IBasicRepository<User, string> userRepository, UserManager<User> userManager, IUserStore<User> userStore,
            RoleManager<Role> roleManager)
        : IDataSeeder
    {
        public async Task Seed(CancellationToken cancellationToken = default)
        {
            if (!await roleRepository.AnyAsync(a => a.Name == "admin"))
            {
                await roleManager.CreateAsync(new Role("admin", Enums.RoleType.Admin));
            }
            if (!await roleRepository.AnyAsync(a => a.Name == "user"))
            {
                await roleManager.CreateAsync(new Role("user", Enums.RoleType.App));
            }

            if (!await userRepository.AnyAsync(a => a.UserName == "admin"))
            {
                var adminUser = new User();
                await userStore.SetUserNameAsync(adminUser, "admin", cancellationToken);

                var emailStore = (IUserEmailStore<User>)userStore;
                await emailStore.SetEmailAsync(adminUser, "136590076@qq.com", cancellationToken);
                await userManager.CreateAsync(adminUser, "Wheel@2023");
                await userManager.AddToRoleAsync(adminUser, "admin");
                await userManager.UpdateAsync(adminUser);
            }
        }
    }
}
