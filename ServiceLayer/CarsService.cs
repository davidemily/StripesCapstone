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
    public class CarsService : ControllerBase
    {
        public ActionResult Post([FromBody] CarRequest request)
        {
            if (request.CarId == null || request.CarNumber == null || request.PhoneNumber == null ||
                request.IsActive == null)
            {
                return BadRequest();
            }

            var convertedCarRequest = new CarRequest()
            {
                CarId = request.CarId,
                CarNumber = request.CarNumber,
                PhoneNumber = request.PhoneNumber,
                IsActive = request.IsActive
            };
            
            try
            {
                DBConnector db = new DBConnector();
                db.InsertNewCar(convertedCarRequest);
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
