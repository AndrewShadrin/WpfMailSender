using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSender.Model
{
    public class Letter : ILetter
    {
        public DateTime SendTime{ get; set; }
        public Email Reciever { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
