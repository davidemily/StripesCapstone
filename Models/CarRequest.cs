using System;

namespace API.Models
{
    public class CarRequest
    {
        public int CarId { get; set; }
        public int CarNumber { get; set; }
        public string PhoneNumber { get; set; }
        public byte IsActive { get; set; }
    }
}