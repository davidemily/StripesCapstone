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
    public class WaitTimeController : ControllerBase
    {
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
            DBConnector dbConnection = new DBConnector();
            var todaysDate = DateTime.Today.ToString("yyyy-MM-dd");
            try
            {
                if(dbConnection.IsNightActive(todaysDate))
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
  
        private int getWaitTime()
        {
            return 105;
        }
    }
}
