using System.Security.Claims;
using Business.Models;
using Business.Results.Bases;
using Business.Services;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly Db _context;
        private readonly IUserService _userService;

        public UsersController(Db context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            List<UserModel> userList = _userService.Query().ToList();
            return View(userList);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            UserModel user = _userService.Query().SingleOrDefault(u => u.Id == id);

            if (user == null)
               return NotFound();
            
            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,Password,IsAdmin,Id")] UserModel user)
        {
            if (ModelState.IsValid)
            {
                Result result = _userService.Add(user);
                if (result.IsSuccessful)
                {
                    //if (!User.Identity.IsAuthenticated)
                    //    return Redirect("Account/Login");

                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "User could not be added!");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            UserModel user = _userService.Query().SingleOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserName,Password,IsAdmin,Id")] UserModel user)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.Update(user);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }

            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            UserModel user = _userService.Query().SingleOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = _userService.Delete(id);
            TempData["Message"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        #region Authentication
        [HttpGet("Account/{action}")]
        public IActionResult Login()
        {
            return View(); 
        }

        [HttpPost("Account/{action}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserModel user)
        {
            var existingUser = _userService.Query().SingleOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);
            if (existingUser is null)
            {
                ModelState.AddModelError("", "Invalid user name and password!");
                return View();
            }

            List<Claim> userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, existingUser.UserName),
                new Claim(ClaimTypes.Role, existingUser.IsAdmin ? "admin" : "user")
            };

            var userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(userPrincipal);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Account/{action}")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // ~/Account/AccessDenied
        [HttpGet("Account/{action}")]
        public IActionResult AccessDenied()
        {
            return View("_Error", "You don't have access to this operation!");
        }
        #endregion
    }
}
