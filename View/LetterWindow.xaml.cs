using System.Windows;
using WpfMailSender.Model;

namespace WpfMailSender.View
{
    /// <summary>
    /// Логика взаимодействия для LetterWindow.xaml
    /// </summary>
    public partial class LetterWindow : Window
    {
        public LetterWindow(ILetter letter)
        {
            InitializeComponent();
            DataContext = letter;
        }
    }
}
