using Business.Services;
using System.ComponentModel.Design;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.Models;

namespace MVC.Controllers
{
    public class DevelopersController : Controller
    {
        private readonly Db _context;

        private readonly IDeveloperService _developerService;

        public DevelopersController(Db context, IDeveloperService developerService, IGameService gameService)
        {
            _context = context;
            _developerService = developerService;
        }

        // GET: Developers
        public async Task<IActionResult> Index()
        {
            List<DeveloperModel> developerList = _developerService.Query().ToList();
            return View(developerList);
        }

        // GET: Developers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            DeveloperModel developer = _developerService.Query().SingleOrDefault(d => d.Id == id);
            if (developer == null)
                return View("_Error", "Developer not found!");

            return View(developer);
        }

        // GET: Developers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Developers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,FoundingDate,Id")] DeveloperModel developer)
        {
            if (ModelState.IsValid)
            {
                var result = _developerService.Add(developer);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }

            return View(developer);
        }

        // GET: Developers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            DeveloperModel developer = _developerService.Query().SingleOrDefault(d => d.Id == id);
            if (developer == null)
                return View("_Error", "Developer not found!");

            return View(developer);
        }

        // POST: Developers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,FoundingDate,Id")] DeveloperModel developer)
        {
            if (ModelState.IsValid)
            {
                var result = _developerService.Update(developer);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;

                    return RedirectToAction(nameof(Details), new { id = developer.Id });
                }
                ModelState.AddModelError("", result.Message);
            }

            return View(developer);
        }

        // GET: Developers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var result = _developerService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        //// POST: Developers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Developers == null)
        //    {
        //        return Problem("Entity set 'Db.Developers'  is null.");
        //    }
        //    var developer = await _context.Developers.FindAsync(id);
        //    if (developer != null)
        //    {
        //        _context.Developers.Remove(developer);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
