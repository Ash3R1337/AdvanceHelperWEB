using System.Windows;
using System.Windows.Controls;
using Menu = AdvanceHelperWEB.Menu;
using AHlibrary;
using System.Diagnostics;

namespace AdvanceHelperWPF
{
    /// <summary>
    /// Логика взаимодействия для TeachersPortfolio.xaml
    /// </summary>
    public partial class TeachersPortfolio : Window
    {
        private TeacherPortfolioViewModel viewModel;
        public TeachersPortfolio(string UserLogin)
        {
            InitializeComponent();
            viewModel = new TeacherPortfolioViewModel();
            DataContext = viewModel;
            // Загрузка изображения преподавателя по умолчанию
            viewModel.ImageSource = viewModel.SelectedTeacher.ImagePath;
           // DBconnect dBconnect = new DBconnect();
           // dBconnect.FillCombobox(TeacherSelector, "ФИО", "преподаватели");
            userLogin = UserLogin;
            labelLogin.Content = UserLogin;
        }
        string userLogin;

        private void TeacherSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Обновление изображения при выборе нового преподавателя
            viewModel.SelectedTeacher = (Teacher)TeacherSelector.SelectedItem;
            viewModel.ImageSource = viewModel.SelectedTeacher.ImagePath;
        }

        private void MainBtn_Click(object sender, RoutedEventArgs e)
        {
           Menu menu = new Menu(userLogin);
           menu.Show();
           this.Close();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
