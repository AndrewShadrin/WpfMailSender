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
using WpfMailSender.Model;
using WpfMailSender.View;
using WpfMailSender.ViewModel;

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
            var locator = (ViewModelLocator)FindResource("Locator");
            sc.SendEmails(dtSendDateTime, emailSender, locator.Main.Emails, Subject.Text, MessageBody.Text);

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tscTabSwitcher_btnNextClick(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 1;
        }

        private void btnAddLetter_Click(object sender, RoutedEventArgs e)
        {
            var locator = (ViewModelLocator)FindResource("Locator");
            locator.Main.AddLetter();
        }

        private void lvLetters_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LetterWindow letterWindow = new LetterWindow((Letter)lvLetters.SelectedItem);
            letterWindow.Show();
        }
    }
}
