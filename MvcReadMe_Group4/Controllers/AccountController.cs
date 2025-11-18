using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MvcReadMe_Group4.Data;
using MvcReadMe_Group4.Models;
using MvcReadMe_Group4.ViewModels;
using MvcReadMe_Group4.Hubs;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MvcReadMe_Group4.Controllers
{
    public class AccountController : Controller
    {
        private readonly MvcReadMe_Group4Context _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public AccountController(MvcReadMe_Group4Context context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
            }

            var user = _context.Users
                .FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.Password);

            if (user == null)
            {
                return Json(new { success = false });
            }

            // Store user info in session
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            // also set integer value for helpers that expect GetInt32
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetString("UserFullName", $"{user.FirstName} {user.LastName}");

            // Store success message in TempData
            TempData["LoginSuccess"] = true;

            // Send notification about login
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", 
                "User Activity", 
                $"Welcome back, {user.FirstName}! 📚");

            var redirectUrl = user.Role == "Admin" ? 
                Url.Action("Dashboard", "Admin") : 
                Url.Action("Home", "User");

            // Return JSON response for Ajax login
            return Json(new { 
                success = true, 
                redirectTo = redirectUrl
            });

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Validate if username and password is valid and does not yet exist
            if (!ModelState.IsValid)
                return View(model);

            if (model.Password.Length < 6)
            {
                ModelState.AddModelError(nameof(model.Password), "Password must be at least 6 characters long");
                return View(model);
            }

            if (_context.Users.Any(u => u.UserName == model.UserName))
            {
                ModelState.AddModelError(nameof(model.UserName), "Username is already taken");
                return View(model);
            }

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError(nameof(model.Email), "Email is already registered");
                return View(model);
            }

            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                Role = "User" // default role
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            
            TempData["RegistrationSuccess"] = true;

            TempData["RegisterSuccess"] = model.UserName; // store username for Swal
            return RedirectToAction("Login"); // redirect to login page with alert

        }

        [HttpGet]
        public IActionResult LoginLoading()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }
    }
}