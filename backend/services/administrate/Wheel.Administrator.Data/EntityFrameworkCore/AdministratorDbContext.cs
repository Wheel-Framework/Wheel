using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Wheel.Administrator.Domain.FileStorages;
using Wheel.Administrator.Domain.Identity;
using Wheel.Administrator.Domain.Menus;
using Wheel.Administrator.Domain.Permissions;
using Wheel.Administrator.Domain.Settings;

namespace Wheel.Administrator.EntityFrameworkCore
{
    public class AdministratorDbContext : DbContext
    {
        public AdministratorDbContext(DbContextOptions<AdministratorDbContext> options) : base(options)
        {
        }
        #region Identity

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of Users.
        /// </summary>
        public virtual DbSet<BackendUser> Users { get; set; } = default!;

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User claims.
        /// </summary>
        public virtual DbSet<BackendUserClaim> UserClaims { get; set; } = default!;

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User logins.
        /// </summary>
        public virtual DbSet<BackendUserLogin> UserLogins { get; set; } = default!;

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User tokens.
        /// </summary>
        public virtual DbSet<BackendUserToken> UserTokens { get; set; } = default!;
        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User roles.
        /// </summary>
        public virtual DbSet<BackendUserRole> UserRoles { get; set; } = default!;

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of roles.
        /// </summary>
        public virtual DbSet<BackendRole> Roles { get; set; } = default!;

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of role claims.
        /// </summary>
        public virtual DbSet<BackendRoleClaim> RoleClaims { get; set; } = default!;
        #endregion
        #region Permission
        public DbSet<PermissionGrant> PermissionGrants { get; set; }
        #endregion

        #region Menu
        public DbSet<Menu> Menus { get; set; }
        public DbSet<RoleMenu> RoleMenus { get; set; }
        #endregion
        #region Setting
        public DbSet<SettingGroup> SettingGroups { get; set; }
        public DbSet<SettingValue> SettingValues { get; set; }
        #endregion

        #region FileStorage
        public DbSet<FileStorage> FileStorages { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureIdentity(modelBuilder);
            ConfigurePermissionGrants(modelBuilder);
            ConfigureMenus(modelBuilder);
            ConfigureSettings(modelBuilder);
            ConfigureFileStorage(modelBuilder);
        }

