using Microsoft.AspNetCore.Identity;
using Wheel.Administrator.Domain.Identity;
using Wheel.Domain;

namespace Wheel.DataSeeders.Identity
{
    public class IdentityDataSeeder(IBasicRepository<BackendRole, string> roleRepository,
            IBasicRepository<BackendUser, string> userRepository, UserManager<BackendUser> userManager, IUserStore<BackendUser> userStore,
            RoleManager<BackendRole> roleManager)
        : IDataSeeder
    {
        public async Task Seed(CancellationToken cancellationToken = default)
        {
            if (!await roleRepository.AnyAsync(a => a.Name == "admin"))
            {
                await roleManager.CreateAsync(new BackendRole("admin"));
            }

            if (!await userRepository.AnyAsync(a => a.UserName == "admin"))
            {
                var adminUser = new BackendUser();
                await userStore.SetUserNameAsync(adminUser, "admin", cancellationToken);

                var emailStore = (IUserEmailStore<BackendUser>)userStore;
                await emailStore.SetEmailAsync(adminUser, "136590076@qq.com", cancellationToken);
                await userManager.CreateAsync(adminUser, "Wheel@2023");
                await userManager.AddToRoleAsync(adminUser, "admin");
                await userManager.UpdateAsync(adminUser);
            }
        }
    }
}
