using Microsoft.AspNetCore.Mvc;
using Handlingform.Models; // Ensure to import your models
using Handlingform.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

public class UserController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context) // Inject ApplicationDbContext into the controller
    {
        _context = context;
    }

    // GET: /User/Register
    public IActionResult Register()
    {
        return View(); // Return the registration view
    }

    // POST: /User/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(User user)
    {
        if (ModelState.IsValid) // Check if model state is valid
        {
            // Check if the email already exists in the database
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "This email is already taken.");
                return View(user); // Return the form with an error message
            }

            // Hash the password before saving (for security reasons)
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); // Assuming you use BCrypt for hashing

            // Save the user to the database
            _context.Add(user); // Add the user to the context
            await _context.SaveChangesAsync(); // Save changes to the database

            return RedirectToAction("Index", "Home"); // Redirect to Home or Login page after successful registration
        }

        return View(user); // If model state is invalid, return the view with validation errors
    }

    // GET: /User/Login
    public IActionResult Login()
    {
        return View(); // Return the login view
    }

    // POST: /User/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(Login login)
    {
        if (ModelState.IsValid)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);

            if (user != null && BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {
                // Create claims for the user
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Set the authentication cookie
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                // Redirect to Home page
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
        }

        return View(login);
    }

    // Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clear the session
        return RedirectToAction("Login"); // Redirect to the Login page
    }
}
