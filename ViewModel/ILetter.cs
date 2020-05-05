using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSender.Model
{
    public interface ILetter
    {
        DateTime SendTime { get; set; }
        string Subject { get; set; }
        string Message { get; set; }
        Email Reciever { get; set; }
    }
}
