using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using AlleyCatBarbers.Data;
using AlleyCatBarbers.Models;
using Microsoft.EntityFrameworkCore;
using AlleyCatBarbers.Services;
using System.Collections.Generic;

namespace AlleyCatBarbers.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string spreadsheetPath)
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

            if (!userManager.Users.Any())
            { 
                //Create Admin/Staff
                await CreateUser(userManager, "admin@admin.com", "password", "Admin", "John", "Doe", "0400 000 000", new DateOnly(2000, 1, 1));
                await CreateUser(userManager, "staff@staff.com", "password", "Staff", "Jane", "Doe", "0400 000 000", new DateOnly(2000, 1, 1));

                //Create Customers
                var userRecords = SpreadsheetReader.ReadUserRecords(spreadsheetPath);

                foreach (var userRecord in userRecords)
                {
                    await CreateUser(userManager, userRecord.Email, userRecord.Password, userRecord.Role, userRecord.FirstName, userRecord.LastName, userRecord.PhoneNumber, userRecord.DateOfBirth);
                }
            }

            //Create reviews
            if (!context.Reviews.Any())
            {
                await CreateReviews(context, userManager);
            }

        }

        public static async Task CreateUser(UserManager<ApplicationUser> userManager, string email, 
            string password, string role, string firstName, string lastName, string phoneNumber, DateOnly dateOfBirth)
        {
            

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    DateOfBirth = dateOfBirth
                    
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


        public static async Task CreateReviews(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {

            var users = userManager.Users.ToList();

            if (!users.Any())
            {
                throw new Exception("No users to assign reviews");
            }

            var random = new Random();
            var reviews = new List<Review>
        {
            new Review { Comments = "Great service!", Rating = 5, DateCreated = DateTime.Now, UserId = users[random.Next(users.Count)].Id },
            new Review { Comments = "Good service.", Rating = 4, DateCreated = DateTime.Now, UserId = users[random.Next(users.Count)].Id },
            new Review { Comments = "Average experience.", Rating = 3, DateCreated = DateTime.Now, UserId = users[random.Next(users.Count)].Id }
        };

            context.Reviews.AddRange(reviews);
            await context.SaveChangesAsync();
        }


    }
}
