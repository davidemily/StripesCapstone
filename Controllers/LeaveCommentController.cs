using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DataAccess;

using System.Collections.Generic;
using System;
using API.ServiceLayer;

using System.Text;
using System.Net.Mail;
using System.Net.Mime;

namespace API.Controllers
{
    [Route("api/[controller]")] 
    [ApiController] 
    public class LeaveCommentController : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody] CommentRequest commentToEmail)
        {
            LeaveCommentService leaveCommentService = new LeaveCommentService();
            leaveCommentService.Post(commentToEmail);
        //     try
        //     {
        //         MailMessage mailMsg = new MailMessage();

        //         // To
        //         mailMsg.To.Add(new MailAddress("STRIPESComments@gmail.com"));

        //         // From
        //         mailMsg.From = new MailAddress("STRIPESComments@gmail.com");

        //         // Subject and multipart/alternative Body
        //         mailMsg.Subject = "Patron Comment";
        //         mailMsg.Body = commentToEmail.comment;

        //         // Init SmtpClient and send
        //         SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587))
        //         {
        //             Port = 587, EnableSsl = true
        //         };
        //         var credentials = new System.Net.NetworkCredential("STRIPESComments@gmail.com", "1qaz@wsx#edc4");
        //         smtpClient.Credentials = credentials;
        //         smtpClient.Send(mailMsg);
        //     }
            
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine(ex.Message + " " + ex.InnerException);
        //     }
        }
    }
}
