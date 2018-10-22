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
                waitTime = 105
            };
            
            DBConnector dbConnection = new DBConnector();
            if (dbConnection.OpenConnection())
            {
                try 
                {
                    List<string> queryResult = dbConnection.getNightStatus();
                    // if(queryResult[2].Equals("1"))
                    // {
                        result.status = queryResult[2];
                    // }
                }
                catch (Exception ex)
                {
                    // return StatusCode(500);
                }
                dbConnection.CloseConnection();
            }

            return result;
        }
    }
}