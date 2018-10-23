using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DataAccess;
using System.Linq;

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
            DBConnector dbConnection = new DBConnector();

            WaitTime result = new WaitTime
            {
                status = getStatus(dbConnection),
                waitTime = getTime()
            };

            dbConnection.CloseConnection();
            return result;
        }
    
        public string getStatus(DBConnector dbConnection)
        {
            string temp = "";
            if (dbConnection.OpenConnection())
            {
                try 
                {
                    string[] queryResult = dbConnection.getNightStatus();
                    // List<string> queryResult = dbConnection.getNightStatus();
                    // this line above will return ["1", "10/21/2018 12:00:00 AM", "1"]
                    // for some reason after I get the list from the database I am getting a: System.Collections.Generic.List`1[System.String]
                    // need to figure out how to get this into a list so I can do something like: queryResult[2]
                    // but for some reason this is giving me an out of bounds...
                    Console.WriteLine(queryResult[0]);
                    // if(queryResult[2].Equals("1"))
                    // {
                        return temp;
                    // }
                    // else 
                    // {
                    //     return "notRunning";
                    // }
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                    // return StatusCode(500);
                }
            }
            else 
            {
                return "";
            }
        }

        public int getTime()
        {
            return 105;
        }
    }
}
