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

namespace WpfMailSender.View
{
    /// <summary>
    /// Логика взаимодействия для ListLettersElementView.xaml
    /// </summary>
    public partial class ListLettersElementView : UserControl
    {
        public ListLettersElementView()
        {
            InitializeComponent();
            ((FrameworkElement)Content).DataContext = this;
        }
    }
}
