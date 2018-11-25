using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DataAccess;

using System.Collections.Generic;
using System;

namespace API.ServiceLayer
{
    public class RidesService : ControllerBase
    {
        [HttpPost]
        public ActionResult Post([FromBody] PhoneRideRequest request)
        {
            if (request.dropoffs == null || request.passengers == null || request.firstName == null ||
                request.homeAddress == null || request.pickupLocation == null || request.cellPhoneNumber == null)
            {
                return BadRequest();
            }

            var convertedRideRequest = new RideRequest()
            {
                Pickup = request.pickupLocation,
                PhoneNumber = request.cellPhoneNumber,
                NumberOfPeople = request.passengers,
                Destination = request.homeAddress,
                PatronName = request.firstName
            };   
            
            try
            {
                DBConnector db = new DBConnector();
                db.InsertRide(convertedRideRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
            return Ok();

        }
    }
}
