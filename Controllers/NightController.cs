using System;
using API.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")] 
    [ApiController] 
    public class NightController : ControllerBase
    {
        private DBConnector _dbConnector;
        [HttpPost]
        public ActionResult Post()
        {
            //get hours 9 hours from utc or 3 hours before central time to cover open till 3
            var todaysDate = DateTime.UtcNow.Date.AddHours(-9);
            try
            {
                if (!_dbConnector.IsNightActive(todaysDate))
                {
                    _dbConnector.CreateNewNight(todaysDate);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.InnerException);
                return BadRequest();
            }

            return Ok();
        }
    }
}