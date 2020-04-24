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
        EmailSendServiceClass emailSendService = new EmailSendServiceClass();
        public MainWindow()
        {
            InitializeComponent();
            Server.Text = AppConfigClass.ServerName;
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            emailSendService.Send(Sender.Text, Reciever.Text, Subject.Text, MessageBody.Document.ToString(), Server.Text, AppConfigClass.ServerPort, Password.Password);
        }
    }
}
