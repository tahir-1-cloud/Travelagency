using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travelagncyapi.Models;

namespace Travelagncyapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly travelContext _dbcontext;
        public BookingController( travelContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpPost("book")]
        public async Task<IActionResult> PostBooking([FromBody] Booking booking)
        {
            if (booking == null)
            {
                return BadRequest(new { Message = "Booking data is required" });
            }

            try
            {
                // Set the booking date (you can adjust this to match your business logic)
                booking.BookingDate = DateTime.UtcNow;

                // Save to the database
                await _dbcontext.Bookings.AddAsync(booking);
                await _dbcontext.SaveChangesAsync();

                return Ok(new { Message = "Booking created successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { Message = "Internal Server Error" });
            }
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetBookings()
        {
            try
            {
                var bookings = await _dbcontext.Bookings.ToListAsync();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { Message = "Internal Server Error" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            try
            {
                var booking = await _dbcontext.Bookings.FindAsync(id);

                if (booking == null)
                {
                    return NotFound(new { Message = "Booking not found" });
                }

                return Ok(booking);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { Message = "Internal Server Error" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] Booking updatedBooking)
        {
            if (updatedBooking == null)
            {
                return BadRequest(new { Message = "Updated booking data is required" });
            }

            try
            {
                var booking = await _dbcontext.Bookings.FindAsync(id);

                if (booking == null)
                {
                    return NotFound(new { Message = "Booking not found" });
                }

                // Update the booking data
                booking.FirstName = updatedBooking.FirstName;
                booking.LastName = updatedBooking.LastName;
                booking.Email = updatedBooking.Email;
                booking.PhoneNumber = updatedBooking.PhoneNumber;
                booking.Gender = updatedBooking.Gender;
                booking.LeavingFrom = updatedBooking.LeavingFrom;
                booking.TravelingTo = updatedBooking.TravelingTo;
                booking.NumberOfPersons = updatedBooking.NumberOfPersons;
                booking.BookingDate = DateTime.UtcNow;

                // Save the updated booking to the database
                _dbcontext.Bookings.Update(booking);
                await _dbcontext.SaveChangesAsync();

                return Ok(new { Message = "Booking updated successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { Message = "Internal Server Error" });
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            try
            {
                var booking = await _dbcontext.Bookings.FindAsync(id);

                if (booking == null)
                {
                    return NotFound(new { Message = "Booking not found" });
                }

                // Remove the booking from the database
                _dbcontext.Bookings.Remove(booking);
                await _dbcontext.SaveChangesAsync();

                return Ok(new { Message = "Booking deleted successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { Message = "Internal Server Error" });
            }
        }



    }
}
