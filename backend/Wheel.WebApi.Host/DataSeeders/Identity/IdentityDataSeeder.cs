using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Wheel.Domain;
using Wheel.Domain.Identity;

namespace Wheel.DataSeeders.Identity
{
    public class IdentityDataSeeder : IDataSeeder
    {
        private readonly IBasicRepository<Role, string> _roleRepository;
        private readonly IBasicRepository<User, string> _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;

        public IdentityDataSeeder(IBasicRepository<Role, string> roleRepository, IBasicRepository<User, string> userRepository, UserManager<User> userManager, IUserStore<User> userStore)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _userStore = userStore;
        }

        public async Task Seed(CancellationToken cancellationToken = default)
        {
            if(!await _roleRepository.AnyAsync(a=>a.Name == "admin"))
            {
                await _roleRepository.InsertAsync(new Role("admin", Enums.RoleType.Admin), true);
            }
            if(!await _roleRepository.AnyAsync(a=>a.Name == "user"))
            {
                await _roleRepository.InsertAsync(new Role("user", Enums.RoleType.App), true);
            }

            if(!await _userRepository.AnyAsync(a=>a.UserName == "admin"))
            {
                var adminUser = new User();
                await _userStore.SetUserNameAsync(adminUser, "admin", cancellationToken);

                var emailStore = (IUserEmailStore<User>)_userStore;
                await emailStore.SetEmailAsync(adminUser, "136590076@qq.com", cancellationToken);
                await _userManager.CreateAsync(adminUser, "Wheel@2023");
                await _userManager.AddToRoleAsync(adminUser, "admin");
                await _userManager.UpdateAsync(adminUser);
            }
        }
    }
}
