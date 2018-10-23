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
                    List<string> queryResult = dbConnection.getNightStatus();
                    foreach(var i in queryResult)
                    {
                        temp += i + " MEMES";
                        Console.WriteLine(i);
                    }
                    // Console.WriteLine(queryResult);
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
