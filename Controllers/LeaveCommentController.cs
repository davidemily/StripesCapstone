using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DataAccess;

using System.Collections.Generic;
using System;

using System.Text;
using System.Net.Mail;
using System.Net.Mime;

namespace API.Controllers
{
    [Route("api/[controller]")] 
    [ApiController] 
    public class LeaveCommentController : ControllerBase
    {
        /* HttpPost]
        public ActionResult Post([FromBody] RideRequest request)
        {
            if (request.PatronName == null || request.PhoneNumber == null || request.Pickup == null || request.Destination == null || request.NumberOfPeople == null)
            {
                return BadRequest();
            }

            try
            {
                DBConnector db = new DBConnector();
                db.LeaveComment(request);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
            return Ok();

        }*/

        public void sendEmail() {
            Console.WriteLine("test");
            
            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress("STRIPESComments@gmail.com", "To STRIPES")); //testing for now

                // From
                mailMsg.From = new MailAddress("STRIPESComments@gmail.com", "Patron"); //this will have to change

                // Subject and multipart/alternative Body
                mailMsg.Subject = "Patron Comment";
                string text = "text body test";
                string html = @"<p>html body</p>";
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("STRIPESComments@gmail.com", "1qaz@wsx#edc4");
                smtpClient.Credentials = credentials;

                smtpClient.Send(mailMsg);
            }
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
                }
    }
}
