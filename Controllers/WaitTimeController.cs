using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DataAccess;

using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Razor.Language.Extensions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaitTimeController : ControllerBase
    {
        private int _estWaitTimeOfUnassigned = 20;
        private int _estWaitTimeOfAssigned = 10;
        private int __estWaitTimeOfRiding = 5;
        private readonly DBConnector _dbConnector = new DBConnector();

        [HttpGet]
        public async Task<WaitTime> Get()
        {
            // ex for waitTime: 105 = 1 hour 45 min
            // need to calculate 
            WaitTime result = new WaitTime
            {
                status = getStatus(),
                waitTime = getWaitTime()
            };

            return result;
        }

        private string getStatus()
        {
            bool running = false;
            
            var todaysDate = DateTime.Today.ToString("yyyy-MM-dd");
            try
            {
                if(_dbConnector.IsNightActive(todaysDate))
                {
                    return "running";
                }
            }
            catch (Exception ex)
            {
                return "error";
            }
            return "notRunning";
            
        }
  //Approx. wait time =
  //[(Number of unassigned requested rides * Estimated wait time for unassigned ride in minutes)
  //+
  //(Number assigned rides * estimated wait time of assigned ride in minutes * amount of patrons dropped off per location)] / Number of cars
        private int getWaitTime()
        {
            
//            int numOfAssignedRides = 0;
//
//            int numOfRidingRides = 0;
//
//            float numOfStopsPerRide = 0;
//            
//            int numOfCarsRunning = 0;

            int numOfUnassignedRides = _dbConnector.GetUnassignedRides();

//            numOfAssignedRides = _dbConnector.GetAssignedRides();
//
//            numOfRidingRides = _dbConnector.GetRidingRides();
//
//            numOfStopsPerRide = _dbConnector.GetStopsPerRide();
//
//            numOfCarsRunning = _dbConnector.GetNumberCarsRunning();
            return numOfUnassignedRides;
        }
    }
}
