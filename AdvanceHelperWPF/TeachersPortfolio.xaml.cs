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
        public TeachersPortfolio(string UserLogin, string UserStatus)
        {
            InitializeComponent();
            viewModel = new TeacherPortfolioViewModel();
            DataContext = viewModel;
            // Загрузка изображения преподавателя по умолчанию
            viewModel.ImageSource = viewModel.SelectedTeacher.ImagePath;
            BirthDate.Content = viewModel.SelectedTeacher.BirthDate;
            Subdivision.Content = viewModel.SelectedTeacher.Subdivision;
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
            Subdivision.Content = viewModel.SelectedTeacher.Subdivision;
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
            DBconnect dBconnect = new DBconnect();
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
