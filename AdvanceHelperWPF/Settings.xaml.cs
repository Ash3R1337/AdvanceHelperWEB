using System.IO;
using System.Windows;
using System.Windows.Forms;
using Menu = AdvanceHelperWEB.Menu;
using Tables = AdvanceHelperWEB.Tables;

namespace AdvanceHelperWPF
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        string DirPathStr;
        string FilesFromat;
        string dbusername;
        string dbname;
        string password;
        string userLogin;
        string userStatus;
        FileHandler fileHandler = new FileHandler();
        public Settings(string UserLogin, string UserStatus)
        {
            InitializeComponent();
            SettingsTextBlock.Text = $"Версия программы: 1.2.4\nПользователь: {UserLogin}";
            userLogin = UserLogin;
            userStatus = UserStatus;
            if (File.Exists("config.txt"))
            {
                DirPathStr = fileHandler.GetPath("config.txt", "Путь к рабочей директории = ");
                DirTextBlock.Text = $"Текущая рабочая директория: {DirPathStr}";

                FilesFromat = fileHandler.GetPath("config.txt", "Доступные форматы файлов (через запятую): ");
                TBfileFormat.Text = FilesFromat;

                dbusername = fileHandler.GetPath("config.txt", "Имя пользователя базы данных: ");
                TBdbusername.Text = dbusername;

                dbname = fileHandler.GetPath("config.txt", "Название базы данных: ");
                TBdbname.Text = dbname;

                password = fileHandler.GetPath("config.txt", "Пароль базы данных: ");
                TBpassword.Text = password;
            }
            if (UserStatus == "администратор")
            {
                PathViewBtn.IsEnabled = false;
                PathViewBtn.Cursor = System.Windows.Input.Cursors.Cross;
                PathViewBtn.ToolTip = "Недоступно для статуса Администратор";
                TBfileFormat.IsEnabled = false;
                TBfileFormat.Cursor = System.Windows.Input.Cursors.Cross;
                TBfileFormat.ToolTip = "Недоступно для статуса Администратор";
            }
            else if (UserStatus == "администратор бд")
            {
                PathViewBtn.IsEnabled = false;
                PathViewBtn.Cursor = System.Windows.Input.Cursors.Cross;
                PathViewBtn.ToolTip = "Недоступно для статуса Администратор БД";
                TBfileFormat.IsEnabled = false;
                TBfileFormat.Cursor = System.Windows.Input.Cursors.Cross;
                TBfileFormat.ToolTip = "Недоступно для статуса Администратор БД";
                MainBtn.Visibility = Visibility.Hidden;
                TablesBtn.Visibility = Visibility.Visible;
            }
        }

        private void MainBtn_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu(userLogin, userStatus);
            menu.Show();
            this.Close();
        }

        private void PathViewBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DirTextBlock.Text = $"Текущая рабочая директория: {folderBrowser.SelectedPath}";;
                DirPathStr = folderBrowser.SelectedPath;
            }
        }

        private void SaveAllBtn_Click(object sender, RoutedEventArgs e)
        {
            fileHandler.FileSave("config.txt", DirPathStr, "Путь к рабочей директории = ");
            fileHandler.FileSave("config.txt", TBfileFormat.Text, "Доступные форматы файлов (через запятую): ");
            fileHandler.FileSave("config.txt", TBdbusername.Text, "Имя пользователя базы данных: ");
            fileHandler.FileSave("config.txt", TBdbname.Text, "Название базы данных: ");
            fileHandler.FileSave("config.txt", TBpassword.Text, "Пароль базы данных: ");
            System.Windows.MessageBox.Show("Настройки были успешно сохранены");
        }

        private void TablesBtn_Click(object sender, RoutedEventArgs e)
        {
            Tables tables = new Tables(userLogin, userStatus);
            tables.Show();
            this.Close();
        }
    }
}
