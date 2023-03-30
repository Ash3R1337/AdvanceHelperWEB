using AdvanceHelperWPF;
using System.Windows;

namespace AdvanceHelperWEB
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu(string UserLogin)
        {
            InitializeComponent();
            labelHeaderLogin.Content = $"Добро пожаловать {UserLogin}! С чего начнем?";
            userLogin = UserLogin;
        }

        string userLogin;

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            Start start = new Start(userLogin);
            start.Show();
            this.Close();
        }

        private void Tables_Click(object sender, RoutedEventArgs e)
        {
            Tables tables = new Tables(userLogin);
            tables.Show();
            this.Close();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TeachersPortfolioBtn_Click(object sender, RoutedEventArgs e)
        {
            TeachersPortfolio teachersPortfolio = new TeachersPortfolio(userLogin);
            teachersPortfolio.Show();
            this.Close();
        }
    }
}
