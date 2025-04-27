using System.Runtime.CompilerServices;
using Camp_Rating_System.Data;
using Camp_Rating_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Camp_Rating_System.Controllers
{
    [Authorize]
    public class CampController : Controller
    {
        private readonly ProjectDbContext _context;

        public CampController(ProjectDbContext context)
        {
            _context = context;
        }

        // GET: /Camps
        [AllowAnonymous]
        public async Task<IActionResult> Index(string search)
        {
            var campSites = _context.Camps.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                campSites = campSites.Where(c => c.Name.Contains(search));
            }

            return View(await campSites.ToListAsync());
        }

        // GET: /Camps/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var camp = await _context.Camps
                .Include(c => c.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (camp == null) return NotFound();

            return View(camp);
        }

        // GET: /Camps/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Camps/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CampViewModel camp, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length <= 2 * 1024 * 1024)
                {
                    using var ms = new MemoryStream();
                    await image.CopyToAsync(ms);
                    camp.Photo = ms.ToArray();
                }

                _context.Add(camp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(camp);
        }

        // GET: /Camps/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var campSite = await _context.Camps.FindAsync(id);
            if (campSite == null) return NotFound();

            return View(campSite);
        }

        // POST: /Camps/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, CampViewModel campModel, IFormFile image)
        {
            if (id != campModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (image != null && image.Length <= 2 * 1024 * 1024)
                    {
                        using var ms = new MemoryStream();
                        await image.CopyToAsync(ms);
                        campModel.Photo = ms.ToArray();
                    }
                    _context.Update(campModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CampExists(campModel.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(campModel);
        }

        // GET: /CampSites/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var campSite = await _context.Camps.FirstOrDefaultAsync(m => m.Id == id);
            if (campSite == null) return NotFound();

            return View(campSite);
        }

        // POST: /Camps/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var camp = await _context.Camps.FindAsync(id);
            _context.Camps.Remove(camp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CampExists(int id)
        {
            return _context.Camps.Any(e => e.Id == id);
        }

    }
}
