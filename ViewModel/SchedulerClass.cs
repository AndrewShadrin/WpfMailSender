using EmailSend;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
using WpfMailSender.Model;

namespace WpfMailSender.ViewModel
{
    /// <summary>
    /// Класс-планировщик, который создает расписание, следит за его выполнением и напоминает о событиях
    /// Также помогает автоматизировать рассылку писем в соответствии с расписанием
    /// </summary>
    public class SchedulerClass
    {
        DispatcherTimer timer = new DispatcherTimer();
        EmailSendService emailSender;
        DateTime dtSend;
        ObservableCollection<Email> emails;
        string text;
        string subject;

        ObservableCollection<ILetter> letters;

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
        public void SendEmails(DateTime dtSend, EmailSendService emailSender, ObservableCollection<Email> emails, string subject, string textRange)
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

        public void SendEmails(EmailSendService emailSender, ObservableCollection<ILetter> letters)
        {
            this.letters = letters;
            this.emailSender = emailSender;
            timer.Tick += TimerTickLetter;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (dtSend.ToShortTimeString() == DateTime.Now.ToShortTimeString())
            {
                foreach (Email email in emails)
                {
                    emailSender.Send(email.Value, subject, text);
                }
                timer.Stop();
                MessageBox.Show("Письма отправлены.");
            }
        }

        private void TimerTickLetter(object sender, EventArgs e)
        {
            foreach (ILetter letter in letters)
            {
                if (letter.SendTime <= DateTime.Now)
                {
                    emailSender.Send(letter.Reciever.Value, letter.Subject, letter.Message);
                }
                timer.Stop();
                MessageBox.Show("Письма отправлены.");
            }
            foreach (ILetter item in letters.Where(lte => lte.SendTime <= DateTime.Now))
            {
                letters.Remove(item);
            }
            timer.Stop();
            MessageBox.Show("Письма отправлены.");
        }
    }

}
