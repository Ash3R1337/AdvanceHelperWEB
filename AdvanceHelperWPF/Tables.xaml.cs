using System.Windows;
using AHlibrary;

namespace AdvanceHelperWEB
{
    /// <summary>
    /// Логика взаимодействия для Tables.xaml
    /// </summary>
    public partial class Tables : Window
    {
        DBconnect dBconnect = new DBconnect();
        string userLogin;
        string userStatus;
        public Tables(string UserLogin, string UserStatus)
        {
            InitializeComponent();
            SelectTablesComboBox.Items.Add("Материалы");
            SelectTablesComboBox.Items.Add("Подразделение");
            SelectTablesComboBox.Items.Add("Специальности");
            SelectTablesComboBox.Items.Add("Предметы");
            SelectTablesComboBox.Items.Add("Преподаватели");
            SelectTablesComboBox.Items.Add("Грамоты");
            SelectTablesComboBox.Items.Add("Пользователи");
            labelLogin.Content = UserLogin;
            userLogin = UserLogin;
            userStatus = UserStatus;
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MainBtn_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu(userLogin, userStatus);
            menu.Show();
            this.Close();
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            dBconnect.DB(SelectTablesComboBox.Text.ToLower(), dataGrid);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            dBconnect.SaveTable();
            MessageBox.Show("Таблица была успешно сохранена");
        }
    }
}
