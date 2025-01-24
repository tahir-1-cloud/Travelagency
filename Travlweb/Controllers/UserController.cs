using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Text.Json;
using Travlweb.Models;

namespace Travlweb.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        public UserController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }

      
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7224/api/Security/login", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                ViewBag.Message = "Login successful!";
                return RedirectToAction("admindetail", "Admin");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, error);

            return View(model);
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7224/api/Security/register", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Registration successful!";
                return RedirectToAction("Login");
            }

            // If the response contains an error message, read it and pass it to the View
            var error = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.BadRequest)  // Adjust this check based on the API's error response code
            {
                ModelState.AddModelError(string.Empty, "Email already exists or other error: " + error);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + error);
            }

            return View(model);
        }



        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Clear session data if needed
            HttpContext.Session.Clear();

            // Redirect to the home page or another page after logging out
            return RedirectToAction("Index", "Home");
        }
    }
}
