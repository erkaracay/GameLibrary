using Business.Models;
using Business.Results.Bases;
using Business.Services;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Controllers
{
    public class GamesController : Controller
    {
        private readonly Db _context;
        private readonly IGameService _gameService;
        private readonly IDeveloperService _developerService;
        private readonly ICategoryService _categoryService;

        public GamesController(Db context, IDeveloperService developerService, IGameService gameService, ICategoryService categoryService)
        {
            _context = context;
            _gameService = gameService;
            _developerService = developerService;
            _categoryService = categoryService;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            List<GameModel> gameList = _gameService.Query().ToList();

            return View(gameList);
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            GameModel game = _gameService.Query().SingleOrDefault(g => g.Id == id);

            if (game == null)
                return NotFound();

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            ViewBag.DeveloperId = new SelectList(_developerService.Query().ToList(), "Id", "Name");
            ViewBag.CategoryId = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,Revenue,CategoryId,DeveloperId,ReleaseDate,Id")] GameModel game)
        {
            if (ModelState.IsValid)
            {
                Result result = _gameService.Add(game);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }

            ViewBag.CategoryId = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            ViewBag.DeveloperId = new SelectList(_developerService.Query().ToList(), "Id", "Name");
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            GameModel game = _gameService.Query().SingleOrDefault(g => g.Id == id);
            if (game == null)
                return NotFound();

            ViewBag.CategoryId = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            ViewBag.DeveloperId = new SelectList(_developerService.Query().ToList(), "Id", "Name");
            return View(game);

        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Price,Revenue,CategoryId,DeveloperId,ReleaseDate,Id")] GameModel game)
        {
            if (ModelState.IsValid)
            {
                var result = _gameService.Update(game);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }

            ViewBag.CategoryId = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            ViewBag.DeveloperId = new SelectList(_developerService.Query().ToList(), "Id", "Name");
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            GameModel game = _gameService.Query().SingleOrDefault(g => g.Id == id);
            if (game == null)
                return NotFound();

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = _gameService.Delete(id);
            TempData["Message"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}
