using System;

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
