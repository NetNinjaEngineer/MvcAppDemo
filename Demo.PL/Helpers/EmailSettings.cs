using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helpers
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            /*
				smtp.gmail.com

				Requires SSL: Yes

				Requires TLS: Yes (if available)

				Requires Authentication: Yes

				Port for TLS/STARTTLS: 587
			 */
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("me5260287@gmail.com", "mjyeiwzrjdlskqyn");
            smtpClient.Send("me5260287@gmail.com", email.To, email.Subject, email.Body);

        }
    }
}
