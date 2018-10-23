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

        public string getStatus()
        {
            List<string> queryResult = new List<string>();
            DBConnector dbConnection = new DBConnector();
            queryResult = dbConnection.getNightStatus();
            if(queryResult[2] != "True")
            {
                return "running";
            } else 
            {
                return "notRunning";
            }
        }

        public int getWaitTime()
        {
            return 105;
        }
    }
}
