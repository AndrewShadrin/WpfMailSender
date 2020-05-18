using DocumentFormat.OpenXml.Wordprocessing;
using System;

namespace WpfMailSender.Model
{
    public static class AppConfigClass
    {
        public static string ServerName = "smtp.yandex.ru";
        public static int ServerPort = 25; //465 не работает

        public static BorderValues BorderValuesType = BorderValues.Single;
        public static UInt32 BorderSize = 4;
    }
}
