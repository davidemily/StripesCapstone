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
        public async Task<List<string>> Get()
        {
            // ex for waitTime: 105 = 1 hour 45 min
            // need to calculate 
            WaitTime result = new WaitTime
            {
                waitTime = 105
            };
            List<string> queryResult = new List<string>();
            DBConnector dbConnection = new DBConnector();
            queryResult = dbConnection.getNightStatus();
            console.WriteLine(queryResult[1]);
            if (dbConnection.OpenConnection())
            {
                try 
                {
                    
                    // Console.WriteLine(queryResult);
                    // if(queryResult[2].Equals(1))
                    // {
                        result.status = queryResult[0].ToString();
                    // }
                }
                catch (Exception ex)
                {
                    result.status = ex.ToString();
                    // return StatusCode(500);
                }
                dbConnection.CloseConnection();
            }

            return queryResult;
        }
    }
}
