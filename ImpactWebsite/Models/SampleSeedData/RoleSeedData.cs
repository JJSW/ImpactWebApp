using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ImpactWebsite.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ImpactWebsite.Models
{
    public class RoleSeedData
    {
        public static async void Initialize(IServiceProvider isp)
        {
            var context = isp.GetService<ApplicationDbContext>();
            var roleStore = new RoleStore<IdentityRole>(context);

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                await roleStore.CreateAsync(new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                });
            }

            if (!context.Roles.Any(r => r.Name == "Member"))
            {
                await roleStore.CreateAsync(new IdentityRole
                {
                    Name = "Member",
                    NormalizedName = "MEMBER"
                });
            }

            var admin = new ApplicationUser
            {
                FirstName = "admin",
                LastName = "admin",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                PhoneNumber = "000-000-0000",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                CompanyName = "Fairbank Financial",
                NewsletterRequired = true
            };

            var member = new ApplicationUser
            {
                FirstName = "temp",
                LastName = "temp",
                Email = "temp@user.com",
                NormalizedEmail = "TEMP@USER.COM",
                UserName = "temp@user.com",
                NormalizedUserName = "TEMP@USER.COM",
                PhoneNumber = "000-000-0000",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                CompanyName = "Retailed Investor",
                NewsletterRequired = true
            };

            if (!context.Users.Any(u => u.UserName == admin.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(admin, "Password1!");
                admin.PasswordHash = hashed;

                var userStore = new UserStore<ApplicationUser>(context);
                var result = userStore.CreateAsync(admin);
            }

            if (!context.Users.Any(u => u.UserName == member.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(member, "Password2!");
                member.PasswordHash = hashed;

                var userStore = new UserStore<ApplicationUser>(context);
                var result = userStore.CreateAsync(member);

                await AssignRoles(isp, admin.Email, "Admin");
                await AssignRoles(isp, member.Email, "Member");
            }

            await context.SaveChangesAsync();
        }

        public static async Task<IdentityResult> AssignRoles(IServiceProvider services, string username, string role)
        {
            UserManager<ApplicationUser> _userManager = services.GetService<UserManager<ApplicationUser>>();
            ApplicationUser user = await _userManager.FindByNameAsync(username);
            var result = await _userManager.AddToRoleAsync(user, role);

            return result;
        }
    }

}