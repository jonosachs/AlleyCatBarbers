using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlleyCatBarbers.Data;
using AlleyCatBarbers.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using AlleyCatBarbers.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace AlleyCatBarbers.Controllers
{
    
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reviews
        public async Task<IActionResult> List()
        {
            var reviews = await _context.Reviews
                .Include(r => r.User)
                .Select(r => new ReviewViewModel
                {
                    Rating = r.Rating,
                    Comments = r.Comments,
                    UserName = r.User.UserName,
                    DateCreated = r.DateCreated

                })
                .ToListAsync();

            var viewModel = new ReviewListViewModel
            {
                Reviews = reviews,
                AverageRating = reviews.Any() ? ((int)reviews.Average(r => r.Rating)) : 0
            };

            return View(viewModel);
        }

        // GET: Reviews/Create
        [Authorize(Roles = "Customer")]
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create(ReviewViewModel reviewViewModel)
        {
            if (ModelState.IsValid)
            {
                
                var user = await _userManager.GetUserAsync(User);
                var review = new Review
                {
                    Rating = reviewViewModel.Rating,
                    Comments = reviewViewModel.Comments,
                    UserId = user.Id,
                    DateCreated = DateTime.Now
                };

                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }

            return View(reviewViewModel);
        }


        //private bool ReviewExists(int id)
        //{
        //    return _context.Reviews.Any(e => e.Id == id);
        //}
    }
}
