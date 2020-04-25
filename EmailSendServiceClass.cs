using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace WpfMailSender
{
    public sealed class EmailSendServiceClass
    {
        public void Send(string sender, string reciever, string subject, string mailbody, string server, int port, string password)
        {
            try
            {
                using (var mail = new MailMessage(sender, reciever))
                {
                    mail.Subject = subject;
                    mail.Body = mailbody;
                    mail.IsBodyHtml = false;
                    using (var sc = new SmtpClient(server, port))
                    {
                        sc.EnableSsl = true;
                        sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                        sc.UseDefaultCredentials = false;
                        sc.Credentials = new NetworkCredential(sender, password);
                        sc.Send(mail);
                    }
                }
            }
            catch (SmtpException ex)
            {
                throw new SmtpException(ex.Message);
            }
        }
    }
}
