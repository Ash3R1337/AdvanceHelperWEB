using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Menu = AdvanceHelperWEB.Menu;
using AHlibrary;

namespace AdvanceHelperWPF
{
    /// <summary>
    /// Логика взаимодействия для TeachersPortfolio.xaml
    /// </summary>
    public partial class TeachersPortfolio : Window
    {
        private TeacherPortfolioViewModel viewModel;
        string userLogin;
        string userStatus;
        DBconnect dBconnect = new DBconnect();
        public TeachersPortfolio(string UserLogin, string UserStatus)
        {
            InitializeComponent();
            viewModel = new TeacherPortfolioViewModel();
            DataContext = viewModel;
            // Загрузка изображения преподавателя по умолчанию
            viewModel.ImageSource = viewModel.SelectedTeacher.ImagePath;
            BirthDate.Content = viewModel.SelectedTeacher.BirthDate;
            Subdivision.Text = dBconnect.GetValueByString("Цикловая_комиссия", "подразделение", "Код_подразделения", viewModel.SelectedTeacher.Subdivision);
            WorkExp.Content = viewModel.SelectedTeacher.WorkExp;
            Specialization.Text = viewModel.SelectedTeacher.Specialization;
            Phone.Content = viewModel.SelectedTeacher.Phone;
            Email.Content = viewModel.SelectedTeacher.Email;
            MaterialsCount.Content = dBconnect.GetCount("материалы", "Код_Материала", "Код_преподавателя", viewModel.SelectedTeacher.Id);
            userLogin = UserLogin;
            userStatus = UserStatus;
            labelLogin.Content = UserLogin;
        }

        private void TeacherSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Обновление изображения при выборе нового преподавателя
            viewModel.SelectedTeacher = (Teacher)TeacherSelector.SelectedItem;
            viewModel.ImageSource = viewModel.SelectedTeacher.ImagePath;
            BirthDate.Content = viewModel.SelectedTeacher.BirthDate;
            Subdivision.Text = dBconnect.GetValueByString("Цикловая_комиссия", "подразделение", "Код_подразделения", viewModel.SelectedTeacher.Subdivision);
            WorkExp.Content = viewModel.SelectedTeacher.WorkExp;
            Specialization.Text = viewModel.SelectedTeacher.Specialization;
            Phone.Content = viewModel.SelectedTeacher.Phone;
            Email.Content = viewModel.SelectedTeacher.Email;
            MaterialsCount.Content = dBconnect.GetCount("материалы", "Код_Материала", "Код_преподавателя", viewModel.SelectedTeacher.Id);
        }

        private void MainBtn_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu(userLogin, userStatus);
            menu.Show();
            this.Close();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CertificatesBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Certificate> Certificates = new List<Certificate>();
            if (viewModel.SelectedTeacher != null)
            {
                Certificates = dBconnect.GetCertificatesFromDatabase("грамоты", viewModel.SelectedTeacher.Id);
                if (Certificates.Count > 0)
                {
                    CertificatesWindow certificatesWindow = new CertificatesWindow(Certificates);
                    certificatesWindow.Show();
                }
                else MessageBox.Show("Грамоты и другие достижения не были обнаружены.");
            }
        }
    }
}
