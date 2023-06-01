using System.Windows;
using Settings = AdvanceHelperWPF.Settings;
using AHlibrary;
using System;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Collections.ObjectModel;

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
            if (UserStatus == "администратор бд")
            {
                MainBtn.Visibility = Visibility.Hidden;
                SettingsBtn.Visibility = Visibility.Visible;
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
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
            ObservableCollection<DataGridColumn> columns = dataGrid.Columns;
            // Очистка элементов ComboBox
            SelectCond.Items.Clear();
            // Добавление названий колонок в ComboBox
            foreach (DataGridColumn column in columns)
            {
                SelectCond.Items.Add(column.Header);
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectTablesComboBox.Text == "Пользователи")
                dBconnect.SaveUsersTable();
            else
                dBconnect.SaveTable();
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings(userLogin, userStatus);
            settings.Show();
            this.Close();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dBconnect.TableSearch(dataGrid, SelectTablesComboBox.Text.ToLower(), SelectCond.Text, Search.Text);
            }
            catch (Exception ex) { System.Windows.MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, (MessageBoxImage)MessageBoxIcon.Error); }

        }
    }
}
