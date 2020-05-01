using EmailSend;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataAccessService servProxy)
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            serviceProxy = servProxy;
            emails = new ObservableCollection<Email>();
            ReadAllCommand = new RelayCommand(GetEmails);
            servers = new ObservableCollection<Servers>();
            servers = serviceProxy.GetServers();
            SaveCommand = new RelayCommand(SaveEmail);
            SendEmailsCommand = new RelayCommand(SendEmails);
            emailInfo = new Email();
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
                //using (var emailSendService = new EmailSendService(cbServerSelect.Text, Int32.Parse(cbServerSelect.SelectedValue.ToString()), cbSenderSelect.Text, cbSenderSelect.Text, pbPassword.Password))
                //{
                //    TextRange text = new TextRange(MessageBody.Document.ContentStart, MessageBody.Document.ContentEnd);
                //    foreach (Email email in Emails)
                //    {
                //        emailSendService.Send(email.Value, Subject.Text, text.Text);
                //    }
                //}
                MessageWindow windowMessage = new MessageWindow("Все отлично!");
                windowMessage.Show();
            }
            catch (Exception ex)
            {
                MessageWindow windowMessage = new MessageWindow(ex.Message);
                windowMessage.Show();
            }

        }
    }
}