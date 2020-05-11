using EmailSend;

namespace WpfMailSender.Model
{
    public struct MailStruct
    {
        public EmailSendService SendService;
        public Email Email;
        public string Subject;
        public string MessageText;
    }
}
