using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Menu = AdvanceHelperWEB.Menu;
using AHlibrary;
using MainWindow = AdvanceHelperWEB.MainWindow;
using System;
using System.Windows.Media.Imaging;

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
            try
            {
                InitializeComponent();
                viewModel = new TeacherPortfolioViewModel();
                DataContext = viewModel;
                // Загрузка изображения преподавателя по умолчанию
                viewModel.ImageSource = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, viewModel.SelectedTeacher.ImagePath);
                TeacherImage.Source = new BitmapImage(new Uri(viewModel.ImageSource));
                BirthDate.Content = viewModel.SelectedTeacher.BirthDate;
                Subdivision.Text = dBconnect.GetValueByString("Цикловая_комиссия", "подразделение", "Код_подразделения", viewModel.SelectedTeacher.Subdivision);
                WorkExp.Content = viewModel.SelectedTeacher.WorkExp;
                Specialization.Text = viewModel.SelectedTeacher.Specialization;
                Phone.Content = viewModel.SelectedTeacher.Phone;
                Email.Content = viewModel.SelectedTeacher.Email;
                MaterialsCount.Content = dBconnect.GetCount("материалы", "Код_Материала", "Код_преподавателя", viewModel.SelectedTeacher.Id);
                MaterialsCountByDocs.Content = dBconnect.GetDocsCountByTeacher("Код_преподавателя", "материалы", viewModel.SelectedTeacher.Id);
                userLogin = UserLogin;
                userStatus = UserStatus;
                labelLogin.Content = UserLogin;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, (MessageBoxImage)System.Windows.Forms.MessageBoxIcon.Error);
                userLogin = UserLogin;
                userStatus = UserStatus;
                labelLogin.Content = UserLogin;
            }
        }

        private void TeacherSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Обновление изображения при выборе нового преподавателя
                viewModel.SelectedTeacher = (Teacher)TeacherSelector.SelectedItem;
                viewModel.ImageSource = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, viewModel.SelectedTeacher.ImagePath);
                TeacherImage.Source = new BitmapImage(new Uri(viewModel.ImageSource));
                BirthDate.Content = viewModel.SelectedTeacher.BirthDate;
                Subdivision.Text = dBconnect.GetValueByString("Цикловая_комиссия", "подразделение", "Код_подразделения", viewModel.SelectedTeacher.Subdivision);
                WorkExp.Content = viewModel.SelectedTeacher.WorkExp;
                Specialization.Text = viewModel.SelectedTeacher.Specialization;
                Phone.Content = viewModel.SelectedTeacher.Phone;
                Email.Content = viewModel.SelectedTeacher.Email;
                MaterialsCount.Content = dBconnect.GetCount("материалы", "Код_Материала", "Код_преподавателя", viewModel.SelectedTeacher.Id);
                MaterialsCountByDocs.Content = dBconnect.GetDocsCountByTeacher("Код_преподавателя", "материалы", viewModel.SelectedTeacher.Id);
            }
            catch (Exception ex) { MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, (MessageBoxImage)System.Windows.Forms.MessageBoxIcon.Error); }
        }

        private void MainBtn_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu(userLogin, userStatus);
            menu.Show();
            this.Close();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
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
