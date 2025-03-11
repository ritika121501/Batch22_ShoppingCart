using System;
using System.Net;
using System.Net.Mail;

namespace ShoppingCart.Utility
{
    public class EmailSender : IEmailSender
    {
        public void SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("ritika120115@gmail.com"); // Update with your email
                    mailMessage.To.Add("ritika1215@gmail.com");
                    mailMessage.Subject = subject;
                    mailMessage.Body = htmlMessage;
                    mailMessage.IsBodyHtml = true;

                    using (SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587)) // Valid SMTP Server
                    {
                        smtpClient.UseDefaultCredentials = true;
                        //smtpClient.Credentials = new NetworkCredential("ritika", "your-password");
                        smtpClient.EnableSsl = true;

                        // Ensure TLS 1.2 is enforced
                        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        smtpClient.Send(mailMessage);
                        Console.WriteLine("Email sent successfully.");
                    }
                }
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }
        }
    }
}
