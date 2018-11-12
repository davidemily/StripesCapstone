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
            var todaysDate = DateTime.Today.ToString("MM/dd/yyyy");
            var convertedTodaysDate = Convert.ToDate(todaysDate);
            try
            {
                Console.WriteLine(todaysDate);
                Console.WriteLine(convertedTodaysDate);
                if (!dbConnector.IsNightActive(convertedTodaysDate))
                {
                    dbConnector.CreateNewNight(convertedTodaysDate);
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