using System.Windows;
using AHlibrary;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.RegularExpressions;
namespace AdvanceHelperWEB
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            DBconnect dbconnect = new DBconnect();
            dbconnect.AuthCheck(LoginBox, passwordBox, "root");
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }

        Regex regex = new Regex(@"[^a-zA-Z0-9]");
        private void LoginBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MatchCollection matches = regex.Matches(LoginBox.Text);
            if (matches.Count > 0)
            {
                System.Windows.MessageBox.Show("Логин содержит недопустимые символы");
                LoginBox.BorderBrush = Brushes.Red;
            }
            else LoginBox.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
        }

        private void labelForgetPass_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            labelForgetPass.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 174));
            labelForgetPassInstruct.Visibility = Visibility.Visible;
        }

        private void labelForgetPass_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            labelForgetPass.Foreground = new SolidColorBrush(Color.FromRgb(3, 172, 118));
            labelForgetPassInstruct.Visibility = Visibility.Hidden;
        }
    }
}
