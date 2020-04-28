using EmailSend;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;

namespace WpfMailSender
{
    /// <summary>
    /// Класс-планировщик, который создает расписание, следит за его выполнением и напоминает о событиях
    /// Также помогает автоматизировать рассылку писем в соответствии с расписанием
    /// </summary>
    class SchedulerClass
    {
        DispatcherTimer timer = new DispatcherTimer();
        EmailSendService emailSender;
        DateTime dtSend;
        IQueryable<Email> emails;
        TextRange text;
        string subject;

        /// <summary>
        /// Метод, который превращает строку из текстбокса tbTimePicker в TimeSpan
        /// </summary>
        /// <param name="strSendTime"></param>
        /// <returns></returns>
        public TimeSpan GetSendTime(string strSendTime)
        {
            TimeSpan tsSendTime = new TimeSpan();
            try
            {
                tsSendTime = TimeSpan.Parse(strSendTime);
            }
            catch { }
            return tsSendTime;
        }

        /// <summary>
        /// Отправка писем адресатам
        /// </summary>
        /// <param name="dtSend">Дата отправки</param>
        /// <param name="emailSender">Адрес отправителя</param>
        /// <param name="emails">Список получателей</param>
        /// <param name="subject">Тема письма</param>
        /// <param name="textRange">Содержание письма</param>
        public void SendEmails(DateTime dtSend, EmailSendService emailSender, IQueryable<Email> emails, string subject, TextRange textRange)
        {
            this.emailSender = emailSender;
            this.dtSend = dtSend;
            this.emails = emails;
            text = textRange;
            this.subject = subject;
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }
        
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (dtSend.ToShortTimeString() == DateTime.Now.ToShortTimeString())
            {
                foreach (Email email in emails)
                {
                    emailSender.Send(email.Value, subject, text.Text);
                }
                timer.Stop();
                MessageBox.Show("Письма отправлены.");
            }
        }
    }

}
