using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace Dyno.Platform.ReferentialData.Business.Helper
{
    internal class EmailHelperSMTP
    {
        public bool SendEmail(string userEmail, string emailBody)
        {


            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("no.reply.dyno.app@gmail.com"));
            email.To.Add(MailboxAddress.Parse(userEmail));
            email.Subject = "Verifier Email";
            email.Body = new TextPart(TextFormat.Html) { Text = emailBody };

            var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 465, true);

            smtp.Authenticate("no.reply.dyno.app@gmail.com", "Dyno123%%%");

            smtp.Send(email);
            smtp.Disconnect(true);

            return true;
        }
    }
}
