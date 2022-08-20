using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using BeastLearn.Application.Interfaces;


namespace BeastLearn.Application.Services
{
    public class SendEmail:IMailSender
    {
        public void Send(string to, string subject, string body)
        {
            var defaultEmail = "akoo.kurdnejad@gmail.com";

            var mail = new MailMessage();

            var SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress(defaultEmail, "خودآموز بست لرن");

            mail.To.Add(to);

            mail.Subject = subject;

            mail.Body = body;

            mail.IsBodyHtml = true;

            // System.Net.Mail.Attachment attachment;
            // attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
            // mail.Attachments.Add(attachment);

            SmtpServer.Port = 587;

            SmtpServer.Credentials = new System.Net.NetworkCredential(defaultEmail, "akoo3062331");

            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }
    }
}