        void ConfigureIdentity(ModelBuilder builder)
        {
            var storeOptions = GetStoreOptions();
            var maxKeyLength = storeOptions?.MaxLengthForKeys ?? 0;
            if (maxKeyLength == 0)
            {
                maxKeyLength = 128;
            }
            builder.Entity<BackendUser>(b =>
            {
                // Primary key
                b.HasKey(u => u.Id);

                // Indexes for "normalized" username and email, to allow efficient lookups
                b.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
                b.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");

                // Maps to the BackendUsers table
                b.ToTable("BackendUsers");

                // A concurrency token for use with the optimistic concurrency checking
                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                // Limit the size of columns to use efficient database types
                b.Property(u => u.UserName).HasMaxLength(256);
                b.Property(u => u.NormalizedUserName).HasMaxLength(256);
                b.Property(u => u.Email).HasMaxLength(256);
                b.Property(u => u.NormalizedEmail).HasMaxLength(256);

                // The relationships between User and other entity types
                // Note that these relationships are configured with no navigation properties

                // Each User can have many UserClaims
                b.HasMany(a=>a.Claims).WithOne(a => a.User).HasForeignKey(uc => uc.UserId).IsRequired();

                // Each User can have many UserLogins
                b.HasMany(a=>a.Logins).WithOne(a => a.User).HasForeignKey(ul => ul.UserId).IsRequired();

                // Each User can have many UserTokens
                b.HasMany(a=>a.Tokens).WithOne(a => a.User).HasForeignKey(ut => ut.UserId).IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(a=>a.UserRoles).WithOne(a => a.User).HasForeignKey(ur => ur.UserId).IsRequired();
            });

            builder.Entity<BackendUserClaim>(b =>
            {
                // Primary key
                b.HasKey(uc => uc.Id);
                b.HasOne(a => a.User);
                // Maps to the BackendUserClaims table
                b.ToTable("BackendUserClaims");
            });

            builder.Entity<BackendUserLogin>(b =>
            {
                // Composite primary key consisting of the LoginProvider and the key to use
                // with that provider
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

                b.HasOne(a => a.User);
                // Limit the size of the composite key columns due to common DB restrictions
                b.Property(l => l.LoginProvider).HasMaxLength(128);
                b.Property(l => l.ProviderKey).HasMaxLength(128);

                // Maps to the BackendUserLogins table
                b.ToTable("BackendUserLogins");
            });

            builder.Entity<BackendUserToken>(b =>
            {
                // Composite primary key consisting of the UserId, LoginProvider and Name
                b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

                b.HasOne(a => a.User);
                // Limit the size of the composite key columns due to common DB restrictions
                b.Property(t => t.LoginProvider).HasMaxLength(maxKeyLength);
                b.Property(t => t.Name).HasMaxLength(maxKeyLength);

                // Maps to the BackendUserTokens table
                b.ToTable("BackendUserTokens");
            });

            builder.Entity<BackendRole>(b =>
            {
                // Primary key
                b.HasKey(r => r.Id);

                // Index for "normalized" role name to allow efficient lookups
                b.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique();

                // Maps to the BackendRoles table
                b.ToTable("BackendRoles");

                // A concurrency token for use with the optimistic concurrency checking
                b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

                // Limit the size of columns to use efficient database types
                b.Property(u => u.Name).HasMaxLength(256);
                b.Property(u => u.NormalizedName).HasMaxLength(256);

                // The relationships between Role and other entity types
                // Note that these relationships are configured with no navigation properties

                // Each Role can have many entries in the UserRole join table
                b.HasMany(a=>a.UserRoles).WithOne(a => a.Role).HasForeignKey(ur => ur.RoleId).IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(a=>a.RoleClaims).WithOne(a => a.Role).HasForeignKey(rc => rc.RoleId).IsRequired();
            });

            builder.Entity<BackendRoleClaim>(b =>
            {
                // Primary key
                b.HasKey(rc => rc.Id);
                b.HasOne(a => a.Role);
                // Maps to the BackendRoleClaims table
                b.ToTable("BackendRoleClaims");
            });

            builder.Entity<BackendUserRole>(b =>
            {
                // Primary key
                b.HasKey(r => new { r.UserId, r.RoleId });

                b.HasOne(a => a.Role);
                b.HasOne(a => a.User);
                // Maps to the BackendUserRoles table
                b.ToTable("BackendUserRoles");
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
        void ConfigureSettings(ModelBuilder builder)
        {
            builder.Entity<SettingGroup>(b =>
            {
                b.HasKey(o => o.Id);
                b.Property(o => o.Name).HasMaxLength(128);
                b.Property(o => o.NormalizedName).HasMaxLength(128);
                b.HasMany(o => o.SettingValues).WithOne(o => o.SettingGroup);
                b.HasIndex(o => o.Name);
            });
            builder.Entity<SettingValue>(b =>
            {
                b.HasKey(o => o.Id);
                b.Property(o => o.Key).HasMaxLength(128);
                b.Property(o => o.SettingScopeKey).HasMaxLength(128);
                b.Property(o => o.ValueType).HasMaxLength(2048);
                b.HasOne(o => o.SettingGroup).WithMany(o => o.SettingValues);
                b.HasIndex(o => o.Key);
            });
        }
        void ConfigureFileStorage(ModelBuilder builder)
        {
            builder.Entity<FileStorage>(b =>
            {
                b.HasKey(o => o.Id);
                b.Property(o => o.FileName).HasMaxLength(256);
                b.Property(o => o.Path).HasMaxLength(256);
                b.Property(o => o.ContentType).HasMaxLength(32);
                b.Property(o => o.Provider).HasMaxLength(32);
            });
        }


        private StoreOptions? GetStoreOptions() => this.GetService<IDbContextOptions>()
                        .Extensions.OfType<CoreOptionsExtension>()
                        .FirstOrDefault()?.ApplicationServiceProvider
                        ?.GetService<IOptions<IdentityOptions>>()
                        ?.Value?.Stores;
    }
}
