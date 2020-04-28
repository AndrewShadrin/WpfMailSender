using CodePasswordDLL;
using System.Collections.Generic;

namespace WpfMailSender
{
    public static class VariablesClass
    {
        public static Dictionary<string, string> Senders
        {
            get { return dicSenders; }
        }
        private static Dictionary<string, string> dicSenders = new Dictionary<string, string>()
        {
            { "79257443993@yandex.ru", CodePassword.getPassword("1234l;i") },
            { "sok74@yandex.ru", CodePassword.getPassword(";liq34tjk") }
        };
    }

}
