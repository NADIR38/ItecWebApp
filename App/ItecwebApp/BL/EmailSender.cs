using System.Net;
using System.Net.Mail;

namespace ItecwebApp.BL
{
    public class EmailSender
    {
        public static void Send(string to, string subject, string body)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("", ""),
                EnableSsl = true
            };

            client.Send("", to, subject, body);
        }
    }
}
