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
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public async Task<SystemStatus> Get()
        {
            SystemStatus result = new SystemStatus
            {
                SystemIsUp = true,
                DbIsUp = false
            };
            
            DBConnector test = new DBConnector();
            if (test.OpenConnection())
            {
                result.DbIsUp = true;
                test.CloseConnection();
            }

            return result;
        }
    }
}
