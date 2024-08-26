using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Formular
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NavigateButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SecondPage());
        }
    }
}
