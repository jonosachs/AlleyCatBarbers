﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlleyCatBarbers.Data;
using AlleyCatBarbers.ViewModels;
using Microsoft.AspNetCore.Identity;
using AlleyCatBarbers.Models;
using SendGrid.Helpers.Mail;
using System.Security.Claims;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace AlleyCatBarbers.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<BookingsController> _logger;

        public UsersController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, ILogger<BookingsController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();
            

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userViewModel = new UserViewModel
                {

                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    Roles = string.Join(", ", roles)
                };
                userViewModels.Add(userViewModel);
            }

            return View(userViewModels);
        }



        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var model = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Roles = string.Join(", ", roles)
            };

            return View(model);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            _logger.LogInformation("CREATE METHOD TRIGGERED");
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {

            ModelState.Remove(nameof(model.Id));

            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        DateOfBirth = model.DateOfBirth
                    };

                    // Default password
                    var result = await _userManager.CreateAsync(user, "password");

                    if (result.Succeeded)
                    {

                        model.Id = user.Id;

                        var roles = model.Roles.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        // Assign roles
                        if (roles != null && roles.Length > 0)
                        {
                            await _userManager.AddToRolesAsync(user, roles);
                        }

                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while creating the user. Please try again.");

            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid. Errors: {Errors}",
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            }

            _logger.LogInformation("ERROR");

            return View(model);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = new List<string> { string.Join(", ", userRoles) };

            _logger.LogInformation("FORMATTED ROLES {roles}", roles);

            var model = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Roles = string.Join(", ", userRoles)
            };

            return View(model);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.DateOfBirth = model.DateOfBirth;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {

                    var roles = model.Roles.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    foreach (var role in roles)
                    {
                        if (!await _roleManager.RoleExistsAsync(role))
                        {
                            ModelState.AddModelError(string.Empty, $"The role '{role}' does not exist.");
                            return View(model); // Return the view with the model errors
                        }
                    }

                    // Update roles
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var rolesToRemove = userRoles.Except(roles).ToList();
                    var rolesToAdd = roles.Except(userRoles).ToList();


                    if (rolesToRemove.Any())
                    {
                        await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    }

                    if (rolesToAdd.Any())
                    {
                        await _userManager.AddToRolesAsync(user, rolesToAdd);
                    }

                    await _signInManager.RefreshSignInAsync(user);

                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var model = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Roles = string.Join(", ", roles)
            };

            return View(model);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            var roles = await _userManager.GetRolesAsync(user);

            return View("Delete", new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Roles = string.Join(", ", roles)
            });
        }
    }


}

