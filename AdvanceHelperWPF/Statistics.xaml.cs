using System.Windows;
using System.Windows.Controls;
using Menu = AdvanceHelperWEB.Menu;
using AHlibrary;

namespace AdvanceHelperWPF
{
    /// <summary>
    /// Логика взаимодействия для Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {
        private TeacherPortfolioViewModel viewModel;
        DBconnect dBconnect = new DBconnect();
        string userLogin;
        string userStatus;
        public Statistics(string UserLogin, string UserStatus)
        {
            InitializeComponent();
            userLogin = UserLogin;
            userStatus = UserStatus;
            labelLogin.Content = UserLogin;
            viewModel = new TeacherPortfolioViewModel();
            DataContext = viewModel;

            //Грамоты
            Certificates.Content = dBconnect.GetCount("грамоты", "Код_грамоты", "Преподаватель", viewModel.SelectedTeacher.Id);
            CertificatesAll.Content = dBconnect.GetAllCount("грамоты", "Код_грамоты");

            //Материалы
            Materials.Content = dBconnect.GetCount("материалы", "Код_Материала", "Код_преподавателя", viewModel.SelectedTeacher.Id);
            MaterialsAll.Content = dBconnect.GetAllCount("материалы", "Код_Материала");
            dBconnect.FillCombobox(SubjectSelector, "Индекс", "предметы");
            SubjectSelector.SelectedIndex = 0;
            MaterialsBySubjects.Content = dBconnect.GetCount("материалы", "Код_Материала", "Код_предмета", SubjectSelector.SelectedIndex + 1);
            MaterialsWithAllDocuments.Content = dBconnect.GetCountByDocs("материалы");

            //Подразделение
            dBconnect.FillCombobox(SubdivisionSelector, "Цикловая_комиссия", "подразделение");
            SubdivisionSelector.SelectedIndex = 0;
            Teachers.Content = dBconnect.GetValueByString("Количество_преподавателей", "подразделение", "Код_подразделения", (SubdivisionSelector.SelectedIndex + 1).ToString());
            User.Content = dBconnect.GetUserLoginFromSubdivision("Логин", "подразделение", SubdivisionSelector.SelectedIndex + 1);

            //Пользователи
            StatusSelector.Items.Add("Администратор");
            StatusSelector.Items.Add("Методист");
            StatusSelector.Items.Add("Администратор БД");
            StatusSelector.SelectedIndex = 0;
            StatusCount.Content = dBconnect.GetCountByString("Статус", "пользователи", StatusSelector.Text.ToLower());

            //Преподаватели
            MaterialsMax.Content = dBconnect.GetMaxTeacher("ФИО", "преподаватели");
            MaterialsMin.Content = dBconnect.GetMinTeacher("ФИО", "преподаватели");
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MainBtn_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu(userLogin, userStatus);
            this.Close();
            menu.Show();
        }

        private void TeacherSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.SelectedTeacher = (Teacher)TeacherSelector.SelectedItem;
            Certificates.Content = dBconnect.GetCount("Грамоты", "Код_грамоты", "Преподаватель", viewModel.SelectedTeacher.Id);
            Materials.Content = dBconnect.GetCount("Материалы", "Код_Материала", "Код_преподавателя", viewModel.SelectedTeacher.Id);
        }

        private void SubjectSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MaterialsBySubjects.Content = dBconnect.GetCount("материалы", "Код_Материала", "Код_предмета", SubjectSelector.SelectedIndex + 1);
        }

        private void SubdivisionSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Teachers.Content = dBconnect.GetValueByString("Количество_преподавателей", "подразделение", "Код_подразделения", (SubdivisionSelector.SelectedIndex + 1).ToString());
            User.Content = dBconnect.GetUserLoginFromSubdivision("Логин", "подразделение", SubdivisionSelector.SelectedIndex + 1);
        }

        private void StatusSelector_DropDownClosed(object sender, System.EventArgs e)
        {
            StatusCount.Content = dBconnect.GetCountByString("Статус", "пользователи", StatusSelector.Text);
        }
    }
}
