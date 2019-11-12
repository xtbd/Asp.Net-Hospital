using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital.Controllers
    //this class is mainly referenced from FIT5032 week8 lab and I made slightly change to meet the requriment
{
    public class EmailSender
    {
        private const String API_KEY = "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";

        //this metho is used to send email when the booking is confirmed
        public void Send(String toEmail)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("noreply@localhost.com", "Booking Manager"); //the send email will be noreply@localhost.com and sender will be Booking Manager
            var to = new EmailAddress(toEmail, "");
            var subject = "Reservation confirmation"; // the subject is reservation confirmation
            var plainTextContent = "Your reservation has been confirmed!"; // the content is your reservation has been confirmed
            var htmlContent = "<p>" + plainTextContent + "</p >";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        

        }
    }
}
