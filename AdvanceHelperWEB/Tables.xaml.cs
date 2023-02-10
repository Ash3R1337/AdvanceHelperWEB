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
        public Tables()
        {
            InitializeComponent();
            SelectTablesComboBox.Items.Add("Материалы");
            SelectTablesComboBox.Items.Add("Подразделение");
            SelectTablesComboBox.Items.Add("Специальности");
            SelectTablesComboBox.Items.Add("Предметы");
            SelectTablesComboBox.Items.Add("Преподаватели");
            SelectTablesComboBox.Items.Add("Пользователи");
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MainBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            dBconnect.DB("projectdb", SelectTablesComboBox.Text.ToLower(), dataGrid, "root");
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            dBconnect.SaveTable();
            MessageBox.Show("Таблица была успешно сохранена");
        }
    }
}
