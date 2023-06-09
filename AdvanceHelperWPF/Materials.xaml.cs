using System.Windows;
using Menu = AdvanceHelperWEB.Menu;
using MainWindow = AdvanceHelperWEB.MainWindow;
using AHlibrary;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Linq;
using System.ComponentModel;

namespace AdvanceHelperWPF
{
    /// <summary>
    /// Логика взаимодействия для Materials.xaml
    /// </summary>
    public partial class Materials : Window
    {
        private TeacherPortfolioViewModel viewModel;
        DBconnect dBconnect = new DBconnect();
        string userLogin;
        string userStatus;
        public Materials(string UserLogin, string UserStatus)
        {
            InitializeComponent();
            userLogin = UserLogin;
            userStatus = UserStatus;
            labelLogin.Content = UserLogin;
            viewModel = new TeacherPortfolioViewModel();
            DataContext = viewModel;
            MaterialId.ToolTip = "Автоматически сгенерированный номер материала";

            dBconnect.FillCombobox(SubdivisionSelector, "Цикловая_комиссия", "подразделение");
            SubdivisionSelector.SelectedIndex = 0;

            dBconnect.FillCombobox(SubjectSelector, "Индекс", "предметы");
            SubjectSelector.SelectedIndex = 0;

            dBconnect.ShowMaterials("материалы", dataGrid); //Добавление таблицы

            SortTable(); //Сортировка таблицы

            if (dataGrid.Items.Count > 0)
                MaterialId.Text = (dBconnect.GetLastId("Код_Материала", "материалы") + 1).ToString(); //Обновление кода
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.Columns[1].Header = "Подразделение";
            dataGrid.Columns[2].Header = "Предмет";
            dataGrid.Columns[3].Header = "Преподаватель";
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
            this.Close();
            menu.Show();
        }

        private void AddMaterialBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string SubdivisionName = SubdivisionSelector.Text;
                string SubjectName = SubjectSelector.Text;
                string TeacherName = TeacherSelector.Text;
                int TitleRp = Convert.ToInt32(TitleRP.IsChecked);
                int Rp = Convert.ToInt32(RP.IsChecked);
                int TitleFos = Convert.ToInt32(TitleFOS.IsChecked);
                int Fos = Convert.ToInt32(FOS.IsChecked);
                int VnutrREC = Convert.ToInt32(VnutrRec.IsChecked);
                int expZakl = Convert.ToInt32(ExpZakl.IsChecked);
                int vsrs = Convert.ToInt32(VSRS.IsChecked);
                int mupr = Convert.ToInt32(MUPR.IsChecked);
                dBconnect.AddMaterial(SubdivisionName, SubjectName, TeacherName, TitleRp, Rp, TitleFos, Fos, VnutrREC, expZakl, vsrs, mupr);
                MessageBox.Show("Новый материал бы успешно добавлен");
                if (dataGrid.Items.Count > 0)
                {
                    MaterialId.Text = (dBconnect.GetLastId("Код_Материала", "материалы") + 1).ToString(); //Обновление кода
                }
                dBconnect.ShowMaterials("материалы", dataGrid);
                SortTable(); //Сортировка таблицы
            }
            catch (Exception ex) { MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", (MessageBoxButton)System.Windows.Forms.MessageBoxButtons.OK, (MessageBoxImage)System.Windows.Forms.MessageBoxIcon.Error); }
        }

        private void TeacherSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void SubjectSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void SubdivisionSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            dBconnect.SaveMaterialsTable(dataGrid);
            SortTable(); //Сортировка таблицы
            if (dataGrid.Items.Count > 0)
            {
                MaterialId.Text = (dBconnect.GetLastId("Код_Материала", "материалы") + 1).ToString(); //Обновление кода
            }
        }

        private void SortTable()
        {
            DataGridColumn column = dataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "Код_Материала");

            if (column != null)
            {
                // Очистка существующей сортировки
                dataGrid.Items.SortDescriptions.Clear();

                // Добавление сортировки по столбцу "Код_Материала" в порядке убывания
                dataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, ListSortDirection.Descending));

                // Применение сортировки
                dataGrid.Items.Refresh();
            }
        }

        private void ClearMaterialBtn_Click(object sender, RoutedEventArgs e)
        {
            SubdivisionSelector.SelectedIndex = 0;
            SubjectSelector.SelectedIndex = 0;
            TeacherSelector.SelectedIndex = 0;
            TitleRP.IsChecked = false;
            RP.IsChecked = false;
            TitleFOS.IsChecked = false;
            FOS.IsChecked = false;
            VnutrRec.IsChecked = false;
            ExpZakl.IsChecked = false;
            VSRS.IsChecked = false;
            MUPR.IsChecked = false;
        }
    }
}
