using EmailSend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfMailSender
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cbSenderSelect.ItemsSource = VariablesClass.Senders;
            cbSenderSelect.DisplayMemberPath = "Key";
            cbSenderSelect.SelectedValuePath = "Value";
            DBClass db = new DBClass();
            dgRecievers.ItemsSource = db.Emails;
            cbServerSelect.ItemsSource = db.Servers;
            cbServerSelect.DisplayMemberPath = "Name";
            cbServerSelect.SelectedValuePath = "Port";
            STB.Items = db.Servers;
            STB.DisplayMemberPath = "Name";
            STB.SelectedValuePath = "Port";
            STB.LblSelectText = "Выбрать сервер";
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var emailSendService = new EmailSendService(cbServerSelect.Text, Int32.Parse(cbServerSelect.SelectedValue.ToString()), cbSenderSelect.Text, cbSenderSelect.Text, pbPassword.Password))
                {
                    TextRange text = new TextRange(MessageBody.Document.ContentStart, MessageBody.Document.ContentEnd);
                    foreach (Email email in (IQueryable<Email>)dgRecievers.ItemsSource)
                    {
                        emailSendService.Send(email.Value, Subject.Text, text.Text);
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageWindow windowMessage = new MessageWindow(ex.Message);
                windowMessage.Show();
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            SchedulerClass sc = new SchedulerClass();
            TimeSpan tsSendTime = sc.GetSendTime(tbTimePicker.Text);
            if (tsSendTime == new TimeSpan())
            {
                MessageBox.Show("Некорректный формат даты");
                return;
            }
            DateTime dtSendDateTime = (cldSchedulDateTimes.SelectedDate ?? DateTime.Today).Add(tsSendTime);
            if (dtSendDateTime < DateTime.Now)
            {
                MessageBox.Show("Дата и время отправки писем не могут быть раньше, чем настоящее время");
                return;
            }
            EmailSendService emailSender = new EmailSendService(cbServerSelect.Text, Int32.Parse(cbServerSelect.SelectedValue.ToString()), cbSenderSelect.Text, cbSenderSelect.Text, pbPassword.Password);
            sc.SendEmails(dtSendDateTime, emailSender, (IQueryable<Email>)dgRecievers.ItemsSource, Subject.Text, new TextRange(MessageBody.Document.ContentStart, MessageBody.Document.ContentEnd));

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tscTabSwitcher_btnNextClick(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 1;
        }

    }
}
