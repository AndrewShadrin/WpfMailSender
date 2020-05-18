using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using EmailSend;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
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
        ObservableCollection<Server> servers;
        Email emailInfo;
        string searchName;
        ObservableCollection<Letter> letters;
        Server server;
        string messageText;
        string subject;
        private KeyValuePair<string, string> sender;

        #endregion

        #region Properties

        public RelayCommand ReadAllCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand SendEmailsCommand { get; private set; }
        public RelayCommand ExportReportCommand { get; private set; }
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
        public ObservableCollection<Server> Servers
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
        public Server Server { get => server; set => server = value; }
        public string MessageText { get => messageText; set => messageText = value; }
        public string Subject { get => subject; set => subject = value; }
        public KeyValuePair<string, string> Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataAccessService accessService)
        {
            serviceProxy = accessService;
            emails = new ObservableCollection<Email>();
            servers = new ObservableCollection<Server>();
            letters = new ObservableCollection<Letter>();
            emailInfo = new Email();
            server = new Server();
            servers = serviceProxy.GetServers();
            ReadAllCommand = new RelayCommand(GetEmails);
            SaveCommand = new RelayCommand(SaveEmail);
            SendEmailsCommand = new RelayCommand(SendEmails);
            ExportReportCommand = new RelayCommand(ExportReport);
        }

        private void ExportReport()
        {
            using (WordprocessingDocument doc
                = WordprocessingDocument.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Report.docx"), WordprocessingDocumentType.Document))
            {
                // Add a main document part. 
                MainDocumentPart mainPart = doc.AddMainDocumentPart();

                // Create the document structure and add some text.
                mainPart.Document = new Document();

                Body body = mainPart.Document.AppendChild(new Body());
                Paragraph para = body.AppendChild(new Paragraph());
                Run run = para.AppendChild(new Run());
                run.AppendChild(new Text("Список получателей почтовой рассылки"));

                // Create an empty table.
                Table table = new Table();

                // Create a TableProperties object and specify its border information.
                TableProperties tblProp = new TableProperties(
                    new TableBorders(
                        new TopBorder()
                        {
                            Val = new EnumValue<BorderValues>(AppConfigClass.BorderValuesType),
                            Size = AppConfigClass.BorderSize
                        },
                        new BottomBorder()
                        {
                            Val = new EnumValue<BorderValues>(AppConfigClass.BorderValuesType),
                            Size = AppConfigClass.BorderSize
                        },
                        new LeftBorder()
                        {
                            Val = new EnumValue<BorderValues>(AppConfigClass.BorderValuesType),
                            Size = AppConfigClass.BorderSize
                        },
                        new RightBorder()
                        {
                            Val = new EnumValue<BorderValues>(AppConfigClass.BorderValuesType),
                            Size = AppConfigClass.BorderSize
                        },
                        new InsideHorizontalBorder()
                        {
                            Val = new EnumValue<BorderValues>(AppConfigClass.BorderValuesType),
                            Size = AppConfigClass.BorderSize
                        },
                        new InsideVerticalBorder()
                        {
                            Val = new EnumValue<BorderValues>(AppConfigClass.BorderValuesType),
                            Size = AppConfigClass.BorderSize
                        }
                    )
                );

                // Append the TableProperties object to the empty table.
                table.AppendChild<TableProperties>(tblProp);

                AddRowDataToTable(table, "Имя получателя", "Email получателя");

                foreach (Email email in emails)
                {
                    AddRowDataToTable(table, email.Name, email.Value);
                }

                // Append the table to the document.
                doc.MainDocumentPart.Document.Body.Append(table);
            }
        }

        private static void AddRowDataToTable(Table table, string text1cell, string text2cell)
        {
            // Create a row.
            TableRow tr = new TableRow();

            // Create a cell.
            TableCell tc1 = new TableCell();

            // Specify the table cell content.
            tc1.Append(new Paragraph(new Run(new Text(text1cell))));

            // Append the table cell to the table row.
            tr.Append(tc1);

            // Create a second table cell
            TableCell tc2 = new TableCell();

            // Specify the table cell content.
            tc2.Append(new Paragraph(new Run(new Text(text2cell))));

            // Append the table cell to the table row.
            tr.Append(tc2);

            // Append the table row to the table.
            table.Append(tr);
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
            EmailInfo.Id = serviceProxy.AddEmail(emailInfo);
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