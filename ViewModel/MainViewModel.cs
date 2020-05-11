using EmailSend;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Documents;
using WpfMailSender.Model;
using WpfMailSender.Services;

namespace WpfMailSender.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Data

        IDataAccessService serviceProxy;
        ObservableCollection<Email> emails;
        ObservableCollection<Servers> servers;
        Email emailInfo;
        string searchName;
        ObservableCollection<Letter> letters;
        Servers server;
        string messageText;
        string subject;

        #endregion

        #region Properties

        public RelayCommand ReadAllCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand SendEmailsCommand { get; set; }
        public Email EmailInfo
        {
            get { return emailInfo; }
            set
            {
                emailInfo = value;
                RaisePropertyChanged(nameof(EmailInfo));
            }
        }
        public ObservableCollection<Email> Emails
        {
            get { return emails; }
            set
            {
                emails = value;
                RaisePropertyChanged(nameof(Emails));
            }
        }
        public ObservableCollection<Servers> Servers
        {
            get => servers;
            set
            {
                servers = value;
                RaisePropertyChanged(nameof(Servers));
            }
        }
        public string SearchName 
        {
            get
            {
                return searchName;
            }
            set 
            {
                searchName = value;
                GetEmailsFiltered();
            }
        }
        public ObservableCollection<Letter> Letters { get => letters; set => letters = value; }

        public Servers Server { get => server; set => server = value; }
        public string MessageText { get => messageText; set => messageText = value; }
        public string Subject { get => subject; set => subject = value; }

        private KeyValuePair<string, string> sender;

        public KeyValuePair<string, string> Sender
        {
            get { return sender; }
            set { sender = value; }
        }


        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataAccessService servProxy)
        {
            serviceProxy = servProxy;
            emails = new ObservableCollection<Email>();
            ReadAllCommand = new RelayCommand(GetEmails);
            servers = new ObservableCollection<Servers>();
            servers = serviceProxy.GetServers();
            SaveCommand = new RelayCommand(SaveEmail);
            SendEmailsCommand = new RelayCommand(SendEmails);
            emailInfo = new Email();
            letters = new ObservableCollection<Letter>();
            server = new Servers();
            sender = VariablesClass.Senders.ElementAt(0);
        }

        internal void AddLetter()
        {
            letters.Add(new Letter() { SendTime = DateTime.Now, Subject = "Hellow it's me", Message = "Hellow, world!" });
        }

        void GetEmails()
        {
            Emails.Clear();
            foreach (var item in serviceProxy.GetEmails())
            {
                Emails.Add(item);
            }
        }

        void GetEmailsFiltered()
        {
            Emails.Clear();
            foreach (var item in serviceProxy.GetEmails().Where(e => e.Name.ToUpper().Contains(SearchName.ToUpper())))
            {
                Emails.Add(item);
            }
        }

        void SaveEmail()
        {
            EmailInfo.Id = serviceProxy.CreateEmail(emailInfo);
            if (EmailInfo.Id != 0)
            {
                Emails.Add(EmailInfo);
                RaisePropertyChanged(nameof(EmailInfo));
            }
        }

        void SendEmails()
        {
            try
            {
                EmailSendService emailSender = new EmailSendService(server.Name, (int)server.Port, sender.Key, sender.Key, CodePasswordDLL.CodePassword.GetPassword(sender.Value));
                foreach (Email email in emails)
                {
                    Thread thread = new Thread(new ParameterizedThreadStart(SendMail));
                    MailStruct mailStruct = new MailStruct { Email = email, SendService = emailSender, Subject = subject, MessageText = messageText };
                    thread.Start(mailStruct);
                }
                MessageWindow windowMessage = new MessageWindow("Все отправили!");
                windowMessage.Show();
            }
            catch (Exception ex)
            {
                MessageWindow windowMessage = new MessageWindow(ex.Message);
                windowMessage.Show();
            }

        }

        void SendMail(Object param)
        {
            var mailStruct = (MailStruct)param;
            EmailSendService sendService = mailStruct.SendService;
            sendService.Send(mailStruct.Email.Value, mailStruct.Subject, mailStruct.MessageText);
        }
    }
}