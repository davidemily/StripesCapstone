using System;

namespace API.Models
{
    public class RideRequest
    {
        public int RideId { get; set; }
        public string PatronName { get; set; }
        public string PhoneNumber { get; set; }
        public string NumberOfPeople { get; set; }
        public string Pickup { get; set; }
        public string PickupLat { get; set; }
        public string PickupLong { get; set; }
        public string Destination { get; set; }
        public string DestinationLat { get; set; }
        public string DestinationLong { get; set; }
        public string Description { get; set; }

        public enum Status
        {
            waiting,
            assigned,
            canceled,
            completed,
            riding
        }

        public DateTime TimeRequested { get; set; }
        public DateTime TimeDispatched { get; set; }
        public DateTime TimeFinished { get; set; }
        public DateTime TimePickedUp { get; set; }

        public enum RequestSource
        {
            iphone,
            android,
            caller
        }

        public int CARS_CarId { get; set; }
    }
}