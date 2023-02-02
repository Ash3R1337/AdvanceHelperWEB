using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AHlibrary;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            SelectTablesComboBox.Items.Add("materials");
            SelectTablesComboBox.Items.Add("subdivision");
            SelectTablesComboBox.Items.Add("subjects");
            SelectTablesComboBox.Items.Add("teachers");
            SelectTablesComboBox.Items.Add("users");
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
