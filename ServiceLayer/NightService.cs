using System;
using API.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace API.ServiceLayer
{
    public class NightService : ControllerBase
    {
        [HttpPost]
        public ActionResult Post()
        {
            var dbConnector = new DBConnector();
            //get hours 9 hours from utc or 3 hours before central time to cover open till 3
            var todaysDate = DateTime.Today.ToString("yyyy-MM-dd");

            try
            {
                if (dbConnector.IsNightActive(todaysDate) == false)
                {
                    Console.WriteLine("night does not exist");
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