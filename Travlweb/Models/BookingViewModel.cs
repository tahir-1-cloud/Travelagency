namespace Travlweb.Models
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string LeavingFrom { get; set; } = null!;
        public string TravelingTo { get; set; } = null!;
        public int NumberOfPersons { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
