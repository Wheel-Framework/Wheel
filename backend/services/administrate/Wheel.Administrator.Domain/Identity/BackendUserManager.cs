using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Administrator.Domain.Identity;
using Wheel.Domain;

namespace Wheel.Administrator.Identity
{
    public class BackendUserManager : UserManager<BackendUser>
    {
        private readonly IServiceProvider _services;
        private readonly IBasicRepository<BackendUser, string> _userRepository;
        public BackendUserManager(IUserStore<BackendUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<BackendUser> passwordHasher, IEnumerable<IUserValidator<BackendUser>> userValidators, IEnumerable<IPasswordValidator<BackendUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<BackendUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _services = services;
        }
        public async Task<BackendUser?> FindByPhoneNumberAsync(string phoneNumber)
        {
            ThrowIfDisposed();
            if(phoneNumber.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(phoneNumber));

            var user = await _userRepository.FindAsync(a => a.PhoneNumber == phoneNumber).ConfigureAwait(false);

            // Need to potentially check all keys
            if (user == null && Options.Stores.ProtectPersonalData)
            {
                var keyRing = _services.GetService<ILookupProtectorKeyRing>();
                var protector = _services.GetService<ILookupProtector>();
                if (keyRing != null && protector != null)
                {
                    foreach (var key in keyRing.GetAllKeyIds())
                    {
                        var oldKey = protector.Protect(key, phoneNumber);
                        user = await Store.FindByNameAsync(oldKey, CancellationToken).ConfigureAwait(false);
                        if (user != null)
                        {
                            return user;
                        }
                    }
                }
            }
            return user;
        }
    }
}
