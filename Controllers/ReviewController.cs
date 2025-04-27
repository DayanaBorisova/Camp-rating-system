using System.Runtime.CompilerServices;
using Camp_Rating_System.Data;
using Camp_Rating_System.Data.Entities;
using Camp_Rating_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Camp_Rating_System.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly ProjectDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewController(ProjectDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Review/MyReviews
        public async Task<IActionResult> MyReviews()
        {
            var userId = _userManager.GetUserId(User);
            var reviews = await _context.Reviews
                .Include(r => r.Camp)
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return View(reviews);
        }

        // GET: Review/Index
        public async Task<IActionResult> Index(int? campId)
        {
            // Fetch all reviews for a specific camp or all reviews if no campId is provided
            IQueryable<Review> reviewsQuery = _context.Reviews.Include(r => r.Camp).Include(r => r.User);

            if (campId.HasValue)
            {
                reviewsQuery = reviewsQuery.Where(r => r.CampId == campId.Value);
            }

            var reviews = await reviewsQuery
                .Select(r => new ReviewViewModel
                {
                    Id = r.Id,
                    Content = r.Content,
                    Rating = r.Rating,
                    CreatedDate = r.CreatedOn,
                    UserId = r.UserId,
                    UserName = r.User.UserName,
                    CampId = r.CampId,
                    CampName = r.Camp.Name
                })
                .ToListAsync();

            return View(reviews);
        }

        public IActionResult Create(int campId)
        {
            var model = new ReviewViewModel
            {
                CampId = campId
            };

            return View(model);
        }

        // POST: /Review/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Content))
            {
                ModelState.AddModelError("", "Съдържанието не може да бъде празно.");
                ViewBag.CampId = model.CampId;
                return View();
            }

            if (ModelState.IsValid)
            {
                // Get the current user
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var review = new Review
                    {
                        CampId = model.CampId,
                        UserId = user.Id,
                        Rating = model.Rating,
                        Content = model.Content,
                        CreatedOn = DateTime.Now
                    };

                    _context.Reviews.Add(review);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "Camps", new { id = model.CampId });
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (review.UserId != currentUser?.Id)
            {
                // Return forbidden if the user is not the owner of the review
                return Forbid();
            }

            var model = new ReviewViewModel
            {
                Id = review.Id,
                CampId = review.CampId,
                Content = review.Content,
                Rating = review.Rating,
                CreatedDate = review.CreatedOn,
                UserId = review.UserId
            };

            return View(model);
        }

        // POST: Review/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReviewViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            // Check if the current user is the owner of the review
            var currentUser = await _userManager.GetUserAsync(User);
            if (review.UserId != currentUser?.Id)
            {
                // Return forbidden if the user is not the owner of the review
                return Forbid(); 
            }

            if (ModelState.IsValid)
            {
                review.Rating = model.Rating;
                    review.Content = model.Content;

                    _context.Update(review);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "Camp", new { id = model.CampId });
            }

            return View(model);
        }

        // GET: /Review/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.Reviews.Include(r => r.Camp).FirstOrDefaultAsync(r => r.Id == id);
            if (review == null || review.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (review.UserId != currentUser?.Id)
            {
                // Return forbidden if the user is not the owner of the review
                return Forbid();
            }

            return View(review);
        }

        // POST: /Review/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null || review.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (review.UserId != currentUser?.Id)
            {
                // Return forbidden if the user is not the owner of the review
                return Forbid();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyReviews));
        }
    }
}
