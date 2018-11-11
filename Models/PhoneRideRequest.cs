namespace API.Models
{
    public class PhoneRideRequest
    {
        public string firstName { get; set; }
        public string homeAddress { get; set; }
        public string cellPhoneNumber { get; set; }
        public string passengers { get; set; }
        public string dropoffs { get; set; }
        public string pickupLocation { get; set; }
    }
}