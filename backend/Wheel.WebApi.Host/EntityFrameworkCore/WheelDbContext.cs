using Castle.Core.Resource;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using Wheel.Domain.Identity;
using Wheel.Domain.Localization;
using Wheel.Domain.Menus;
using Wheel.Domain.Permissions;

namespace Wheel.EntityFrameworkCore
{
    public class WheelDbContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        #region Localization
        public DbSet<LocalizationCulture> Cultures { get; set; }
        public DbSet<LocalizationResource> Resources { get; set; }
        #endregion
        #region Permission
        public DbSet<PermissionGrant> PermissionGrants { get; set; }
        #endregion

        #region Menu
        public DbSet<Menu> Menus { get; set; }
        #endregion

        private StoreOptions? GetStoreOptions() => this.GetService<IDbContextOptions>()
                            .Extensions.OfType<CoreOptionsExtension>()
                            .FirstOrDefault()?.ApplicationServiceProvider
                            ?.GetService<IOptions<IdentityOptions>>()
                            ?.Value?.Stores;

        public WheelDbContext(DbContextOptions<WheelDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureIdentity(builder);
            ConfigureLocalization(builder);
            ConfigurePermissionGrants(builder);
            ConfigureMenus(builder);
        }

        void ConfigureIdentity(ModelBuilder builder)
        {
            var storeOptions = GetStoreOptions();
            var maxKeyLength = storeOptions?.MaxLengthForKeys ?? 0;

            builder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
                b.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");
                b.ToTable("Users");
                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                b.Property(u => u.Id).HasMaxLength(36);
                b.Property(u => u.UserName).HasMaxLength(256);
                b.Property(u => u.NormalizedUserName).HasMaxLength(256);
                b.Property(u => u.Email).HasMaxLength(256);
                b.Property(u => u.NormalizedEmail).HasMaxLength(256);
                b.Property(u => u.CreationTime).HasDefaultValue(DateTimeOffset.Now);

                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
            builder.Entity<UserClaim>(b =>
            {
                b.HasKey(uc => uc.Id);
                b.ToTable("UserClaims");
            });
            builder.Entity<UserLogin>(b =>
            {
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

                if (maxKeyLength > 0)
                {
                    b.Property(l => l.LoginProvider).HasMaxLength(maxKeyLength);
                    b.Property(l => l.ProviderKey).HasMaxLength(maxKeyLength);
                }

                b.ToTable("UserLogins");
            });
            builder.Entity<UserToken>(b =>
            {
                b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

                if (maxKeyLength > 0)
                {
                    b.Property(t => t.LoginProvider).HasMaxLength(maxKeyLength);
                    b.Property(t => t.Name).HasMaxLength(maxKeyLength);
                }
                b.ToTable("UserTokens");
            });
            builder.Entity<Role>(b =>
            {
                b.HasKey(r => r.Id);
                b.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();
                b.ToTable("Roles");
                b.Property(u => u.Id).HasMaxLength(36);
                b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

                b.Property(u => u.Name).HasMaxLength(256);
                b.Property(u => u.NormalizedName).HasMaxLength(256);

                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });

            builder.Entity<RoleClaim>(b =>
            {
                b.HasKey(rc => rc.Id);
                b.ToTable("RoleClaims");
            });

            builder.Entity<UserRole>(b =>
            {
                b.HasKey(r => new { r.UserId, r.RoleId });
                b.ToTable("UserRoles");
            });
        }

        void ConfigureLocalization(ModelBuilder builder)
        {
            builder.Entity<LocalizationCulture>(b =>
            {
                b.Property(a => a.Id).ValueGeneratedOnAdd();
                b.ToTable("LocalizationCulture"); 
                b.Property(a => a.Name).HasMaxLength(32);
                b.HasMany(a => a.Resources);
            });
            builder.Entity<LocalizationResource>(b =>
            {
                b.Property(a => a.Id).ValueGeneratedOnAdd();
                b.ToTable("LocalizationResource");
                b.HasOne(a => a.Culture);
                b.HasIndex(a => a.CultureId);
            });
        }

        void ConfigurePermissionGrants(ModelBuilder builder)
        {
            builder.Entity<PermissionGrant>(b =>
            {
                b.HasKey(o => o.Id);
                b.Property(o => o.Permission).HasMaxLength(128);
                b.Property(o => o.GrantValue).HasMaxLength(128);
                b.Property(o => o.GrantType).HasMaxLength(32);
            });
        }
        void ConfigureMenus(ModelBuilder builder)
        {
            builder.Entity<Menu>(b =>
            {
                b.HasKey(o => o.Id);
                b.Property(o => o.Permission).HasMaxLength(128);
                b.Property(o => o.Path).HasMaxLength(128);
                b.Property(o => o.Name).HasMaxLength(128);
                b.Property(o => o.Icon).HasMaxLength(128);
                b.Property(o => o.DisplayName).HasMaxLength(128);
                b.HasMany(o => o.Children).WithOne(o => o.Parent);
                b.HasIndex(o => o.ParentId);
            });
            builder.Entity<RoleMenu>(b =>
            {
                b.HasKey(o => new { o.MenuId, o.RoleId });
                b.Property(o => o.RoleId).HasMaxLength(36);
            });
        }
    }
}
