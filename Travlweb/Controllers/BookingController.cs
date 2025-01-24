using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Travlweb.Models;

namespace Travlweb.Controllers
{
    public class BookingController : Controller
    {
        private readonly HttpClient _httpClient;
        public BookingController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddTravelBooking()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTravelBooking(BookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7224/api/Booking/book", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Booking successful!";
                return RedirectToAction("BookingList", "Booking");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, error);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> BookingList()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7224/api/Booking/List");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var bookings = JsonSerializer.Deserialize<List<BookingViewModel>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(bookings);
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to fetch bookings";
                    return View(new List<BookingViewModel>());
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(new List<BookingViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> BookingDetails(int id)
        {
            try
            {
                // Use the full API URL directly
                //string apiUrl = $"https://localhost:7224/api/Booking/{id}";
                //HttpClient client = new HttpClient();

                //var response = await client.GetAsync(apiUrl);
                var response = await _httpClient.GetAsync($"https://localhost:7224/api/Booking/List/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var booking = await response.Content.ReadFromJsonAsync<BookingViewModel>();
                    return View(booking);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ViewBag.ErrorMessage = "Booking not found.";
                    return View("Error");
                }

                ViewBag.ErrorMessage = "Unable to fetch booking details.";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            try
            {
                // Send the DELETE request to the API
                var response = await _httpClient.DeleteAsync($"https://localhost:7224/api/Booking/{id}");

                if (response.IsSuccessStatusCode)
                {
                    // Redirect to the list of bookings after successful deletion
                    TempData["SuccessMessage"] = "Booking deleted successfully.";

                    // Redirect to the BookingDetails page or a list view
                    return RedirectToAction("BookingList", "Booking");  // Or wherever you want to redirect after successful deletion
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ViewBag.ErrorMessage = "Booking not found.";
                    return View("Error");
                }

                ViewBag.ErrorMessage = "Unable to delete the booking.";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View("Error");
            }
        }


        public async Task<IActionResult>UpdateBooking(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7224/api/Booking/{id}");

            if (response.IsSuccessStatusCode)
            {
                var bookingData = await response.Content.ReadFromJsonAsync<BookingViewModel>();
                return View(bookingData);
            }

            return NotFound();
        }
        // POST: Update Booking
        [HttpPost]
        public async Task<IActionResult> UpdateBooking(BookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            // Make PUT request to the API to update booking
            var response = await _httpClient.PutAsync($"https://localhost:7224/api/Booking/{model.BookingId}", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Booking updated successfully!";
                return RedirectToAction("BookingList");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, error);

            return View(model);
        }

    }
}
