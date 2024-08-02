using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using AlleyCatBarbers.Data;
using AlleyCatBarbers.Models;
using Microsoft.EntityFrameworkCore;

namespace AlleyCatBarbers.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.EnsureCreated();

            // Create roles if they do not exist
            string[] roleNames = { "Admin", "Customer", "Staff" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            await CreateUser(userManager, "admin@admin.com", "password", "Admin");
            await CreateUser(userManager, "staff@staff.com", "password", "Staff");
            await CreateUser(userManager, "customer@customer.com", "password", "Customer");


        }

        public static async Task CreateUser(UserManager<ApplicationUser> userManager, string email, 
            string password, string role)
        {
            

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var createUser = await userManager.CreateAsync(user, password);
                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    throw new Exception("Failed to create new user.");
                }
            }
            else
            {
                // Ensure the user is assigned to the role
                if (!await userManager.IsInRoleAsync(user, role))
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
