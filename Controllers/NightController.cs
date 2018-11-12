using System;
using API.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")] 
    [ApiController] 
    public class NightController : ControllerBase
    {
        [HttpPost]
        public ActionResult Post()
        {
            var dbConnector = new DBConnector();
            //get hours 9 hours from utc or 3 hours before central time to cover open till 3
            var todaysDate = DateTime.Today.AddHours(-3);
            try
            {
                Console.WriteLine(todaysDate);
                Console.WriteLine(DateTime.Today);
                Console.WriteLine(DateTime.Now);
                if (!dbConnector.IsNightActive(todaysDate))
                {
                    dbConnector.CreateNewNight(todaysDate);
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