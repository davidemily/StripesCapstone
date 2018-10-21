using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DataAccess;

using System.Collections.Generic;
using System;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RidesController : ControllerBase
    {
        [HttpPost]
        public ActionResult Post([FromBody] RideRequest request)
        {
            if (request.PatronName == null || request.PhoneNumber == null || request.Pickup == null || request.Destination == null)
            {
                return BadRequest();
            }

            try
            {
                DBConnector db = new DBConnector();
                db.InsertRide(request);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
            return Ok();

        }
    }
}
