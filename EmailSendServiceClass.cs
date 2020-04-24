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
                using (var mm = new MailMessage(sender, reciever))
                {
                    mm.Subject = subject;
                    mm.Body = mailbody;
                    mm.IsBodyHtml = false;
                    using (var sc = new SmtpClient(server, port))
                    {
                        sc.EnableSsl = true;
                        sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                        sc.UseDefaultCredentials = false;
                        sc.Credentials = new NetworkCredential(sender, password);
                        sc.Send(mm);
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
