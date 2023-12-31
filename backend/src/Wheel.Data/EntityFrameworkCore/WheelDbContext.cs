﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Wheel.Domain.FileStorages;
using Wheel.Domain.Identity;
using Wheel.Domain.Localization;
using Wheel.Domain.Menus;
using Wheel.Domain.Permissions;
using Wheel.Domain.Settings;

namespace Wheel.EntityFrameworkCore
{
    public class WheelDbContext(DbContextOptions<WheelDbContext> options) : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options)
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
        public DbSet<RoleMenu> RoleMenus { get; set; }
        #endregion
        #region Setting
        public DbSet<SettingGroup> SettingGroups { get; set; }
        public DbSet<SettingValue> SettingValues { get; set; }
        #endregion

        #region FileStorage
        public DbSet<FileStorage> FileStorages { get; set; }
        #endregion

        private StoreOptions? GetStoreOptions() => this.GetService<IDbContextOptions>()
                            .Extensions.OfType<CoreOptionsExtension>()
                            .FirstOrDefault()?.ApplicationServiceProvider
                            ?.GetService<IOptions<IdentityOptions>>()
                            ?.Value?.Stores;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureIdentity(builder);
            ConfigureLocalization(builder);
            ConfigurePermissionGrants(builder);
            ConfigureMenus(builder);
            ConfigureSettings(builder);
            ConfigureFileStorage(builder);
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
                b.Property(a => a.Key).HasMaxLength(256);
                b.Property(a => a.Value).HasMaxLength(1024);
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
    }
}
